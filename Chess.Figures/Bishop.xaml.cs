﻿using System.Windows;
using System.Windows.Controls;

namespace Chess.Figures
{
    /// <summary>
    /// Interaktionslogik für Bishop.xaml
    /// </summary>
    public partial class Bishop : UserControl, IFigure
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(Bishop), new PropertyMetadata(Color.Black));
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);

            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point), typeof(Bishop), new PropertyMetadata(new Point(0, 0)));
        public Point Position
        {
            get => (Point)GetValue(PositionProperty);

            set => SetValue(PositionProperty, value);
        }

        public Bishop()
        {
            InitializeComponent();
        }
    }
}
