using System.Windows;
using System.Windows.Controls;

namespace Chess.Figures
{
    /// <summary>
    /// Interaktionslogik für Tower.xaml
    /// </summary>
    public partial class Tower : UserControl, IFigure
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(Tower), new PropertyMetadata(Color.Black));
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);

            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Position), typeof(Tower), new PropertyMetadata(new Position(0, 0)));
        public Position Position
        {
            get => (Position)GetValue(PositionProperty);

            set => SetValue(PositionProperty, value);
        }

        public Tower()
        {
            InitializeComponent();
        }
    }
}
