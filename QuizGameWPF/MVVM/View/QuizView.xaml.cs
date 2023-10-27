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
        private int CorrectIndex;
        public QuizView()
        {
            InitializeComponent();

            viewModel = new QuizViewModel(QuizInit.ChoosenCategory);
            DataContext = viewModel;

            RepositionAnswers();

            QuizInit.IndexOfQuiz = 1;
            QuizInit.CorrectAnswers = 0;
            QuizInit.TotalQuestions = viewModel.CurrentQuiz.Questions.Count;
            QuizPosition.Text = $"{QuizInit.IndexOfQuiz}/{QuizInit.TotalQuestions}";
        }

        //Shuffle the position of the answers so they are not always in the same place
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
            CorrectIndex = viewModel.CurrentQuestion.CorrectAnswer;

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

        //Answer button method
        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if(QuizInit.ChoosenCategory != null)
            {
                if (sender is Button button && int.TryParse(button.Tag.ToString(), out int selectedAnswer))
                {
                    if(AnswerA.Content != "")
                    {
                        if (selectedAnswer == viewModel.CurrentQuestion.CorrectAnswer)
                        {
                            QuizInit.BarProgress++;
                            QuizInit.CorrectAnswers++;
                            MessageBox.Show("Correct, good job!", "Correct", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show($"Incorrect, the right answer is:\n{viewModel.CurrentQuestion.Answers[CorrectIndex]}", "Incorrect", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        int currentIndex = viewModel.CurrentQuiz.Questions.IndexOf(viewModel.CurrentQuestion);

                        if (currentIndex >= 0)
                        {
                            viewModel.CurrentQuiz.RemoveQuestion(currentIndex);
                        }

                        viewModel.CurrentQuestion = viewModel.CurrentQuiz.GetRandomQuestion();

                        RepositionAnswers();

                        if (QuizInit.IndexOfQuiz < QuizInit.TotalQuestions)
                        {
                            QuizInit.IndexOfQuiz++;
                        }
                        QuizPosition.Text = $"{QuizInit.IndexOfQuiz}/{QuizInit.TotalQuestions}";
                    }
                }
            }
        }
    }
}
