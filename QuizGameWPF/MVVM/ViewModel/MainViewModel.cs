using QuizGameWPF.Core;
using QuizGameWPF.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGameWPF.MVVM.ViewModel
{
    
    //For changing the CurrentView in the MainWindow
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DiscoverViewCommand { get; set; }
        public RelayCommand GameViewCommand { get; set; }
        public RelayCommand CreateViewCommand { get; set; }
        public RelayCommand EditViewCommand { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public DiscoverViewModel DiscoverVM { get; set; }
        public QuizViewModel QuizVM { get; set; }
        public CreateViewModel CreateVM { get; set; }
        public EditViewModel EditVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            DiscoverVM = new DiscoverViewModel();
            QuizVM = new QuizViewModel(QuizInit.ChoosenCategory);
            CreateVM = new CreateViewModel();
            EditVM = new EditViewModel();

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            DiscoverViewCommand = new RelayCommand(o =>
            {
                
                CurrentView = DiscoverVM;
            });

            GameViewCommand = new RelayCommand(o =>
            {
                CurrentView = QuizVM;
            });

            CreateViewCommand = new RelayCommand(o =>
            {
                CurrentView = CreateVM;
            });

            EditViewCommand = new RelayCommand(o =>
            {
                CurrentView = EditVM;
            });
        }

    }
}
