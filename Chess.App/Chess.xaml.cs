using System.Windows;
using System.Linq;
using System.Collections;
using Chess.Figures;
using System.Windows.Controls;
using System.Collections.Generic;

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
            Grid.Children.Add(new Tower()
            {
                Name = $"Tower_{ColorUp}_0",
                Color = ColorUp,
                Position = new Position(0, 0)
            });

            Grid.Children.Add(new Jumper()
            {
                Name = $"Jumper_{ColorUp}_0",
                Color = ColorUp,
                Position = new Position(1, 0)
            });

            Grid.Children.Add(new Bishop()
            {
                Name = $"Bishop_{ColorUp}_0",
                Color = ColorUp,
                Position = new Position(2, 0)
            });

            // King and queen at the right start pos
            if (ColorUp == Color.Black)
            {
                Grid.Children.Add(new King()
                {
                    Name = $"King_{ColorUp}",
                    Color = ColorUp,
                    Position = new Position(3, 0)
                });

                Grid.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorUp}",
                    Color = ColorUp,
                    Position = new Position(4, 0)
                });
            }
            else
            {
                Grid.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorUp}",
                    Color = ColorUp,
                    Position = new Position(3, 0)
                });

                Grid.Children.Add(new King()
                {
                    Name = $"King_{ColorUp}",
                    Color = ColorUp,
                    Position = new Position(4, 0)
                });
            }

            Grid.Children.Add(new Bishop()
            {
                Name = $"Bishop_{ColorUp}_1",
                Color = ColorUp,
                Position = new Position(5, 0)
            });

            Grid.Children.Add(new Jumper()
            {
                Name = $"Jumper_{ColorUp}_1",
                Color = ColorUp,
                Position = new Position(6, 0)
            });

            Grid.Children.Add(new Tower()
            {
                Name = $"Tower_{ColorUp}_1",
                Color = ColorUp,
                Position = new Position(7, 0)
            });

            // All the farmers
            for (int i = 0; i < 8; i++)
                Grid.Children.Add(new Farmer()
                {
                    Name = $"Farmer_{ColorUp}_{i}",
                    Color = ColorUp,
                    Position = new Position(i, 1)
                });
            #endregion

            // Reverse color
            Color ColorDown = ColorUp == Color.Black ? Color.White : Color.Black;

            #region Set figures up
            Grid.Children.Add(new Tower()
            {
                Name = $"Tower_{ColorDown}_0",
                Color = ColorDown,
                Position = new Position(0, 7)
            });

            Grid.Children.Add(new Jumper()
            {
                Name = $"Jumper_{ColorDown}_0",
                Color = ColorDown,
                Position = new Position(1, 7)
            });

            Grid.Children.Add(new Bishop()
            {
                Name = $"Bishop_{ColorDown}_0",
                Color = ColorDown,
                Position = new Position(2, 7)
            });

            // King and queen at the right start pos
            if (ColorDown == Color.White)
            {
                Grid.Children.Add(new King()
                {
                    Name = $"King_{ColorDown}",
                    Color = ColorDown,
                    Position = new Position(3, 7)
                });

                Grid.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorDown}",
                    Color = ColorDown,
                    Position = new Position(4, 7)
                });
            }
            else
            {
                Grid.Children.Add(new Queen()
                {
                    Name = $"Queen_{ColorDown}",
                    Color = ColorDown,
                    Position = new Position(3, 7)
                });

                Grid.Children.Add(new King()
                {
                    Name = $"King_{ColorDown}",
                    Color = ColorDown,
                    Position = new Position(4, 7)
                });
            }

            Grid.Children.Add(new Bishop()
            {
                Name = $"Bishop_{ColorDown}_1",
                Color = ColorDown,
                Position = new Position(5, 7)
            });

            Grid.Children.Add(new Jumper()
            {
                Name = $"Jumper_{ColorDown}_1",
                Color = ColorDown,
                Position = new Position(6, 7)
            });

            Grid.Children.Add(new Tower()
            {
                Name = $"Tower_{ColorDown}_1",
                Color = ColorDown,
                Position = new Position(7, 7)
            });

            // All the farmers
            for (int i = 0; i < 8; i++)
                Grid.Children.Add(new Farmer()
                {
                    Name = $"Farmer_{ColorDown}_{i}",
                    Color = ColorDown,
                    Position = new Position(i, 6)
                });
            #endregion
        }
    }
}
