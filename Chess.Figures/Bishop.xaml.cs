using Localization;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Chess.Figures
{
    /// <summary>
    /// Interaktionslogik für Bishop.xaml
    /// </summary>
    public partial class Bishop : UserControl, IFigure
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(Bishop), new PropertyMetadata(Color.Black));
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);

            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point), typeof(Bishop), new PropertyMetadata(new Point(0, 0)));
        public Point Position
        {
            get => (Point)GetValue(PositionProperty);

            set => SetValue(PositionProperty, value);
        }

        string IFigure.Name { get; } = LangHelper.GetString("Figure.Bishop");

        public Start Start { get; set; }

        public bool OnStart { get; set; } = true;

        public Bishop()
        {
            InitializeComponent();
        }

        public IEnumerable<Point> GetMovement(IEnumerable<(Point Position, bool isFriend)> OtherFigures)
        {
            Point Pos = Position;

            // Top-left
            Pos.X--;
            Pos.Y--;
            while (Pos.X >= 0 && Pos.X <= 7 && Pos.Y >= 0 && Pos.Y <= 7)
            {
                if (OtherFigures.Any(Figure => Figure.Position.X == Pos.X && Figure.Position.Y == Pos.Y && !Figure.isFriend))   // Hit an enemy
                {
                    yield return new Point(Pos.X, Pos.Y);
                    break;
                }
                else if (!OtherFigures.Any(Figure => Figure.Position.X == Pos.X && Figure.Position.Y == Pos.Y))     // Just move
                    yield return new Point(Pos.X, Pos.Y);
                else     // Dont hit teammates
                    break;

                Pos.X--;
                Pos.Y--;
            }
            Pos = Position;

            // Top-right
            Pos.X++;
            Pos.Y--;
            while (Pos.X >= 0 && Pos.X <= 7 && Pos.Y >= 0 && Pos.Y <= 7)
            {
                if (OtherFigures.Any(Figure => Figure.Position.X == Pos.X && Figure.Position.Y == Pos.Y && !Figure.isFriend))   // Hit an enemy
                {
                    yield return new Point(Pos.X, Pos.Y);
                    break;
                }
                else if (!OtherFigures.Any(Figure => Figure.Position.X == Pos.X && Figure.Position.Y == Pos.Y))     // Just move
                    yield return new Point(Pos.X, Pos.Y);
                else     // Dont hit teammates
                    break;

                Pos.X++;
                Pos.Y--;
            }
            Pos = Position;

            // Down-right
            Pos.X++;
            Pos.Y++;
            while (Pos.X >= 0 && Pos.X <= 7 && Pos.Y >= 0 && Pos.Y <= 7)
            {
                if (OtherFigures.Any(Figure => Figure.Position.X == Pos.X && Figure.Position.Y == Pos.Y && !Figure.isFriend))   // Hit an enemy
                {
                    yield return new Point(Pos.X, Pos.Y);
                    break;
                }
                else if (!OtherFigures.Any(Figure => Figure.Position.X == Pos.X && Figure.Position.Y == Pos.Y))     // Just move
                    yield return new Point(Pos.X, Pos.Y);
                else     // Dont hit teammates
                    break;

                Pos.X++;
                Pos.Y++;
            }
            Pos = Position;

            // Down-left
            Pos.X--;
            Pos.Y++;
            while (Pos.X >= 0 && Pos.X <= 7 && Pos.Y >= 0 && Pos.Y <= 7)
            {
                if (OtherFigures.Any(Figure => Figure.Position.X == Pos.X && Figure.Position.Y == Pos.Y && !Figure.isFriend))   // Hit an enemy
                {
                    yield return new Point(Pos.X, Pos.Y);
                    break;
                }
                else if (!OtherFigures.Any(Figure => Figure.Position.X == Pos.X && Figure.Position.Y == Pos.Y))     // Just move
                    yield return new Point(Pos.X, Pos.Y);
                else     // Dont hit teammates
                    break;

                Pos.X--;
                Pos.Y++;
            }
        }
    }
}
