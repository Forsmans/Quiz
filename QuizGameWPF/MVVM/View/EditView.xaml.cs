using QuizGameWPF.MVVM.Model;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for EditView.xaml
    /// </summary>
    public partial class EditView : UserControl
    {
        public string QuizToEdit;
        public EditView()
        {
            InitializeComponent();
            string imageDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            List<ImageItem> imagePaths = GetImagesFromDirectory();

            ImageList.ItemsSource = imagePaths;
            LoadQuizTitles();
        }

        //Loading the list of quizes to edit 
        private void LoadQuizTitles()
        {
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "quiz.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<Question> questions = JsonSerializer.Deserialize<List<Question>>(json);

                var titlesWithImages = questions
                    .Select(q => new { Title = ToTitleCase(q.Title), ImagePath = q.ImagePath })
                    .Where(title => title.Title != "Default" && title.Title != "Music" && title.Title != "Sports" && title.Title != "Politics" 
                    && title.Title != "Animals" && title.Title != "General Knowledge" && title.Title != "Movies" && title.Title != "History" && title.Title != "Geography")
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


        private void EditTitleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TitleList.SelectedItem != null)
            {
                var selectedTitleWithImage = (dynamic)TitleList.SelectedItem; 
                string selectedTitle = selectedTitleWithImage.Title.ToLower();
                string selectedImagePath = selectedTitleWithImage.ImagePath;
                QuizToEdit = selectedTitleWithImage.Title.ToLower();

                DisplayQuestionsAndAnswers(selectedTitle);
            }
        }

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

        //Displaying the quiz in the textboxes 
        private async Task DisplayQuestionsAndAnswers(string selectedTitle)
        {
            ClearTextboxes();

            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "quiz.json");

            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath); // Use async file reading
                List<Question> questions = JsonSerializer.Deserialize<List<Question>>(json);

                var selectedQuestions = questions
                    .Where(q => q.Title == selectedTitle)
                    .ToList();

                TextBox TitleBox = FindName("TitleBox") as TextBox;

                if (TitleBox != null)
                {
                    TitleBox.Text = selectedTitle;
                }

                for (int i = 0; i < selectedQuestions.Count; i++)
                {
                    TextBox questionTextBox = FindName($"Question{i + 1}") as TextBox;
                    TextBox answerATextBox = FindName($"AnswerA{i + 1}") as TextBox;
                    TextBox answerBTextBox = FindName($"AnswerB{i + 1}") as TextBox;
                    TextBox answerCTextBox = FindName($"AnswerC{i + 1}") as TextBox;

                    if (questionTextBox != null && answerATextBox != null && answerBTextBox != null && answerCTextBox != null)
                    {
                        questionTextBox.Text = selectedQuestions[i].Statement;
                        answerATextBox.Text = selectedQuestions[i].Answers[0];
                        answerBTextBox.Text = selectedQuestions[i].Answers[1];
                        answerCTextBox.Text = selectedQuestions[i].Answers[2];
                    }
                }
            }
        }

        private void ClearTextboxes()
        {
            for (int i = 1; i <= 11; i++)
            {
                TextBox questionTextBox = (TextBox)FindName($"Question{i}");
                TextBox answerATextBox = (TextBox)FindName($"AnswerA{i}");
                TextBox answerBTextBox = (TextBox)FindName($"AnswerB{i}");
                TextBox answerCTextBox = (TextBox)FindName($"AnswerC{i}");

                questionTextBox.Text = "";
                answerATextBox.Text = "";
                answerBTextBox.Text = "";
                answerCTextBox.Text = "";
            }
        }



        //Button methods
        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            int numberOfQuestions = 3;

            if (!await IsQuizNameTaken())
            {
                if (AreAllTextboxesFilled(numberOfQuestions))
                {
                    await RemoveOldQuizJson();
                    CreateQuizQuestionsJson();
                    MessageBox.Show("Your quiz has been successfully updated\n It's now updated in the Discover list.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("You have to fill in all the fields for each question.", "Incomplete Form", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }


        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if(QuizToEdit != null)
            {
                RemoveOldQuizJson();
                MessageBox.Show("Your quiz has been successfully deleted!", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("You have to choose a quiz to delete", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }

        //Remove Quiz
        private async Task RemoveOldQuizJson()
        {
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "quiz.json");

            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);
                List<Question> questions = JsonSerializer.Deserialize<List<Question>>(json);

                questions = questions.Where(q => q.Title != QuizToEdit).ToList();

                string newJson = JsonSerializer.Serialize(questions);

                await File.WriteAllTextAsync(filePath, newJson);
            }
        }

        //Check if name is already taken
        private async Task<bool> IsQuizNameTaken()
        {
            string title = TitleBox.Text.ToLower();
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "quiz.json");

            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);
                List<Question> questions = JsonSerializer.Deserialize<List<Question>>(json);

                if (title != QuizToEdit)
                {
                    if (questions.Any(q => q.Title.ToLower() == title))
                    {
                        MessageBox.Show("Quiz with the same title already exists.\n Please choose a different title.", "Title Already Taken", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return true;
                    }
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

        //Create quiz 
        public async Task CreateQuizQuestionsJson()
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

        public class ImageItem
        {
            public string ImagePath { get; set; }
            public string Text { get; set; }
        }

    }
}
