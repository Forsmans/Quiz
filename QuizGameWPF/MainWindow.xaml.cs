using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using QuizGameWPF.MVVM.Model;

namespace QuizGameWPF
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            QuizInit.BarProgressChanged += OnBarProgressChanged;

            QuizInit.PlayColorChanged += OnPlayColorChanged;

            QuizInit.PlayColor = "#D2D5D0";

            if (ProgressBar.Value == 100)
            {
                MessageBox.Show("You completed all Quizes! Try creating your own and challenge your friends or replay the old ones!");
            }

            
        }

        private void OnBarProgressChanged(object sender, int newProgress)
        {
            ProgressBar.Value = newProgress;
            ProgressBarText.Text = newProgress.ToString() + "%";
        }

        private void OnPlayColorChanged(object sender, string newColor)
        {
            Play.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(newColor));
        }





    }

}
