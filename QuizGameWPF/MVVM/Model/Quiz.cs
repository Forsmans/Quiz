using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace QuizGameWPF.MVVM.Model
{
    public class Quiz
    {
        private List<Question> _questions;
        private string _title = string.Empty;

        public List<Question> Questions => _questions;
        public string Title => _title;

        public Quiz(string title, List<Question> questions)
        {
            _title = title;
            _questions = questions;
        }        
        public Question GetRandomQuestion()
        {
            if(QuizInit.ChoosenCategory != null)
            {
                if (_questions.Count > 0)
                {
                    Random random = new Random();
                    int randomIndex = random.Next(0, _questions.Count);
                    return _questions[randomIndex];
                }
                else
                {
                    AddQuestion($"Congratulations, you answered all the questions!\n You have answered {QuizInit.CorrectAnswers} of {QuizInit.TotalQuestions} questions correct\n Choose another category in Discover!", 3, "/images/quizlogo.png", "Default", new string[] { "", "", "" });
                    return Questions[0];
                }
            }
            AddQuestion("Choose a quiz in 'Discover' to start!", 3, "/images/quizlogo.png", "Default", new string[] { "", "", "" });
            return Questions[0];
        }

        public void AddQuestion(string statement, int correctAnswer, string imagepath, string title, params string[] answers)
        {
            Question newQuestion = new Question(statement, answers, correctAnswer, title, imagepath);
            _questions.Add(newQuestion);
        }

        public void RemoveQuestion(int index)
        {
            if (index >= 0 && index < _questions.Count)
            {
                _questions.RemoveAt(index);
            }
        }



    }

}