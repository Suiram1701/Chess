using System.Collections.Generic;
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

        public Start Start { get; set; }

        public bool OnStart { get; set; } = true;

        public King()
        {
            InitializeComponent();
        }

        public IEnumerable<Point> GetMovement(IEnumerable<(Point Position, bool isFriend)> OtherFigures)
        {
            throw new System.NotImplementedException();
        }
    }
}
