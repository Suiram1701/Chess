using System;

namespace Chess.App.Files
{
    /// <summary>
    /// Define all metadata for custom files
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal sealed class FileAttribute : Attribute
    {
        public string DefaultPath { get; }

        /// <summary>
        /// The extension for the file
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// Setup metadata for custom files
        /// </summary>
        /// <param name="defaultPath">Default folder to save</param>
        /// <param name="extension">Extension to save</param>
        public FileAttribute(string defaultPath, string extension = "xml")
        {
            DefaultPath = defaultPath;
            Extension = extension;
        }
    }
}