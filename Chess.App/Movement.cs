using Chess.Figures;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Chess.App
{
    public partial class ChessGame
    {
        /// <summary>
        /// Set all figures new at the table
        /// </summary>
        /// <param name="ColorUp"></param>
        private void SetFigures(Color ColorUp)
        {
            // Clear old
            List<UIElement> Elements = new List<UIElement>();
            foreach (UIElement Element in Grid.Children)
            {
                if (Element as IFigure != null)
                    Elements.Add(Element);
            }

            #region Set figures up
            Canvas.Children.Add(new Tower()
            {
                Name = $"Tower_{ColorUp}_0",
                Color = ColorUp,
                Position = new Point(0, 0),
                Start = Start.Up
            });

            Canvas.Children.Add(new Jumper()
            {
                Name = $"Jumper_{ColorUp}_0",
                Color = ColorUp,
                Position = new Point(1, 0),
                Start = Start.Up
            });

            Canvas.Children.Add(new Bishop()
            {
                Name = $"Bishop_{ColorUp}_0",
                Color = ColorUp,
                Position = new Point(2, 0),
                Start = Start.Up
            });

            // King and queen at the right start pos
            if (ColorUp == Color.Black)
            {
                Canvas.Children.Add(new King()
                {
                    Name = $"King_{ColorUp}",
                    Color = ColorUp,
                    Position = new Point(4, 0),
                    Start = Start.Up
                });

                Canvas.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorUp}",
                    Color = ColorUp,
                    Position = new Point(3, 0),
                    Start = Start.Up
                });
            }
            else
            {
                Canvas.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorUp}",
                    Color = ColorUp,
                    Position = new Point(4, 0),
                    Start = Start.Up
                });

                Canvas.Children.Add(new King()
                {
                    Name = $"King_{ColorUp}",
                    Color = ColorUp,
                    Position = new Point(3, 0),
                    Start = Start.Up
                });
            }

            Canvas.Children.Add(new Bishop()
            {
                Name = $"Bishop_{ColorUp}_1",
                Color = ColorUp,
                Position = new Point(5, 0),
                Start = Start.Up
            });

            Canvas.Children.Add(new Jumper()
            {
                Name = $"Jumper_{ColorUp}_1",
                Color = ColorUp,
                Position = new Point(6, 0),
                Start = Start.Up
            });

            Canvas.Children.Add(new Tower()
            {
                Name = $"Tower_{ColorUp}_1",
                Color = ColorUp,
                Position = new Point(7, 0),
                Start = Start.Up
            });

            // All the farmers
            for (int i = 0; i < 8; i++)
                Canvas.Children.Add(new Farmer()
                {
                    Name = $"Farmer_{ColorUp}_{i}",
                    Color = ColorUp,
                    Position = new Point(i, 1),
                    Start = Start.Up
                });
            #endregion

            // Reverse color
            Color ColorDown = ColorUp == Color.Black ? Color.White : Color.Black;

            #region Set figures down
            Canvas.Children.Add(new Tower()
            {
                Name = $"Tower_{ColorDown}_0",
                Color = ColorDown,
                Position = new Point(0, 7),
                Start = Start.Down
            });

            Canvas.Children.Add(new Jumper()
            {
                Name = $"Jumper_{ColorDown}_0",
                Color = ColorDown,
                Position = new Point(1, 7),
                Start = Start.Down
            });

            Canvas.Children.Add(new Bishop()
            {
                Name = $"Bishop_{ColorDown}_0",
                Color = ColorDown,
                Position = new Point(2, 7),
                Start = Start.Down
            });

            // King and queen at the right start pos
            if (ColorDown == Color.White)
            {
                Canvas.Children.Add(new King()
                {
                    Name = $"King_{ColorDown}",
                    Color = ColorDown,
                    Position = new Point(4, 7),
                    Start = Start.Down
                });

                Canvas.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorDown}",
                    Color = ColorDown,
                    Position = new Point(3, 7),
                    Start = Start.Down
                });
            }
            else
            {
                Canvas.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorDown}",
                    Color = ColorDown,
                    Position = new Point(4, 7),
                    Start = Start.Down
                });

                Canvas.Children.Add(new King()
                {
                    Name = $"King_{ColorDown}",
                    Color = ColorDown,
                    Position = new Point(3, 7),
                    Start = Start.Down
                });
            }

            Canvas.Children.Add(new Bishop()
            {
                Name = $"Bishop_{ColorDown}_1",
                Color = ColorDown,
                Position = new Point(5, 7),
                Start = Start.Down
            });

            Canvas.Children.Add(new Jumper()
            {
                Name = $"Jumper_{ColorDown}_1",
                Color = ColorDown,
                Position = new Point(6, 7),
                Start = Start.Down
            });

            Canvas.Children.Add(new Tower()
            {
                Name = $"Tower_{ColorDown}_1",
                Color = ColorDown,
                Position = new Point(7, 7),
                Start = Start.Down
            });

            // All the farmers
            for (int i = 0; i < 8; i++)
                Canvas.Children.Add(new Farmer()
                {
                    Name = $"Farmer_{ColorDown}_{i}",
                    Color = ColorDown,
                    Position = new Point(i, 6),
                    Start = Start.Down
                });
            #endregion

            // Add drag & drop event and position for all
            foreach (UIElement element in Canvas.Children)
            {
                if (element as IFigure != null)
                {
                    // Add handlers
                    element.MouseLeftButtonDown += Figure_LeftMouseButtonDown;
                    element.MouseMove += Figure_MouseMove;
                    element.MouseLeftButtonUp += Figure_LeftMouseButtonUp;

                    // Position
                    System.Windows.Controls.Canvas.SetLeft(element, ((IFigure)element).Position.X * 100);
                    System.Windows.Controls.Canvas.SetTop(element, ((IFigure)element).Position.Y * 100);
                }
            }
        }

        #region Rochade moves
        private enum RochadeMove
        {
            RochadeShort,
            RochadeLong
        }

        private IEnumerable<(Point KingTarget, string TowerName, Point TowerTarget)> PossibleRochade { get; set; } = new List<(Point KingTarget, string TowerName, Point TowerTarget)>();

        /// <summary>
        /// Get all possible rochades for moving king
        /// </summary>
        /// <returns></returns>
        private IEnumerable<(Point KingTarget, string TowerName, Point TowerTarget)> GetPossibleRochade()
        {
            // Get all figures
            IEnumerable<IFigure> Figures = Canvas.Children.OfType<IFigure>().ToList();

            switch (DragFigure.Color)
            {
                case Color.Black:
                    if (!Figures.First(Figure => Figure.GetType() == typeof(King) && Figure.Color == Color.Black).OnStart)     // King mustnt move
                        yield break;

                    if (Figures.FirstOrDefault(Element => Element.Color == Color.Black).Start == Start.Up)     // Black figures on top
                    {
                        // Short rochade
                        if (Figures.Any(Figure => Figure.Position == new Point(7, 0) && Figure.OnStart) &&     // Tower must exist
                            Figures.All(Figure => Figure.Position != new Point(5, 0) && Figure.Position != new Point(6, 0)))     // Fields between them muste be free
                            yield return (new Point(6, 0), ((FrameworkElement)Figures.First(Figure => Figure.Position == new Point(7, 0))).Name, new Point(5, 0));

                        // Long rochade
                        if (Figures.Any(Figure => Figure.Position == new Point(0, 0) && Figure.OnStart) &&     // Tower must exist
                            Figures.All(Figure => Figure.Position != new Point(1, 0) && Figure.Position != new Point(2, 0) && Figure.Position != new Point(3, 0)))     // Fields between them muste be free
                            yield return (new Point(2, 0), ((FrameworkElement)Figures.First(Figure => Figure.Position == new Point(0, 0))).Name, new Point(3, 0));
                    }
                    else
                    {
                        // Short rochade
                        if (Figures.Any(Figure => Figure.Position == new Point(0, 7) && Figure.OnStart) &&     // Tower must exist
                            Figures.All(Figure => Figure.Position != new Point(1, 7) && Figure.Position != new Point(2, 7)))     // Fields between them muste be free
                            yield return (new Point(1, 7), ((FrameworkElement)Figures.First(Figure => Figure.Position == new Point(0, 7))).Name, new Point(2, 7));

                        // Long rochade
                        if (Figures.Any(Figure => Figure.Position == new Point(7, 7) && Figure.OnStart) &&     // Tower must exist
                            Figures.All(Figure => Figure.Position != new Point(4, 7) && Figure.Position != new Point(4, 7) && Figure.Position != new Point(4, 7)))     // Fields between them muste be free
                            yield return (new Point(5, 7), ((FrameworkElement)Figures.First(Figure => Figure.Position == new Point(7, 7))).Name, new Point(4, 7));
                    }
                    break;
                case Color.White:
                    if (!Figures.First(Figure => Figure.GetType() == typeof(King) && Figure.Color == Color.White).OnStart)     // King mustnt move
                        yield break;

                    if (Figures.FirstOrDefault(Element => Element.Color == Color.White).Start == Start.Up)     // White figures on top
                    {
                        // Short rochade
                        if (Figures.Any(Figure => Figure.Position == new Point(0, 0) && Figure.OnStart) &&     // Tower must exist
                            Figures.All(Figure => Figure.Position != new Point(1, 0) && Figure.Position != new Point(2, 0)))     // Fields between them muste be free
                            yield return (new Point(1, 0), ((FrameworkElement)Figures.First(Figure => Figure.Position == new Point(0, 0))).Name, new Point(2, 0));

                        // Long rochade
                        if (Figures.Any(Figure => Figure.Position == new Point(7, 0) && Figure.OnStart) &&     // Tower must exist
                            Figures.All(Figure => Figure.Position != new Point(4, 0) && Figure.Position != new Point(5, 0) && Figure.Position != new Point(6, 0)))     // Fields between them muste be free
                            yield return (new Point(5, 0), ((FrameworkElement)Figures.First(Figure => Figure.Position == new Point(7, 0))).Name, new Point(4, 0));
                    }
                    else
                    {
                        // Short rochade
                        if (Figures.Any(Figure => Figure.Position == new Point(7, 7) && Figure.OnStart) &&     // Tower must exist
                            Figures.All(Figure => Figure.Position != new Point(5, 7) && Figure.Position != new Point(6, 7)))     // Fields between them muste be free
                            yield return (new Point(6, 7), ((FrameworkElement)Figures.First(Figure => Figure.Position == new Point(7, 7))).Name, new Point(5, 7));

                        // Long rochade
                        if (Figures.Any(Figure => Figure.Position == new Point(0, 7) && Figure.OnStart) &&     // Tower must exist
                            Figures.All(Figure => Figure.Position != new Point(1, 7) && Figure.Position != new Point(2, 7) && Figure.Position != new Point(3, 7)))     // Fields between them muste be free
                            yield return (new Point(2, 7), ((FrameworkElement)Figures.First(Figure => Figure.Position == new Point(7, 7))).Name, new Point(3, 7));
                    }
                    break;
            }
        }
        #endregion
    }
}
