using Chess.Figures;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Chess.App
{
    /// <summary>
    /// Select a figure for farmer transform
    /// </summary>
    public partial class SelectFigureDialog : Window
    {
        public Type SelectedType { get; private set; }

        public SelectFigureDialog() =>
            InitializeComponent();

        private void Figure_Click(object sender, RoutedEventArgs e)
        {
            // Evaluate buttons
            switch (((Button)sender).Tag)
            {
                case "Queen":
                    SelectedType = typeof(Queen);
                    break;
                case "Jumper":
                    SelectedType = typeof(Jumper);
                    break;
                case "Bishop":
                    SelectedType = typeof(Bishop);
                    break;
                case "Tower":
                    SelectedType = typeof(Tower);
                    break;
            }

            DialogResult = true;
        }
    }
}
