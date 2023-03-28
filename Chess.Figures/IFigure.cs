using System.Windows;

namespace Chess.Figures
{
    public enum Color
    {
        Black,
        White
    }

    public interface IFigure
    {
        /// <summary>
        /// The team color of the figure
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// The current position in the grid
        /// </summary>
        Point Position { get; set; }
    }
}
