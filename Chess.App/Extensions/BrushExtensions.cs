using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Chess.App.Extensions
{
    internal static class BrushExtensions
    {
        /// <summary>
        /// Convert the brush to a string
        /// </summary>
        /// <param name="brush"></param>
        /// <returns></returns>
        public static string ConvertToString(this Brush brush)
        {
            var settings = new XmlWriterSettings { Indent = true };
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                XamlWriter.Save(brush, xmlWriter);
                return stringWriter.ToString();
            }
        }
    }
}
