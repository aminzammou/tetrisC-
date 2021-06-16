using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Mainwindow : Window
    {
        public Mainwindow()
        {
            InitializeComponent();

        }

        public void Quit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void SinglePlayer(object sender, RoutedEventArgs e)
        {
            SingleplayerWindow window = new SingleplayerWindow();
            window.Show();
            this.Close();
        }

        public void MultiPlayer(object sender, RoutedEventArgs e)
        {
            MultiplayerWindow window = new MultiplayerWindow();
            window.Show();
            this.Close();
        }
    }

}
