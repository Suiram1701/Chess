using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Chess.Figures
{
    public enum Color
    {
        Black,
        White
    }

    public enum Start
    {
        Up,
        Down
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

        /// <summary>
        /// Start position of the figure
        /// </summary>
        Start Start { get; set; }

        /// <summary>
        /// Figure on start position
        /// </summary>
        bool OnStart { get; set; }

        /// <summary>
        /// Calculate all field to set the figure
        /// </summary>
        /// <param name="OtherFigures">Position of all figures on table</param>
        /// <returns>All field on that the figure can set</returns>
        IEnumerable<Point> GetMovement(IEnumerable<(Point Position, bool isFriend)> OtherFigures);
    }
}
