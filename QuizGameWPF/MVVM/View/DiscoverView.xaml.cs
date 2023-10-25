using QuizGameWPF.MVVM.Model;
using QuizGameWPF.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

namespace QuizGameWPF.MVVM.View
{
    /// <summary>
    /// Interaction logic for DiscoverView.xaml
    /// </summary>
    public partial class DiscoverView : UserControl
    {
        public DiscoverView()
        {
            InitializeComponent();
            LoadQuizTitles();
        }

        private void LoadQuizTitles()
        {
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "quiz.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<Question> questions = JsonSerializer.Deserialize<List<Question>>(json);

                var titlesWithImages = questions
                    .Select(q => new { Title = ToTitleCase(q.Title), ImagePath = q.ImagePath }) 
                    .Where(title => title.Title != "Default")
                    .Distinct()
                    .ToList();

                TitleList.ItemsSource = titlesWithImages;
            }
        }

        private string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            string[] words = input.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(words[i]))
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }

            return string.Join(" ", words);
        }


        private void TitleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TitleList.SelectedItem != null)
            {
                var selectedTitleWithImage = (dynamic)TitleList.SelectedItem; // Cast to the anonymous type
                string selectedTitle = selectedTitleWithImage.Title;
                string selectedImagePath = selectedTitleWithImage.ImagePath;

                // Use selectedTitle and selectedImagePath as needed
                QuizInit.ChoosenCategory = selectedTitle.ToLower();
                QuizInit.PlayColor = "#90BE6D";
            }
        }


        private void MusicBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            QuizInit.ChoosenCategory = "music";
            QuizInit.PlayColor = "#90BE6D";
        }

        private void PoliticsBorder_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            QuizInit.ChoosenCategory = "politics";
            QuizInit.PlayColor = "#90BE6D";
        }

        private void SportsBorder_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            QuizInit.ChoosenCategory = "sports";
            QuizInit.PlayColor = "#90BE6D";
        }
        private void AnimalsBorder_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            QuizInit.ChoosenCategory = "animals";
            QuizInit.PlayColor = "#90BE6D";
        }

        private void GeneralKnowledge_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            QuizInit.ChoosenCategory = "generalknowledge";
            QuizInit.PlayColor = "#90BE6D";
        }

        private void Movies_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            QuizInit.ChoosenCategory = "movies";
            QuizInit.PlayColor = "#90BE6D";
        }
        private void History_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            QuizInit.ChoosenCategory = "history";
            QuizInit.PlayColor = "#90BE6D";
        }

        private void Geography_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            QuizInit.ChoosenCategory = "geography";
            QuizInit.PlayColor = "#90BE6D";
        }
    }
}
