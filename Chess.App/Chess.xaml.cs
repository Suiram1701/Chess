using Chess.App.Files;
using Chess.App.Models;
using Chess.App.UserControl;
using Chess.Figures;
using Localization;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Color = Chess.Figures.Color;

namespace Chess.App
{
    /// <summary>
    /// Interaktionslogik für Chess.xaml
    /// </summary>
    public partial class ChessGame : Window
    {
        #region Localization
        public string L_Title => LangHelper.GetString("MainMenu.Title");
        public string L_PossibleMovBtn => LangHelper.GetString("Chess.PossibleMovBtn");
        public string L_EnemyMovBtn => LangHelper.GetString("Chess.EnemyMovBtn");
        #endregion

        private Color CurrentPlayer { get; set; } = Color.White;

        public ChessGame(ChessFile gameSave = null)
        {
            InitializeComponent();

            // If set load gamesave
            if (gameSave != null)
            {
                CurrentPlayer = gameSave.CurrentColor;
                
                // Load history
                foreach (MoveListView move in gameSave.MoveHistory.Select(model => (MoveListView)model))
                    MoveListViews.Items.Add(move);
                Scroll.ScrollToEnd();

                // Load figures
                foreach (UIElement element in gameSave.Figures.Select(figure => (UIElement)figure))
                    Canvas.Children.Add(element);

                // Add drag & drop event and position for all
                foreach (UIElement element in Canvas.Children)
                {
                    if ((element as IFigure) != null)
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
            else
                SetFigures(Color.Black);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Title = "Save the game",
                Filter = "Chess file (*.chess)|*.chess",
                FileName = DateTime.Now.ToShortDateString() + ".chess",
            };
            if (dialog.ShowDialog() == true )
            {
                // Get paths
                string path = dialog.FileName.Remove(dialog.FileName.LastIndexOf('\\'));
                string file = dialog.FileName.Substring(dialog.FileName.LastIndexOf('\\') + 1);
                string fileName = file.Remove(file.LastIndexOf('.'));

                // Create file
                ChessFile.Create(fileName, filePath: path,
                    (nameof(ChessFile.CurrentColor), CurrentPlayer),
                    (nameof(ChessFile.MoveHistory), MoveListViews.Items.Cast<MoveListView>().Select(view => (MoveModel)view).ToList()),
                    (nameof(ChessFile.Figures), Canvas.Children.OfType<IFigure>().Select(figure => (FigureModel)(UIElement)figure).ToList()));
            }
        }

        #region Drag & drop
        private IFigure DragFigure { get; set; } = null;
        private IEnumerable<Point> AllowedFields { get; set; } = null;
        private (bool Possible, string FigureName) EnPassend = (false, null);
        private IEnumerable<Point> _EnemyFields { get; set; } = null;

        /// <summary>
        /// Select the figure as current item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Figure_LeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((sender as IFigure) != null)
            {
#if !DEBUG
                if (((IFigure)sender).Color != CurrentPlayer)     // Player must selected
                    return;
#endif
                // Prepare for move
                DragFigure = (IFigure)sender;
                Panel.SetZIndex((UIElement)DragFigure, 5);
                ((UIElement)DragFigure).CaptureMouse();

                CalcMoves();
                _EnemyFields = GetEnemyFields(DragFigure.Color == Color.White ? Color.Black : Color.White);
                MarkFields();
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
                // if on a player kill him
                UIElement PlayerToKill = Canvas.Children.Cast<UIElement>()
                    .FirstOrDefault(Element => (Element as IFigure) != null && (Element as IFigure).Position == DropPosition);
                if (PlayerToKill != null)
                    Canvas.Children.Remove(PlayerToKill);

                // If you kill the enemy king you win
                if (PlayerToKill?.GetType() == typeof(King))
                    PlayerWin(CurrentPlayer);

                #region Special moves
                string MoveDetail = string.Empty;
                // Check for rochade
                if (PossibleRochade.Any(Rochade => Rochade.KingTarget == DropPosition) && DragFigure.GetType() == typeof(King))
                {
                    (Point KingTarget, string TowerName, Point TowerTarget) = PossibleRochade.First(Rochade => Rochade.KingTarget == DropPosition);
                    IFigure Tower = (IFigure)Canvas.Children.OfType<FrameworkElement>().FirstOrDefault(Element => Element.Name == TowerName);

                    // Add to move history
                    MoveDetail = "Rochade";
                    MoveListViews.Items.Add(new MoveListView()
                    {
                        Team = Tower.Color == Color.Black ? Brushes.SaddleBrown : Brushes.BurlyWood,
                        FigureName = Tower.Name,
                        StartField = Tower.Position,
                        EndField = TowerTarget,
                        FieldName = ((FrameworkElement)Tower).Name,
                        FigureType = typeof(Tower),
                        Detail = MoveDetail
                    });

                    // Move tower and update position
                    Tower.Position = TowerTarget;
                    Canvas.SetLeft((UIElement)Tower, Tower.Position.X * 100);
                    Canvas.SetTop((UIElement)Tower, Tower.Position.Y * 100);
                }
                else if (EnPassend.Possible && DragFigure.GetType() == typeof(Farmer))     // Do en passend if possible
                {
                    MoveDetail = "En Passend";

                    // Kill figure
                    IFigure farmerToKill = Canvas.Children.OfType<IFigure>().FirstOrDefault(Figure => ((FrameworkElement)Figure).Name == EnPassend.FigureName);
                    if (farmerToKill != null)
                        Canvas.Children.Remove((UIElement)farmerToKill);
                }
                else if (DragFigure.GetType() == typeof(Farmer))     // Check for farmer transform
                {
                    // Check position
                    if (DropPosition.Y == (DragFigure.Color == Color.White ? 0 : 7))
                    {
                    Show:
                        SelectFigureDialog sfd = new SelectFigureDialog();
                        if (sfd.ShowDialog() == true)
                        {
                            // Set new figure and set farmers position
                            IFigure figure = null;
                            switch (sfd.SelectedType.Name)
                            {
                                case "Queen":
                                    figure = new Queen();
                                    break;
                                case "Jumper":
                                    figure = new Jumper();
                                    break;
                                case "Bishop":
                                    figure = new Bishop();
                                    break;
                                case "Tower":
                                    figure = new Tower();
                                    break;
                                default:
                                    goto Show;
                            }
                            figure.OnStart = false;
                            figure.Start = DragFigure.Start;
                            figure.Color = DragFigure.Color;
                            figure.Position = DropPosition;

                            // Replace figure
                            int index = Canvas.Children.IndexOf((UIElement)DragFigure);
                            Canvas.Children.RemoveAt(index);
                            Canvas.Children.Add((UIElement)figure);
                            ((UIElement)figure).MouseLeftButtonDown += Figure_LeftMouseButtonDown;
                            ((UIElement)figure).MouseMove += Figure_MouseMove;
                            ((UIElement)figure).MouseLeftButtonUp += Figure_LeftMouseButtonUp;

                            // Apply position
                            Canvas.SetLeft((UIElement)figure, figure.Position.X * 100);
                            Canvas.SetTop((UIElement)figure, figure.Position.Y * 100);
                        }
                        else
                            goto Show;
                        MoveDetail = "Bauernumwandlung";
                    }
                }
                #endregion

                // Add to move list
                MoveListViews.Items.Add(new MoveListView()
                {
                    Team = DragFigure.Color == Color.Black ? Brushes.SaddleBrown : Brushes.BurlyWood,
                    FigureName = DragFigure.Name,
                    StartField = DragFigure.Position,
                    EndField = DropPosition,
                    FieldName = ((FrameworkElement)DragFigure).Name,
                    FigureType = DragFigure.GetType(),
                    Detail = MoveDetail
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
            Panel.SetZIndex((UIElement)DragFigure, 2);

            // Clear
            RemoveMarks();
            DragFigure = null;
            AllowedFields = null;
            e.Handled = true;
        }

        /// <summary>
        /// Set allowed fields
        /// </summary>
        private void CalcMoves()
        {
            AllowedFields = DragFigure.GetMovement(Canvas.Children.Cast<UIElement>()
                    .Where(Element => (Element as IFigure) != null)     // Only the figures
                    .Select(Element => (((IFigure)Element).Position, ((IFigure)Element).Color == DragFigure.Color))).ToList();     // Get relavent data

            // Get possiblerochade moves
            if (DragFigure.GetType() == typeof(King))
            {
                PossibleRochade = GetPossibleRochade();
                foreach ((Point KingTarget, string TowerName, Point TowerTarget) in PossibleRochade)
                    AllowedFields = AllowedFields.Append(KingTarget);
            }

            // Check for en passend moves
            if (DragFigure.GetType() == typeof(Farmer))
            {
                MoveListView lastMove = MoveListViews.Items.Cast<MoveListView>().LastOrDefault();
                if (lastMove?.FigureType == typeof(Farmer))
                {
                    // Check position
                    if (DragFigure.Position.Y == lastMove.EndField.Y && (DragFigure.Position.X == lastMove.EndField.X - 1 || DragFigure.Position.X == lastMove.EndField.X + 1))
                    {
                        // If possible prepare en passend
                        AllowedFields = AllowedFields.Append(new Point(lastMove.EndField.X, lastMove.EndField.Y + (DragFigure.Color == Color.White ? -1 : +1)));
                        EnPassend.FigureName = lastMove.FieldName;
                        EnPassend.Possible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Mark the fields
        /// </summary>
        private void MarkFields()
        {
            // All same elements
            IEnumerable<Point> sameFields = (PossibleFields.IsChecked ?? false) && (EnemyFields.IsChecked ?? false) ? AllowedFields.Where(point => _EnemyFields.Contains(point)) : null;

            // Mark allowed fields if enabled
            if (PossibleFields.IsChecked == true)
            {
                foreach (Point Point in AllowedFields)
                {
                    // Skip same fields
                    if (sameFields?.Any(x => x.Equals(Point)) ?? false)
                        continue;

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

            // Mark enemy fields is enabled
            if (EnemyFields.IsChecked == true)
            {
                foreach (Point Point in _EnemyFields)
                {
                    // Skip same fields
                    if (sameFields?.Any(x => x.Equals(Point)) ?? false)
                        continue;

                    // Create and set on canvas
                    int Index = Canvas.Children.Add(new Rectangle()
                    {
                        Tag = "Mark",
                        Height = 100,
                        Width = 100,
                        Fill = Brushes.Red,
                        Opacity = 0.5
                    });
                    Canvas.SetLeft(Canvas.Children[Index], Point.X * 100);
                    Canvas.SetTop(Canvas.Children[Index], Point.Y * 100);
                    Panel.SetZIndex(Canvas.Children[Index], 1);
                }
            }

            // Mark same fields
            foreach (Point field in sameFields ?? new List<Point>())
            {
                // Create and set on canvas
                int Index = Canvas.Children.Add(new Rectangle()
                {
                    Tag = "Mark",
                    Height = 100,
                    Width = 100,
                    Fill = Brushes.Yellow,
                    Opacity = 0.5
                });
                Canvas.SetLeft(Canvas.Children[Index], field.X * 100);
                Canvas.SetTop(Canvas.Children[Index], field.Y * 100);
                Panel.SetZIndex(Canvas.Children[Index], 1);
            }
        }

        /// <summary>
        /// Remove all field marks
        /// </summary>
        private void RemoveMarks()
        {
            foreach (UIElement Element in Canvas.Children.Cast<FrameworkElement>().Where(Element => (string)Element.Tag == "Mark").ToArray())
                Canvas.Children.Remove(Element);
        }
        #endregion

        /// <summary>
        /// View that a player win
        /// </summary>
        /// <param name="color">The player</param>
        private void PlayerWin(Color color)
        {
            MessageBox.Show($"Der Spieler {(color == Color.Black ? "Schwarz" : "Weiß")} hat gewonnen!", "Ein Spieler hat gewonnen!", MessageBoxButton.OK);

            // Back to main menu
            new MainMenu().Show();
            Close();
        }
    }
}
