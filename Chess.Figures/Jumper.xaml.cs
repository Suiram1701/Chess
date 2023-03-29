using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Chess.Figures
{
    /// <summary>
    /// Interaktionslogik für Jumper.xaml
    /// </summary>
    public partial class Jumper : UserControl, IFigure
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(Jumper), new PropertyMetadata(Color.Black));
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);

            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point), typeof(Jumper), new PropertyMetadata(new Point(0, 0)));
        public Point Position
        {
            get => (Point)GetValue(PositionProperty);

            set => SetValue(PositionProperty, value);
        }

        public Start Start { get; set; }

        public bool OnStart { get; set; } = true;

        public Jumper()
        {
            InitializeComponent();
        }

        public IEnumerable<Point> GetMovement(IEnumerable<(Point Position, bool isFriend)> OtherFigures)
        {
            // Up
            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X - 1 && Figure.Position.Y == Position.Y - 2 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X - 1 && Figure.Position.Y == Position.Y - 2)) &&
                Position.X - 1 >= 0 && Position.X - 1 <= 7 && Position.Y - 1 >= 0 && Position.Y - 2 <= 7)     // Inside game table
                yield return new Point(Position.X - 1, Position.Y - 2);     // Left

            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X + 1 && Figure.Position.Y == Position.Y - 2 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X + 1 && Figure.Position.Y == Position.Y - 2)) &&
                Position.X + 1 >= 0 && Position.X + 1 <= 7 && Position.Y - 2 >= 0 && Position.Y - 2 <= 7)     // Inside game table
                yield return new Point(Position.X + 1, Position.Y - 2);     // Right

            // Down
            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X - 1 && Figure.Position.Y == Position.Y + 2 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X - 1 && Figure.Position.Y == Position.Y + 2)) &&
                Position.X - 1 >= 0 && Position.X - 1 <= 7 && Position.Y + 2 >= 0 && Position.Y + 2 <= 7)     // Inside game table
                yield return new Point(Position.X - 1, Position.Y + 2);     // Left

            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X + 1 && Figure.Position.Y == Position.Y + 2 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X + 1 && Figure.Position.Y == Position.Y + 2)) &&
                Position.X + 1 >= 0 && Position.X + 1 <= 7 && Position.Y + 2 >= 0 && Position.Y + 2 <= 7)     // Inside game table
                yield return new Point(Position.X + 1, Position.Y + 2);     // Right

            // Left
            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X - 2 && Figure.Position.Y == Position.Y - 1 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X - 2 && Figure.Position.Y == Position.Y - 1)) &&
                Position.X - 2 >= 0 && Position.X - 2 <= 7 && Position.Y - 1 >= 0 && Position.Y - 1 <= 7)     // Inside game table
                yield return new Point(Position.X - 2, Position.Y - 1);     // Up

            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X - 2 && Figure.Position.Y == Position.Y + 1 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X - 2 && Figure.Position.Y == Position.Y + 1)) &&
                Position.X - 2 >= 0 && Position.X - 2 <= 7 && Position.Y + 1 >= 0 && Position.Y + 1 <= 7)     // Inside game table
                yield return new Point(Position.X - 2, Position.Y + 1);     // Down

            // Right
            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X + 2 && Figure.Position.Y == Position.Y - 1 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X + 2 && Figure.Position.Y == Position.Y - 1)) &&
                Position.X + 2 >= 0 && Position.X + 2 <= 7 && Position.Y - 1 >= 0 && Position.Y - 1 <= 7)     // Inside game table
                yield return new Point(Position.X + 2, Position.Y - 1);     // Up

            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X + 2 && Figure.Position.Y == Position.Y + 1 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X + 2 && Figure.Position.Y == Position.Y + 1)) &&
                Position.X + 2 >= 0 && Position.X + 2 <= 7 && Position.Y + 1 >= 0 && Position.Y + 1 <= 7)     // Inside game table
                yield return new Point(Position.X + 2, Position.Y + 1);     // Down
        }
    }
}
