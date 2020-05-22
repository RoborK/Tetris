using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tetris
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RecordsWindow.GetRecord();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow();
            gameWindow.Owner = this;
            gameWindow.ShowDialog();
        }

        private void CustomButton_Click(object sender, RoutedEventArgs e)
        {
            CustomWindow customWindow = new CustomWindow();
            customWindow.Owner = this;
            customWindow.ShowDialog();
        }

        private void RecordsButton_Click(object sender, RoutedEventArgs e)
        {
            RecordsWindow recordsWindow = new RecordsWindow();
            recordsWindow.Owner = this;
            recordsWindow.ShowDialog();
        }
    }


}
