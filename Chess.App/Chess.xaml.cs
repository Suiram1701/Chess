using System.Windows;
using Chess.Figures;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using System.Linq;
using System.Windows.Shapes;
using System.Windows.Media;
using Color = Chess.Figures.Color;
using System.Collections.ObjectModel;
using Chess.App.UserControl;

namespace Chess.App
{
    /// <summary>
    /// Interaktionslogik für Chess.xaml
    /// </summary>
    public partial class ChessGame : Window
    {
        private Color? CurrentPlayer { get; set; } = null;

        public ObservableCollection<(string FigureName, Point Start, Point End)> MoveList { get; set; } = new ObservableCollection<(string FigureName, Point Start, Point End)>();

        public ChessGame()
        {
            // TODO: Rechts (als erstes) geschlagene figuren
            // TODO: Verlauf rechts

            InitializeComponent();
            SetFigures(Color.Black);
        }

        #region Drag & drop
        private IFigure DragFigure { get; set; } = null;
        private IEnumerable<Point> AllowedFields { get; set; } = null;

        /// <summary>
        /// Select the figure as current item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Figure_LeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender as IFigure != null)
            {
                if (((IFigure)sender).Color != CurrentPlayer && CurrentPlayer != null)     // Player must selected
                    return;

                // Prepare for move
                DragFigure = (IFigure)sender;
                ((UIElement)DragFigure).CaptureMouse();

                MarkAllowedFields();
                GC.Collect();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Move the current figure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Figure_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragFigure != null)
            {
                // Calculate current position with mouse and canvas
                Point currentPosition = e.GetPosition(Canvas);

                // Update figure position
                Canvas.SetLeft((UIElement)DragFigure, currentPosition.X - 50);
                Canvas.SetTop((UIElement)DragFigure, currentPosition.Y - 50);

                // Mark event
                e.Handled = true;
            }
        }

        /// <summary>
        /// Release figure and set on fiel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Figure_LeftMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DragFigure == null)
                return;

            (DragFigure as UIElement).ReleaseMouseCapture();

            Point DropPosition = new Point(Math.Round(Canvas.GetLeft((UIElement)DragFigure) / 100), Math.Round(Canvas.GetTop((UIElement)DragFigure) / 100));

            // Check if the figure is on the game table and on one of the allowed fields
            if (DropPosition.X >= 0 && DropPosition.Y >= 0 && DropPosition.X <= 7 && DropPosition.Y <= 7 && AllowedFields.Any(Field => Field == DropPosition))
            {
                if (CurrentPlayer == null)     // First moved team begin
                    CurrentPlayer = ((IFigure)sender).Color;

                // Add to move list
                MoveListViews.Items.Add(new MoveListView()
                {
                    Team = DragFigure.Color == Color.Black ? Brushes.SaddleBrown : Brushes.BurlyWood,
                    FigureName = DragFigure.Name,
                    StartField = DragFigure.Position,
                    EndField = DropPosition,
                });
                Scroll.ScrollToEnd();

                // select other player
                CurrentPlayer = CurrentPlayer == Color.Black ? Color.White : Color.Black;

                DragFigure.OnStart = DropPosition == DragFigure.Position && DragFigure.OnStart;
                DragFigure.Position = DropPosition;
            }

            // Set figure on field
            Canvas.SetLeft((UIElement)DragFigure, DragFigure.Position.X * 100);
            Canvas.SetTop((UIElement)DragFigure, DragFigure.Position.Y * 100);

            // Clear
            RemoveMarks();
            DragFigure = null;
            AllowedFields = null;
            e.Handled = true;
        }

        /// <summary>
        /// Set allowed fields and mark them
        /// </summary>
        private void MarkAllowedFields()
        {
            AllowedFields = DragFigure.GetMovement(Canvas.Children.Cast<UIElement>()
                    .Where(Element => Element as IFigure != null)     // Only the figures
                    .Select(Element => (((IFigure)Element).Position, ((IFigure)Element).Color == DragFigure.Color)));     // Get relavent data

            // Mark the fields
            foreach (Point Point in AllowedFields)
            {
                // Create and set on canvas
                int Index = Canvas.Children.Add(new Rectangle()
                {
                    Tag = "Mark",
                    Height = 100,
                    Width = 100,
                    Fill = Brushes.Green,
                    Opacity = 0.5
                });
                Canvas.SetLeft(Canvas.Children[Index], Point.X * 100);
                Canvas.SetTop(Canvas.Children[Index], Point.Y * 100);
                Panel.SetZIndex(Canvas.Children[Index], 1);
            }
        }

        /// <summary>
        /// Remove all green field marks
        /// </summary>
        private void RemoveMarks()
        {
            foreach (UIElement Element in Canvas.Children.Cast<FrameworkElement>().Where(Element => (string)Element.Tag == "Mark").ToArray())
                Canvas.Children.Remove(Element);
        }
        #endregion

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
                    Position = new Point(3, 0),
                    Start = Start.Up
                });

                Canvas.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorUp}",
                    Color = ColorUp,
                    Position = new Point(4, 0),
                    Start = Start.Up
                });
            }
            else
            {
                Canvas.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorUp}",
                    Color = ColorUp,
                    Position = new Point(3, 0),
                    Start = Start.Up
                });

                Canvas.Children.Add(new King()
                {
                    Name = $"King_{ColorUp}",
                    Color = ColorUp,
                    Position = new Point(4, 0),
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
                    Position = new Point(3, 7),
                    Start = Start.Down
                });

                Canvas.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorDown}",
                    Color = ColorDown,
                    Position = new Point(4, 7),
                    Start = Start.Down
                });
            }
            else
            {
                Canvas.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorDown}",
                    Color = ColorDown,
                    Position = new Point(3, 7),
                    Start = Start.Down
                });

                Canvas.Children.Add(new King()
                {
                    Name = $"King_{ColorDown}",
                    Color = ColorDown,
                    Position = new Point(4, 7),
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
                    Canvas.SetLeft(element, ((IFigure)element).Position.X * 100);
                    Canvas.SetTop(element, ((IFigure)element).Position.Y * 100);
                }
            }
        }
    }
}
