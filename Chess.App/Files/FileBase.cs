using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Chess.App.Files
{
    /// <summary>
    /// Base for custom xml files
    /// </summary>
    /// <typeparam name="T">The devire type</typeparam>
    /// <remarks>
    /// The devired type must a have a parameterless constructor.
    /// Class is the root element and alls properties are children and you can use attributes from <see cref="System.Xml.Serialization"/> to customize the xml document
    /// </remarks>
    public abstract class FileBase<T> : IDisposable where T : class
    {
        [XmlIgnore]
        public bool Disposed { get; private set; }

        /// <summary>
        /// The full path of the file with extension
        /// </summary>
        [XmlIgnore]
        public string FilePath { get; private set; } = string.Empty;

        [XmlIgnore]
        /// <summary>
        /// Name of the loaded file without extension
        /// </summary>
        /// <remarks>
        /// If you change this propertie the file name will be renamed as file (set and read with out extension)
        /// If the file already exist the file will be overridden and if parts of the path don't exist the parts will be created
        /// </remarks>
        public string FileName
        {
            get => _FileName;
            set
            {
                // Rename when name changed
                if (_FileName != value)
                {
                    // Create new path and create if not exist and replace it
                    string rawPath = FilePath.Remove(FilePath.LastIndexOf('\\'));
                    string path = $"{rawPath}\\{value}.{_Attribute.Extension}";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(rawPath);

                    using (FileStream fls = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        fls.SetLength(0);

                        // Copy and delete
                        using (StreamReader str = new StreamReader(FilePath))
                        {
                            byte[] lineEnd = Encoding.UTF8.GetBytes("\n");
                            while (!str.EndOfStream)
                            {
                                byte[] lineBytes = Encoding.UTF8.GetBytes(str.ReadLine());
                                fls.Write(lineBytes, 0, lineBytes.Length);
                                fls.Write(lineEnd, 0, lineEnd.Length);
                            }
                        }
                        File.Delete(FilePath);
                    }
                    
                    // Update values
                    _FileName = value;
                    FilePath = path;
                }
            }
        }
        [XmlIgnore]
        private string _FileName = string.Empty;

        /// <summary>
        /// Metadata for the file type
        /// </summary>
        [XmlIgnore]
        private FileAttribute _Attribute { get; }

        public FileBase()
        {
            // Validate file class
            _Attribute = GetType().GetCustomAttribute<FileAttribute>() ?? throw new NotImplementedException(GetType() + " must implement " + nameof(FileAttribute));
            if (!GetType().GetConstructors().Any(constructor => constructor.GetParameters().Length == 0))
                throw new NotImplementedException(nameof(FileBase<T>) + " need one parameterless constructor!");

            if (!GetType().IsPublic)
                throw new FieldAccessException(GetType() + " must be public!");
        }

        ~FileBase()
        {
            Dispose();
        }

        public void Dispose()
        {
            // Dispose all
            PropertyInfo[] baseProperties = typeof(FileBase<T>).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (PropertyInfo property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
            {
                // Only from devired or if it is readonly
                if (baseProperties.Any(prop => prop.Name == property.Name && prop.PropertyType == property.PropertyType) || !property.CanWrite)
                    continue;

                (property.GetValue(this) as IDisposable)?.Dispose();
                property.SetValue(this, null);
            }

            Disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Create a xml file from <see cref="T"/>
        /// </summary>
        /// <param name="fileName">Name of the file (without extension)</param>
        /// <param name="filePath">Path of the file (default is set in attribute)</param>
        /// <param name="values">Values to set on the instance</param>
        /// <exception cref="ArgumentException"><typeparamref name="T"/> isn't valid for this</exception>
        /// <returns>The instance</returns>
        public static T Create(string fileName, string filePath = default, params (string propertyName, object value)[] values)
        {
            // Check if given type is devired from FileBase
            if (!CalledFromDerived(typeof(T), out Exception ex))
                throw ex;

            // Setup paths
            FileBase<T> instance = Activator.CreateInstance<T>() as FileBase<T>;
            instance._FileName = fileName;
            instance.FilePath = $"{filePath ?? instance._Attribute.DefaultPath}\\{fileName}.{instance._Attribute.Extension}";

            // Set values
            (PropertyInfo property, object value)[] convertedValues = values.Select(val =>
            {
                // Convert string to propertyInfo
                PropertyInfo inf = typeof(T).GetProperty(val.propertyName) ?? throw new ArgumentException(typeof(T) + " don't contain a property named " + val.propertyName);
                return (inf, val.value);
            }).ToArray();
            foreach ((PropertyInfo property, object value) in convertedValues)
                property.SetValue(instance, value);

            instance.Save();
            return instance as T;
        }

        /// <summary>
        /// Create a xml file from <see cref="T"/>
        /// </summary>
        /// <param name="fileName">Name of the file (without extension)</param>
        /// <param name="basedOn">Content of another file</param>
        /// <param name="filePath">Path of the file (default is set in attribute)</param>
        /// <returns>The instance</returns>
        public static T Create(string fileName, T basedOn, string filePath = default)
        {
            // Check if given type is devired from FileBase
            if (!CalledFromDerived(typeof(T), out Exception ex))
                throw ex;

            FileBase<T> baseOn = basedOn as FileBase<T>;

            // Check if disposed
            if (baseOn.Disposed)
                throw new ObjectDisposedException(typeof(T).Name);

            // Setup paths
            FileBase<T> instance = Activator.CreateInstance<T>() as FileBase<T>;
            instance._FileName = fileName;
            instance.FilePath = $"{filePath ?? instance._Attribute.DefaultPath}\\{fileName}.{instance._Attribute.Extension}";

            // Set values
            PropertyInfo[] baseProperties = typeof(FileBase<T>).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                // Only from devired or if it is readonly
                if (baseProperties.Any(prop => prop.Name == property.Name && prop.PropertyType == property.PropertyType) || !property.CanWrite)
                    continue;

                property.SetValue(instance, property.GetValue(baseOn));
            }

            instance.Save();
            return instance as T;
        }

        /// <summary>
        /// Load the the given file
        /// </summary>
        /// <typeparam name="T">The derived type <see cref="FileBase"/> to load</typeparam>
        /// <param name="path">File to load</param>
        /// <exception cref="ArgumentException"><typeparamref name="T"/> isn't valid for this</exception>
        /// <returns>The loaded file can converted to your file class</returns>
        public static T Load(string path)
        {
            // Check if given type is devired from FileBase
            if (!CalledFromDerived(typeof(T), out Exception ex))
                throw ex;

            if (!File.Exists(path))
                throw new FileNotFoundException(nameof(path));

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T content = default;

            try
            {
                using (FileStream fls = new FileStream(path, FileMode.Open, FileAccess.Read))
                    content = (T)serializer.Deserialize(fls);

                if (content == null)
                    throw new SerializationException($"File \"{path}\" can't deserialized");
            }
            catch (Exception e)
            {
                content = default;
            }
            
            return content;
        }

        /// <summary>
        /// Save the current element as file
        /// </summary>
        /// <remarks>
        /// If the file was deleted it will new created and the path will be created if not exist
        /// </remarks>
        /// <exception cref="ObjectDisposedException">File was deleted</exception>
        public virtual void Save()
        {
            // Check if disposed
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            // Setup
            XmlSerializer serializer = new XmlSerializer(GetType());

            // Create path if not exist
            string path = FilePath.Remove((FilePath.Length - 1) - (FileName + _Attribute.Extension).Length);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Write
            using (FileStream fls = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fls.SetLength(0);
                serializer.Serialize(fls, this);
            }
        }

        /// <summary>
        /// Delete the loaded file
        /// </summary>
        /// <exception cref="ObjectDisposedException">File was deleted</exception>
        public virtual void Delete()
        {
            // Check if disposed
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            Dispose();

            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }

        /// <summary>
        /// Get the data as stream
        /// </summary>
        /// <returns>A memory stream</returns>
        /// <exception cref="ObjectDisposedException">File was deleted</exception>
        public Stream GetStream()
        {
            // Check if disposed
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            // Serialize
            XmlSerializer serializer = new XmlSerializer(GetType());
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, this);
            return stream;
        }

        /// <summary>
        /// Check if it called from the right derived class and had the FileAttribute
        /// </summary>
        /// <param name="t">Type to check</param>
        /// <param name="exception">Exception if any error</param>
        /// <returns>True if all is fine</returns>
        private static bool CalledFromDerived(Type t, out Exception exception)
        {
            exception = null;
            if (t.BaseType != typeof(FileBase<T>))
                exception = new ArgumentException(t + " isn't a derived type of " + nameof(FileBase<T>));
            if (t.GetCustomAttribute<FileAttribute>() == null)
                exception = new ArgumentException(t + " must implement " + nameof(FileAttribute));

            if (exception is null)
                return true;
            else
                return false;
        }
    }
}