using Chess.Figures;
using System;
using System.Windows;
using System.Xml.Serialization;

namespace Chess.App.Models
{
    /// <summary>
    /// Model for figures
    /// </summary>
    public class FigureModel
    {

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        [XmlElement]
        public Color Color { get; set; }

        [XmlElement]
        public Point Position { get; set; }

        [XmlElement]
        public Start Start { get; set; }

        [XmlElement]
        public bool OnStart { get; set; }

        /// <summary>
        /// Convert the figure in the model
        /// </summary>
        /// <param name="element">This must a devired type of <see cref="IFigure"/></param>
        public static explicit operator FigureModel(UIElement element)
        {
            IFigure figure = element as IFigure;

            // Is it valid
            if (element == null)
                throw new InvalidCastException(figure?.GetType().Name);

            return new FigureModel()
            {
                Type = figure.GetType().AssemblyQualifiedName,
                Name = figure.Name,
                Color = figure.Color,
                Position = figure.Position,
                Start = figure.Start,
                OnStart = figure.OnStart
            };
        }

        /// <summary>
        /// Convert the model back to the element
        /// </summary>
        /// <param name="model">The model</param>
        public static explicit operator UIElement(FigureModel model)
        {
            IFigure figure = (IFigure)Activator.CreateInstance(System.Type.GetType(model.Type));
            figure.Color = model.Color;
            figure.Position = model.Position;
            figure.Start = model.Start;
            figure.OnStart = model.OnStart;
            return (UIElement)figure;
        }
    }
}
