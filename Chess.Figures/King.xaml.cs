using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Chess.Figures
{
    /// <summary>
    /// Interaktionslogik für King.xaml
    /// </summary>
    public partial class King : UserControl, IFigure
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(King), new PropertyMetadata(Color.Black));
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);

            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point), typeof(King), new PropertyMetadata(new Point(0, 0)));
        public Point Position
        {
            get => (Point)GetValue(PositionProperty);

            set => SetValue(PositionProperty, value);
        }

        string IFigure.Name { get; } = "König";

        public Start Start { get; set; }

        public bool OnStart { get; set; } = true;

        public King()
        {
            InitializeComponent();
        }

        public IEnumerable<Point> GetMovement(IEnumerable<(Point Position, bool isFriend)> OtherFigures)
        {
            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X - 1 && Figure.Position.Y == Position.Y && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X - 1  && Figure.Position.Y == Position.Y)) &&
                Position.X - 1 >= 0 && Position.X - 1 <= 7 && Position.Y >= 0 && Position.Y <= 7)     // Inside game table
                yield return new Point(Position.X - 1, Position.Y);     // Left

            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X - 1 && Figure.Position.Y == Position.Y - 1 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X - 1 && Figure.Position.Y == Position.Y - 1)) &&
                Position.X - 1 >= 0 && Position.X - 1 <= 7 && Position.Y - 1 >= 0 && Position.Y - 1 <= 7)     // Inside game table
                yield return new Point(Position.X - 1, Position.Y - 1);     // Left-Top

            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X && Figure.Position.Y == Position.Y - 1 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X && Figure.Position.Y == Position.Y - 1)) &&
                Position.X >= 0 && Position.X <= 7 && Position.Y - 1 >= 0 && Position.Y - 1 <= 7)     // Inside game table
                yield return new Point(Position.X, Position.Y - 1);     // Top

            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X + 1 && Figure.Position.Y == Position.Y - 1 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X + 1 && Figure.Position.Y == Position.Y - 1)) &&
                Position.X + 1 >= 0 && Position.X + 1 <= 7 && Position.Y - 1 >= 0 && Position.Y - 1 <= 7)     // Inside game table
                yield return new Point(Position.X + 1, Position.Y - 1);     // Right-Top

            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X + 1 && Figure.Position.Y == Position.Y && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X + 1 && Figure.Position.Y == Position.Y)) &&
                Position.X + 1 >= 0 && Position.X + 1 <= 7 && Position.Y >= 0 && Position.Y <= 7)     // Inside game table
                yield return new Point(Position.X + 1, Position.Y);     // Right

            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X + 1 && Figure.Position.Y == Position.Y + 1 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X + 1 && Figure.Position.Y == Position.Y + 1)) &&
                Position.X + 1 >= 0 && Position.X + 1 <= 7 && Position.Y + 1 >= 0 && Position.Y + 1 <= 7)     // Inside game table
                yield return new Point(Position.X + 1, Position.Y + 1);     // Right-Down

            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X && Figure.Position.Y == Position.Y + 1 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X && Figure.Position.Y == Position.Y + 1)) &&
                Position.X >= 0 && Position.X <= 7 && Position.Y + 1 >= 0 && Position.Y + 1 <= 7)     // Inside game table
                yield return new Point(Position.X, Position.Y + 1);     // Down

            if ((OtherFigures.Any(Figure => Figure.Position.X == Position.X - 1 && Figure.Position.Y == Position.Y + 1 && !Figure.isFriend) ||
                !OtherFigures.Any(Figure => Figure.Position.X == Position.X - 1 && Figure.Position.Y == Position.Y + 1)) &&
                Position.X - 1 >= 0 && Position.X - 1 <= 7 && Position.Y + 1 >= 0 && Position.Y + 1 <= 7)     // Inside game table
                yield return new Point(Position.X - 1, Position.Y + 1);     // Left-Down
        }
    }
}
