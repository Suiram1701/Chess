using Localization;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Chess.Figures
{
    /// <summary>
    /// Interaktionslogik für Farmer.xaml
    /// </summary>
    public partial class Farmer : UserControl, IFigure
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(Farmer), new PropertyMetadata(Color.Black));
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);

            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point), typeof(Farmer), new PropertyMetadata(new Point(0, 0)));
        public Point Position
        {
            get => (Point)GetValue(PositionProperty);

            set => SetValue(PositionProperty, value);
        }

        string IFigure.Name { get; } = LangHelper.GetString("Figure.Farmer");

        public Start Start { get; set; }

        public bool OnStart { get; set; } = true;

        public Farmer()
        {
            InitializeComponent();
        }

        public IEnumerable<Point> GetMovement(IEnumerable<(Point Position, bool isFriend)> OtherFigures)
        {
            // One forward
            if (!OtherFigures.Any(Figure => Figure.Position.X == Position.X && Figure.Position.Y == Position.Y + (Start == Start.Up ? 1 : -1)))
                yield return new Point(Position.X, Position.Y + (Start == Start.Up ? 1 : -1));

            // First move
            if (OnStart)
                // No one on field
                if (!OtherFigures.Any(Figure => Figure.Position.X == Position.X && Figure.Position.Y == Position.Y + (Start == Start.Up ? 2 : -2)))
                    yield return new Point(Position.X, Position.Y + (Start == Start.Up ? 2 : -2));

            // Enemy on left
            if (OtherFigures.Any(Figure => Figure.Position.X == Position.X - 1 && Figure.Position.Y == Position.Y + (Start == Start.Up ? 1 : -1) && !Figure.isFriend))
                yield return new Point(Position.X - 1, Position.Y + (Start == Start.Up ? 1 : -1));

            // Enemy on right
            if (OtherFigures.Any(Figure => Figure.Position.X == Position.X + 1 && Figure.Position.Y == Position.Y + (Start == Start.Up ? 1 : -1) && !Figure.isFriend))
                yield return new Point(Position.X + 1, Position.Y + (Start == Start.Up ? 1 : -1));
        }
    }
}
