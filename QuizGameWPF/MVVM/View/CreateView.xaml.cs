using System.IO; 
using QuizGameWPF.MVVM.Model;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class CreateView : UserControl
    {
        public CreateView()
        {
            InitializeComponent();
            string imageDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            List<ImageItem> imagePaths = GetImagesFromDirectory();

            ImageList.ItemsSource = imagePaths;

        }

        //Load image list
        private List<ImageItem> GetImagesFromDirectory()
        {
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "quiz.json");
            List<ImageItem> imageItems = new List<ImageItem>();
            HashSet<string> uniqueImagePaths = new HashSet<string>();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<Question> questions = JsonSerializer.Deserialize<List<Question>>(json);

                var imagePaths = questions
                    .Where(q => !string.IsNullOrEmpty(q.ImagePath) && q.Title != "Default")
                    .Select(q => q.ImagePath)
                    .Distinct();

                foreach (var imagePath in imagePaths)
                {
                    if (!uniqueImagePaths.Contains(imagePath))
                    {
                        imageItems.Add(new ImageItem { ImagePath = imagePath });
                        uniqueImagePaths.Add(imagePath);
                    }
                }
            }

            return imageItems;
        }

        private void ImageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImageList.SelectedItem is ImageItem selectedImageItem)
            {
                string selectedImagePath = selectedImageItem.ImagePath;
            }
        }

        //Button method
        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            int numberOfQuestions = 3;

            if (!IsQuizNameTaken())
            {
                if (AreAllTextboxesFilled(numberOfQuestions))
                {
                    await CreateQuizQuestionsJsonAsync();
                    MessageBox.Show("Your quiz has been successfully created!\n It's now added to the list in Discover.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("You have to fill in all the fields for each question.", "Incomplete Form", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        //Checks if name is already taken
        private bool IsQuizNameTaken()
        {
            string title = TitleBox.Text.ToLower();
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "quiz.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<Question> questions = JsonSerializer.Deserialize<List<Question>>(json);

                if (questions.Any(q => q.Title.ToLower() == title))
                {
                    MessageBox.Show("Quiz with the same title already exists.\n Please choose a different title.", "Title Already Taken", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return true;
                }
            }

            return false;
        }

        //Checks if a minimum of 3 questions are filled in
        private bool AreAllTextboxesFilled(int numberOfQuestions)
        {
            for (int i = 1; i <= numberOfQuestions; i++)
            {
                TextBox questionTextBox = (TextBox)FindName($"Question{i}");
                TextBox answerATextBox = (TextBox)FindName($"AnswerA{i}");
                TextBox answerBTextBox = (TextBox)FindName($"AnswerB{i}");
                TextBox answerCTextBox = (TextBox)FindName($"AnswerC{i}");

                if (string.IsNullOrWhiteSpace(questionTextBox.Text) || string.IsNullOrWhiteSpace(answerATextBox.Text) || string.IsNullOrWhiteSpace(answerBTextBox.Text) || string.IsNullOrWhiteSpace(answerCTextBox.Text))
                {
                    return false;
                }
            }

            return true;
        }

        //Creates quiz
        public async Task CreateQuizQuestionsJsonAsync()
        {
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "quiz.json");

            List<Question> questions;

            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);
                questions = JsonSerializer.Deserialize<List<Question>>(json);
            }
            else
            {
                questions = new List<Question>();
            }

            string title = TitleBox.Text.ToLower();
            if (questions.Any(q => q.Title.ToLower() == title))
            {
                MessageBox.Show("Quiz with the same title already exists.\n Please choose a different title.", "Title Already Exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int questionIndex = 1;

            while (true)
            {
                TextBox questionTextBox = (TextBox)FindName($"Question{questionIndex}");
                TextBox answerATextBox = (TextBox)FindName($"AnswerA{questionIndex}");
                TextBox answerBTextBox = (TextBox)FindName($"AnswerB{questionIndex}");
                TextBox answerCTextBox = (TextBox)FindName($"AnswerC{questionIndex}");

                if (questionTextBox == null || answerATextBox == null || answerBTextBox == null || answerCTextBox == null)
                {
                    break;
                }

                ImageItem selectedImageItem = (ImageItem)ImageList.SelectedItem;
                string imagePath = selectedImageItem != null ? selectedImageItem.ImagePath : "";

                Question newQuestion = CreateQuestion(questionTextBox, answerATextBox, answerBTextBox, answerCTextBox, imagePath);

                if (newQuestion != null)
                {
                    questions.Add(newQuestion);
                }

                questionIndex++;
            }

            string updatedJson = JsonSerializer.Serialize(questions);
            await File.WriteAllTextAsync(filePath, updatedJson);
        }
        private Question CreateQuestion(TextBox questionTextBox, TextBox answerATextBox, TextBox answerBTextBox, TextBox answerCTextBox, string imagePath)
        {
            string questionText = questionTextBox.Text;
            string answerAText = answerATextBox.Text;
            string answerBText = answerBTextBox.Text;
            string answerCText = answerCTextBox.Text;
            string title = TitleBox.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(questionText) && !string.IsNullOrWhiteSpace(answerAText) && !string.IsNullOrWhiteSpace(answerBText) && !string.IsNullOrWhiteSpace(answerCText))
            {
                return new Question(questionText, new string[] { answerAText, answerBText, answerCText }, 0, title, imagePath);
            }
            else
            {
                return null;
            }
        }
    }
    public class ImageItem
    {
        public string ImagePath { get; set; }
        public string Text { get; set; }
    }
}
