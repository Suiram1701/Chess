using Chess.App.Extensions;
using Chess.App.UserControl;
using System;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Chess.App.Models
{
    public class MoveModel
    {
        [XmlAttribute]
        public string FigureType { get; set; }

        [XmlAttribute]
        public string Team { get; set; }

        [XmlElement]
        public string FigureName { get; set; }

        [XmlElement]
        public Point StartField { get; set; }

        [XmlElement]
        public Point EndField { get; set; }

        [XmlElement]
        public string Detail { get; set; }

        [XmlElement]
        public string FieldName { get; set; }

        /// <summary>
        /// Convert the usercontrol to the model
        /// </summary>
        /// <param name="View">Usercontrol</param>
        public static explicit operator MoveModel(MoveListView View)
        {
            return new MoveModel()
            {
                FigureType = View.FigureType.Name,
                Team = View.Team.ConvertToString(),
                FigureName = View.FigureName,
                StartField = View.StartField,
                EndField = View.EndField,
                Detail = View.Detail,
                FieldName = View.FieldName
            };
        }

        /// <summary>
        /// Convert the model back to view
        /// </summary>
        /// <param name="model">Model</param>
        public static explicit operator MoveListView(MoveModel model)
        {
            return new MoveListView()
            {
                FigureType = Type.GetType(model.FigureType),
                Team = model.Team.ConvertToBrush(),
                FigureName = model.FigureName,
                StartField = model.StartField,
                EndField = model.EndField,
                Detail = model.Detail,
                FieldName = model.FieldName
            };
        }
    }
}
