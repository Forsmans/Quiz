using QuizGameWPF.MVVM.Model;
using QuizGameWPF.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using QuizGameWPF.MVVM.ViewModel;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;

namespace QuizGameWPF.MVVM.View
{

    public partial class QuizView : UserControl
    {

        private QuizViewModel viewModel;

        public QuizView()
        {
            InitializeComponent();
            viewModel = new QuizViewModel(QuizInit.ChoosenCategory);
            DataContext = viewModel;
            RepositionAnswers();
        }

        private void RepositionAnswers()
        {
            if(viewModel.CurrentQuestion == null) 
            {
                viewModel.CurrentQuestion = new Question("Choose a quiz in 'Discover' to start!", new string[] { "", "", "" }, 3, "Default", "/images/quizlogo.png");
            }

            List<string> shuffledAnswers = ShuffleAnswers(viewModel.CurrentQuestion.Answers.ToList());

            AnswerA.Content = shuffledAnswers[0];
            AnswerB.Content = shuffledAnswers[1];
            AnswerC.Content = shuffledAnswers[2];
        }

        private List<string> ShuffleAnswers(List<string> answers)
        {
            var random = new Random();
            int correctAnswerIndex = viewModel.CurrentQuestion.CorrectAnswer;
            List<int> tagValues = Enumerable.Range(0, answers.Count).ToList();

            for (int i = 0; i < answers.Count; i++)
            {
                int newIndex = random.Next(i, answers.Count);
             
                int tempTag = tagValues[i];
                tagValues[i] = tagValues[newIndex];
                tagValues[newIndex] = tempTag;
              
                string tempAnswer = answers[i];
                answers[i] = answers[newIndex];
                answers[newIndex] = tempAnswer;
            }

            AnswerA.Content = answers[tagValues[0]];
            AnswerA.Tag = tagValues[0];

            AnswerB.Content = answers[tagValues[1]];
            AnswerB.Tag = tagValues[1];

            AnswerC.Content = answers[tagValues[2]];
            AnswerC.Tag = tagValues[2];

            return answers;
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Tag.ToString(), out int selectedAnswer))
            {
                if (selectedAnswer == viewModel.CurrentQuestion.CorrectAnswer)
                {
                    QuizInit.BarProgress++;
                    MessageBox.Show("Yes, great!","Correct", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    // Remove the current question from the quiz's question list
                    int currentIndex = viewModel.CurrentQuiz.Questions.IndexOf(viewModel.CurrentQuestion);
                    if (currentIndex >= 0)
                    {
                        viewModel.CurrentQuiz.RemoveQuestion(currentIndex);
                    }

                    viewModel.CurrentQuestion = viewModel.CurrentQuiz.GetRandomQuestion();
                    //viewModel.CurrentQuestion = viewModel.CurrentQuizQuestion(viewModel.CurrentQuiz);
                }
                else
                {
                    MessageBox.Show("Nope, try again...", "Incorrect", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                RepositionAnswers();
            }
        }
    }
}
