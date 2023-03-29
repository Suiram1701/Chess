using System.Collections.Generic;
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

        public Start Start { get; set; }

        public bool OnStart { get; set; } = true;

        public Queen()
        {
            InitializeComponent();
        }

        public IEnumerable<Point> GetMovement(IEnumerable<(Point Position, bool isFriend)> OtherFigures)
        {
            throw new System.NotImplementedException();
        }
    }
}
