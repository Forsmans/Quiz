using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGameWPF.MVVM.Model
{
    //For handling stuff i need to access across the whole application
    public static class QuizInit
    {
        public static string ChoosenCategory { get; set; }
        public static int CorrectAnswers { get; set; }
        public static int TotalQuestions { get; set; }
        public static int IndexOfQuiz { get; set; }

        private static int _barProgress;

        private static string _playColor;
        public static string PlayColor
        {
            get { return _playColor; }
            set
            {
                if (_playColor != value)
                {
                    _playColor = value;
                    OnPlayColorChanged(value);
                }
            }
        }
        public static int BarProgress
        {
            get { return _barProgress; }
            set
            {
                if (_barProgress != value)
                {
                    _barProgress = value;
                    OnBarProgressChanged(value);
                }
            }
        }

        public static event EventHandler<int> BarProgressChanged;

        private static void OnBarProgressChanged(int newProgress)
        {
            BarProgressChanged?.Invoke(null, newProgress);
        }

        public static event EventHandler<string> PlayColorChanged;

        private static void OnPlayColorChanged(string newColor)
        {
            PlayColorChanged?.Invoke(null, newColor);
        }


    }
}
