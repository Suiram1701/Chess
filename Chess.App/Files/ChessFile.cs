using Chess.App.Models;
using Chess.Figures;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace Chess.App.Files
{
    [File("saves", extension: "chess")]
    public class ChessFile : FileBase<ChessFile>
    {
        [XmlElement]
        public Color CurrentColor { get; set; }
        
        [XmlElement]
        public List<MoveModel> MoveHistory { get; set; }

        [XmlElement]
        public List<FigureModel> Figures { get; set; }

        [XmlElement]
        public string Signature { get; set; }

        public override void Save()
        {
            // Built string to hash
            StringBuilder builder = new StringBuilder();
            builder.Append(CurrentColor);
            foreach (MoveModel move in MoveHistory)
                builder.Append(move.EndField);
            foreach (FigureModel figure in Figures)
                builder.Append(figure.Position);

            // Make signature
            byte[] bytes;
            using (SHA256 sha256 = SHA256.Create())
                bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()));
            Signature = Encoding.UTF8.GetString(bytes);

            base.Save();
        }

        /// <summary>
        /// Veyrfy the file with the signature
        /// </summary>
        /// <remarks>
        /// Signature will be save with <see cref="Save"/>
        /// </remarks>
        /// <returns>True if nothing is chnaged</returns>
        public bool Veryfy()
        {
            // Built string to hash
            StringBuilder builder = new StringBuilder();
            builder.Append(CurrentColor);
            foreach (MoveModel move in MoveHistory)
                builder.Append(move.EndField);
            foreach (FigureModel figure in Figures)
                builder.Append(figure.Position);

            // Make signature and compare it
            byte[] bytes;
            using (SHA256 sha256 = SHA256.Create())
                bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()));
            return Signature == Encoding.UTF8.GetString(bytes);
        }
    }
}
