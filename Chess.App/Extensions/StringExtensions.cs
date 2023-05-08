using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Chess.App.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Convert a xaml string to the brush
        /// </summary>
        /// <param name="brushString"></param>
        /// <returns></returns>
        public static Brush ConvertToBrush(this string brushString)
        {
            using (var stringReader = new StringReader(brushString))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                return (Brush)XamlReader.Load(xmlReader);
            }
        }
    }
}
