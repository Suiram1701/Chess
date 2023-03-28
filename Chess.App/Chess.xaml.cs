using System.Windows;
using Chess.Figures;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;
using System;

namespace Chess.App
{
    /// <summary>
    /// Interaktionslogik für Chess.xaml
    /// </summary>
    public partial class ChessGame : Window
    {
        public ChessGame()
        {
            // TODO: Rechts (als erstes) geschlagene figuren
            // TODO: Verlauf rechts

            InitializeComponent();
            SetFigures(Color.Black);
        }

        #region Drag & drop
        private IFigure DragFigure { get; set; } = null;

        /// <summary>
        /// Select the figure as current item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Figure_LeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender as IFigure != null)
            {
                // Prepare for move
                DragFigure = (IFigure)sender;
                ((UIElement)DragFigure).CaptureMouse();

                // Set Element distance
                Point currentPosition = e.GetPosition(Canvas);

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
            (DragFigure as UIElement)?.ReleaseMouseCapture();

            // Set figure on field
            DragFigure.Position = new Point(Math.Round(Canvas.GetLeft((UIElement)DragFigure) / 100), Math.Round(Canvas.GetTop((UIElement)DragFigure) / 100));
            Canvas.SetLeft((UIElement)DragFigure, DragFigure.Position.X * 100);
            Canvas.SetTop((UIElement)DragFigure, DragFigure.Position.Y * 100);

            DragFigure = null;
            e.Handled = true;
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
                Position = new Point(0, 0)
            });

            Canvas.Children.Add(new Jumper()
            {
                Name = $"Jumper_{ColorUp}_0",
                Color = ColorUp,
                Position = new Point(1, 0)
            });

            Canvas.Children.Add(new Bishop()
            {
                Name = $"Bishop_{ColorUp}_0",
                Color = ColorUp,
                Position = new Point(2, 0)
            });

            // King and queen at the right start pos
            if (ColorUp == Color.Black)
            {
                Canvas.Children.Add(new King()
                {
                    Name = $"King_{ColorUp}",
                    Color = ColorUp,
                    Position = new Point(3, 0)
                });

                Canvas.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorUp}",
                    Color = ColorUp,
                    Position = new Point(4, 0)
                });
            }
            else
            {
                Canvas.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorUp}",
                    Color = ColorUp,
                    Position = new Point(3, 0)
                });

                Canvas.Children.Add(new King()
                {
                    Name = $"King_{ColorUp}",
                    Color = ColorUp,
                    Position = new Point(4, 0)
                });
            }

            Canvas.Children.Add(new Bishop()
            {
                Name = $"Bishop_{ColorUp}_1",
                Color = ColorUp,
                Position = new Point(5, 0)
            });

            Canvas.Children.Add(new Jumper()
            {
                Name = $"Jumper_{ColorUp}_1",
                Color = ColorUp,
                Position = new Point(6, 0)
            });

            Canvas.Children.Add(new Tower()
            {
                Name = $"Tower_{ColorUp}_1",
                Color = ColorUp,
                Position = new Point(7, 0)
            });

            // All the farmers
            for (int i = 0; i < 8; i++)
                Canvas.Children.Add(new Farmer()
                {
                    Name = $"Farmer_{ColorUp}_{i}",
                    Color = ColorUp,
                    Position = new Point(i, 1)
                });
            #endregion

            // Reverse color
            Color ColorDown = ColorUp == Color.Black ? Color.White : Color.Black;

            #region Set figures down
            Canvas.Children.Add(new Tower()
            {
                Name = $"Tower_{ColorDown}_0",
                Color = ColorDown,
                Position = new Point(0, 7)
            });

            Canvas.Children.Add(new Jumper()
            {
                Name = $"Jumper_{ColorDown}_0",
                Color = ColorDown,
                Position = new Point(1, 7)
            });

            Canvas.Children.Add(new Bishop()
            {
                Name = $"Bishop_{ColorDown}_0",
                Color = ColorDown,
                Position = new Point(2, 7)
            });

            // King and queen at the right start pos
            if (ColorDown == Color.White)
            {
                Canvas.Children.Add(new King()
                {
                    Name = $"King_{ColorDown}",
                    Color = ColorDown,
                    Position = new Point(3, 7)
                });

                Canvas.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorDown}",
                    Color = ColorDown,
                    Position = new Point(4, 7)
                });
            }
            else
            {
                Canvas.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorDown}",
                    Color = ColorDown,
                    Position = new Point(3, 7)
                });

                Canvas.Children.Add(new King()
                {
                    Name = $"King_{ColorDown}",
                    Color = ColorDown,
                    Position = new Point(4, 7)
                });
            }

            Canvas.Children.Add(new Bishop()
            {
                Name = $"Bishop_{ColorDown}_1",
                Color = ColorDown,
                Position = new Point(5, 7)
            });

            Canvas.Children.Add(new Jumper()
            {
                Name = $"Jumper_{ColorDown}_1",
                Color = ColorDown,
                Position = new Point(6, 7)
            });

            Canvas.Children.Add(new Tower()
            {
                Name = $"Tower_{ColorDown}_1",
                Color = ColorDown,
                Position = new Point(7, 7)
            });

            // All the farmers
            for (int i = 0; i < 8; i++)
                Canvas.Children.Add(new Farmer()
                {
                    Name = $"Farmer_{ColorDown}_{i}",
                    Color = ColorDown,
                    Position = new Point(i, 6)
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
