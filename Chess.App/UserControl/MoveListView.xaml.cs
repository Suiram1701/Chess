using System.Windows;
using System.Windows.Media;
using Control = System.Windows.Controls;

namespace Chess.App.UserControl
{
    /// <summary>
    /// Interaktionslogik für MoveListView.xaml
    /// </summary>
    public partial class MoveListView : Control.UserControl
    {
        public static DependencyProperty TeamProperty = DependencyProperty.Register("Team", typeof(Brush), typeof(MoveListView), new PropertyMetadata(Brushes.SaddleBrown));
        public Brush Team
        {
            get => (Brush)GetValue(TeamProperty);
            set => SetValue(TeamProperty, value);
        }

        public static DependencyProperty FigureNameProperty = DependencyProperty.Register("FigureName", typeof(string), typeof(MoveListView), new PropertyMetadata(string.Empty));
        public string FigureName
        {
            get => (string)GetValue(FigureNameProperty);
            set => SetValue(FigureNameProperty, value);
        }

        public static DependencyProperty StartFieldProperty = DependencyProperty.Register("StartField", typeof(Point), typeof(MoveListView), new PropertyMetadata(new Point(0, 0)));
        public Point StartField
        {
            get => (Point)GetValue(StartFieldProperty);
            set => SetValue(StartFieldProperty, value);
        }

        public static DependencyProperty EndFieldProperty = DependencyProperty.Register("EndField", typeof(Point), typeof(MoveListView), new PropertyMetadata(new Point(0, 0)));
        public Point EndField
        {
            get => (Point)GetValue(EndFieldProperty);
            set => SetValue(EndFieldProperty, value);
        }

        public static DependencyProperty DetailProperty = DependencyProperty.Register("Detail", typeof(string), typeof(MoveListView), new PropertyMetadata(string.Empty));
        public string Detail
        {
            get => (string)(GetValue(DetailProperty));
            set => SetValue(DetailProperty, value);
        }

        public MoveListView()
        {
            InitializeComponent();
        }
    }
}
