using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuizGameWPF.MVVM.Model
{
    public class Question
    {
        public string Statement { get; }
        public string[] Answers { get; }
        public int CorrectAnswer { get; }
        public string Title { get; }
        public string ImagePath { get; }

        public Question(string statement, string[] answers, int correctAnswer, string title, string imagePath)
        {
            Statement = statement;
            Answers = answers;
            CorrectAnswer = correctAnswer;
            Title = title;
            ImagePath = imagePath;
        }
        
    }

}
