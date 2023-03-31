using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Chess.Figures
{
    /// <summary>
    /// Interaktionslogik für Queen.xaml
    /// </summary>
    public partial class Queen : UserControl, IFigure
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(Queen), new PropertyMetadata(Color.Black));
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);

            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point), typeof(Queen), new PropertyMetadata(new Point(0, 0)));
        public Point Position
        {
            get => (Point)GetValue(PositionProperty);

            set => SetValue(PositionProperty, value);
        }

        string IFigure.Name { get; } = "Dame";

        public Start Start { get; set; }

        public bool OnStart { get; set; } = true;

        public Queen()
        {
            InitializeComponent();
        }

        public IEnumerable<Point> GetMovement(IEnumerable<(Point Position, bool isFriend)> OtherFigures)
        {
            Point Pos = Position;

            #region diagonally movement
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
            #endregion

            Pos = Position;

            #region straight movement
            // Top
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

                Pos.Y--;
            }
            Pos = Position;

            // Down
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

                Pos.Y++;
            }
            Pos = Position;

            // Left
            Pos.X--;
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
            }
            Pos = Position;

            // Right
            Pos.X++;
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
            }
        }
        #endregion
    }
}
