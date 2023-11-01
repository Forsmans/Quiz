using QuizGameWPF.Core;
using QuizGameWPF.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuizGameWPF.MVVM.ViewModel
{
    //testpush
    class QuizViewModel : ObservableObject
    {
        private Quiz _currentQuiz;
        private Question _currentQuestion;

        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set
            {
                _currentQuestion = value;
                OnPropertyChanged();
            }
        }
        public Quiz CurrentQuiz
        {
            get { return _currentQuiz; }
            set
            {
                _currentQuiz = value;
                OnPropertyChanged();
            }
        }

        public QuizViewModel(string title)
        {
            InitializeAsync(title).GetAwaiter().GetResult();
        }

        //Initialize quiz
        public async Task<QuizViewModel> InitializeAsync(string title)
        {
            CreateQuizQuestionsJason();
            Quiz quiz = new(title, await LoadQuestionsFromJsonFile(title));
            CurrentQuiz = quiz;
            CurrentQuestion = CurrentQuizQuestion(quiz);
            return this;
        }
        public Question CurrentQuizQuestion(Quiz quiz)
        {
            return CurrentQuiz.GetRandomQuestion();
        }


        //Loading quiz
        public async Task<List<Question>> LoadQuestionsFromJsonFile(string title)
        {
            if (QuizInit.ChoosenCategory != null)
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "quiz.json");

                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    List<Question> questions = JsonSerializer.Deserialize<List<Question>>(json);

                    if (title == "Random")
                    {
                        var filteredQuestions = questions
                            .Where(q => q.Title != "Default")
                            .ToList();

                        List<Question> randomQuestions = new List<Question>();
                        Random random = new Random();

                        while (randomQuestions.Count < 10 && filteredQuestions.Count > 0)
                        {
                            int index = random.Next(filteredQuestions.Count);
                            var randomQuestion = filteredQuestions[index];

                            randomQuestions.Add(randomQuestion);
                            filteredQuestions.RemoveAt(index);
                        }

                        return randomQuestions;
                    }
                    else
                    {
                        if (QuizInit.ChoosenCategory.Contains(","))
                        {
                            List<Question> randomQuestions = new List<Question>();
                            Random random = new Random();

                            var categories = QuizInit.ChoosenCategory.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            var filteredQuestions = questions
                                .Where(q => categories.Any(category => string.Equals(q.Title, category, StringComparison.OrdinalIgnoreCase) && q.Title != "Default"))
                                .ToList();

                            while (randomQuestions.Count < 10 && filteredQuestions.Count > 0)
                            {
                                int index = random.Next(filteredQuestions.Count);
                                var randomQuestion = filteredQuestions[index];

                                randomQuestions.Add(randomQuestion);
                                filteredQuestions.RemoveAt(index);
                            }
                            return randomQuestions;
                        }

                        else
                        {
                            var groupedQuestions = questions
                            .Where(q => q.Title == title)
                            .GroupBy(q => q.Statement)
                            .Select(group => group.First())
                            .ToList();

                            return groupedQuestions;
                        }
                    }
                }

                return new List<Question>();
            }
            return new List<Question>();
        }

        //Create default quiz question on first startup
        public void CreateQuizQuestionsJason()
        {
            List<Question> sportsQuestions = new List<Question>
            {
                new Question("Which sport is known as the 'gentleman's game'?", new string[] { "Cricket", "Soccer", "Tennis" }, 0, "sports", "/images/sports.png"),
                new Question("In which sport would you perform a slam dunk?", new string[] { "Basketball", "Swimming", "Table Tennis" }, 0,"sports", "/images/sports.png"),
                new Question("What is the most popular sport in the world?", new string[] { "Soccer", "Golf", "Badminton" }, 0, "sports", "/images/sports.png"),
                new Question("In which sport would you perform a 'hat-trick'?", new string[] { "Cricket", "Rugby", "Boxing" }, 0, "sports", "/images/sports.png"),
                new Question("Which country is famous for its passion for rugby?", new string[] { "New Zealand", "Brazil", "India" }, 0, "sports", "/images/sports.png"),
                new Question("Who holds the record for the most home runs in Major League Baseball?", new string[] { "Barry Bonds", "Usain Bolt", "Michael Jordan" }, 0, "sports", "/images/sports.png"),
                new Question("In which sport can you score a 'try'?", new string[] { "Rugby", "Tennis", "Swimming" }, 0, "sports", "/images/sports.png"),
                new Question("Which sport involves a shuttlecock and a net?", new string[] { "Badminton", "Ice Hockey", "Formula 1" }, 0, "sports", "/images/sports.png"),
                new Question("What is the national sport of Japan?", new string[] { "Sumo Wrestling", "Archery", "Skiing" }, 0, "sports", "/images/sports.png"),
                new Question("Which sport is associated with Wimbledon and the US Open?", new string[] { "Tennis", "Golf", "Boxing" }, 0, "sports", "/images/sports.png")
            };

            List<Question> musicQuestions = new List<Question>
            {
                new Question("Which song by Queen features the iconic line 'Is this the real life, is this just fantasy?'", new string[] { "Bohemian Rhapsody", "We Will Rock You", "Another One Bites the Dust" }, 0, "music", "/images/music.png"),
                new Question("Who is often referred to as the 'King of Pop'?", new string[] { "Michael Jackson", "Elvis Presley", "Prince" }, 0, "music", "/images/music.png"),
                new Question("Which Beatles album cover featured the band crossing the street at a zebra crossing?", new string[] { "Abbey Road", "Sgt. Pepper's Lonely Hearts Club Band", "The White Album" }, 0, "music", "/images/music.png"),
                new Question("What musical instrument does the character Mozart play in the film 'Amadeus'?", new string[] { "Piano", "Violin", "Flute" }, 0, "music", "/images/music.png"),
                new Question("Which famous rapper's real name is Marshall Bruce Mathers III?", new string[] { "Eminem", "Jay-Z", "Snoop Dogg" }, 0, "music", "/images/music.png"),
                new Question("Who sang the hit song 'Hallelujah' that was featured in the movie 'Shrek'?", new string[] { "Leonard Cohen", "John Legend", "Adele" }, 0, "music", "/images/music.png"),
                new Question("In which city did Elvis Presley live at Graceland?", new string[] { "Memphis", "Las Vegas", "Nashville" }, 0, "music", "/images/music.png"),
                new Question("What was the first music video ever played on MTV?", new string[] { "Video Killed the Radio Star", "Billie Jean", "Bohemian Rhapsody" }, 0, "music", "/images/music.png"),
                new Question("Which rock band released the album 'The Dark Side of the Moon'?", new string[] { "Pink Floyd", "Led Zeppelin", "The Rolling Stones" }, 0, "music", "/images/music.png"),
                new Question("Who is known as the 'Piano Man' and released the song of the same name?", new string[] { "Billy Joel", "Elton John", "John Lennon" }, 0, "music", "/images/music.png")
            };

            List<Question> politicsQuestions = new List<Question>
            {
                new Question("Who is the current President of the United States?", new string[] { "Joe Biden", "Donald Trump", "Barack Obama" }, 0, "politics", "/images/politics.png"),
                new Question("What is the capital of France?", new string[] { "Paris", "Berlin", "Madrid" }, 0, "politics", "/images/politics.png"),
                new Question("Which political ideology is associated with Karl Marx?", new string[] { "Communism", "Capitalism", "Socialism" }, 0, "politics", "/images/politics.png"),
                new Question("In which country is the Great Wall located?", new string[] { "China", "India", "Russia" }, 0, "politics", "/images/politics.png"),
                new Question("Who was the first woman to become the Prime Minister of the United Kingdom?", new string[] { "Margaret Thatcher", "Queen Elizabeth II", "Theresa May" }, 0, "politics", "/images/politics.png"),
                new Question("Which country is known as the 'Land of the Rising Sun'?", new string[] { "Japan", "China", "South Korea" }, 0, "politics", "/images/politics.png"),
                new Question("What is the political system in which power is held by a single individual?", new string[] { "Autocracy", "Democracy", "Monarchy" }, 0, "politics", "/images/politics.png"),
                new Question("Who wrote the 'Communist Manifesto'?", new string[] { "Karl Marx and Friedrich Engels", "Vladimir Lenin", "Joseph Stalin" }, 0, "politics", "/images/politics.png"),
                new Question("Which document begins with the words 'We the People'?", new string[] { "The U.S. Constitution", "The Declaration of Independence", "The Bill of Rights" }, 0, "politics", "/images/politics.png"),
                new Question("What is the system of government where power is divided between a central government and regional governments?", new string[] { "Federalism", "Monarchy", "Totalitarianism" }, 0, "politics", "/images/politics.png")
            };

            List<Question> animalQuestions = new List<Question>
            {
                new Question("What is the largest land animal in the world?", new string[] { "African Elephant", "White Rhino", "Giraffe" }, 0, "animals", "/images/animals.png"),
                new Question("Which bird is known for its colorful and elaborate courtship dance?", new string[] { "Peacock", "Bald Eagle", "Ostrich" }, 0, "animals", "/images/animals.png"),
                new Question("What is the fastest land animal on Earth?", new string[] { "Cheetah", "Lion", "Gazelle" }, 0, "animals", "/images/animals.png"),
                new Question("Which species of big cat is known for its distinctive black mane?", new string[] { "Lion", "Tiger", "Leopard" }, 0, "animals", "/images/animals.png"),
                new Question("What is the largest species of shark?", new string[] { "Whale Shark", "Great White Shark", "Hammerhead Shark" }, 0, "animals", "/images/animals.png"),
                new Question("What is the national bird of the United States?", new string[] { "Bald Eagle", "Peregrine Falcon", "American Robin" }, 0, "animals", "/images/animals.png"),
                new Question("Which animal is known as the 'King of the Jungle'?", new string[] { "Lion", "Tiger", "Leopard" }, 0, "animals", "/images/animals.png"),
                new Question("What is the largest species of penguin?", new string[] { "Emperor Penguin", "African Penguin", "King Penguin" }, 0, "animals", "/images/animals.png"),
                new Question("Which reptile is known for changing the color of its skin?", new string[] { "Chameleon", "Iguana", "Komodo Dragon" }, 0, "animals", "/images/animals.png"),
                new Question("What is the largest species of bear?", new string[] { "Kodiak Bear", "Polar Bear", "Brown Bear" }, 0, "animals", "/images/animals.png")
            };

            List<Question> generalKnowledgeQuestions = new List<Question>
            {
                new Question("What is the capital of France?", new string[] { "Paris", "Madrid", "Berlin" }, 0, "general knowledge", "/images/generalknowledge.png"),
                new Question("Which planet is known as the 'Red Planet'?", new string[] { "Mars", "Venus", "Saturn" }, 0, "general knowledge", "/images/generalknowledge.png"),
                new Question("Who wrote 'Romeo and Juliet'?", new string[] { "William Shakespeare", "Charles Dickens", "Leo Tolstoy" }, 0, "general knowledge", "/images/generalknowledge.png"),
                new Question("What is the largest planet in our solar system?", new string[] { "Jupiter", "Saturn", "Neptune" }, 0, "general knowledge", "/images/generalknowledge.png"),
                new Question("Which gas do plants absorb from the atmosphere?", new string[] { "Carbon Dioxide", "Oxygen", "Nitrogen" }, 0, "general knowledge", "/images/generalknowledge.png"),
                new Question("Which country is known as the 'Land of the Rising Sun'?", new string[] { "Japan", "China", "South Korea" }, 0, "general knowledge", "/images/generalknowledge.png"),
                new Question("What is the chemical symbol for gold?", new string[] { "Au", "Ag", "Fe" }, 0, "general knowledge", "/images/generalknowledge.png"),
                new Question("Who painted the Mona Lisa?", new string[] { "Leonardo da Vinci", "Pablo Picasso", "Vincent van Gogh" }, 0, "general knowledge", "/images/generalknowledge.png"),
                new Question("What is the largest ocean in the world?", new string[] { "Pacific Ocean", "Atlantic Ocean", "Indian Ocean" }, 0, "general knowledge", "/images/generalknowledge.png"),
                new Question("In which year did Christopher Columbus discover America?", new string[] { "1492", "1776", "1620" }, 0, "general knowledge", "/images/generalknowledge.png")
            };

            List<Question> movieQuestions = new List<Question>
            {
                new Question("Who played the role of Jack Dawson in the movie 'Titanic'?", new string[] { "Leonardo DiCaprio", "Brad Pitt", "Tom Hanks" }, 0, "movies", "/images/movie.png"),
                new Question("Which film won the Academy Award for Best Picture in 1994?", new string[] { "Forrest Gump", "Pulp Fiction", "The Shawshank Redemption" }, 0, "movies", "/images/movie.png"),
                new Question("Who directed 'The Dark Knight' trilogy?", new string[] { "Christopher Nolan", "Steven Spielberg", "Martin Scorsese" }, 0, "movies", "/images/movie.png"),
                new Question("In 'The Lord of the Rings' trilogy, who played the character Frodo Baggins?", new string[] { "Elijah Wood", "Daniel Radcliffe", "Rupert Grint" }, 0, "movies", "/images/movie.png"),
                new Question("What is the highest-grossing animated film of all time?", new string[] { "The Lion King", "Frozen", "Toy Story 3" }, 0, "movies", "/images/movie.png"),
                new Question("Which actor portrayed Tony Stark/Iron Man in the Marvel Cinematic Universe?", new string[] { "Robert Downey Jr.", "Chris Hemsworth", "Mark Ruffalo" }, 0, "movies", "/images/movie.png"),
                new Question("In 'The Matrix' series, who played the role of Neo?", new string[] { "Keanu Reeves", "Will Smith", "Tom Cruise" }, 0, "movies", "/images/movie.png"),
                new Question("Who directed 'Jurassic Park'?", new string[] { "Steven Spielberg", "James Cameron", "George Lucas" }, 0, "movies", "/images/movie.png"),
                new Question("Which movie features a character named 'Hannibal Lecter'?", new string[] { "The Silence of the Lambs", "Se7en", "Shutter Island" }, 0, "movies", "/images/movie.png"),
                new Question("What 1994 film was the first feature-length movie created entirely through computer-generated imagery (CGI)?", new string[] { "Toy Story", "Jurassic Park", "Avatar" }, 0, "movies", "/images/movie.png")
            };

            List<Question> historyQuestions = new List<Question>
            {
                new Question("Who was the first President of the United States?", new string[] { "George Washington", "Thomas Jefferson", "John Adams" }, 0, "history", "/images/history.png"),
                new Question("In which year did World War II end?", new string[] { "1945", "1939", "1941" }, 0, "history", "/images/history.png"),
                new Question("What was the ancient Egyptian writing system called?", new string[] { "Hieroglyphics", "Cuneiform", "Runes" }, 0, "history", "/images/history.png"),
                new Question("Who is known for the theory of relativity?", new string[] { "Albert Einstein", "Isaac Newton", "Galileo Galilei" }, 0, "history", "/images/history.png"),
                new Question("Which explorer is credited with discovering America in 1492?", new string[] { "Christopher Columbus", "Ferdinand Magellan", "Amerigo Vespucci" }, 0, "history", "/images/history.png"),
                new Question("In what year was the Magna Carta signed?", new string[] { "1215", "1492", "1776" }, 0, "history", "/images/history.png"),
                new Question("Who was the first woman to fly solo across the Atlantic Ocean?", new string[] { "Amelia Earhart", "Bessie Coleman", "Harriet Quimby" }, 0, "history", "/images/history.png"),
                new Question("The Great Wall of China was primarily built to protect against attacks from which group?", new string[] { "Mongols", "Vikings", "Romans" }, 0, "history", "/images/history.png"),
                new Question("What event is often considered the start of World War I?", new string[] { "Assassination of Archduke Franz Ferdinand", "Bombing of Hiroshima and Nagasaki", "Signing of the Treaty of Versailles" }, 0, "history", "/images/history.png"),
                new Question("Which ancient civilization is known for constructing the pyramids in Egypt?", new string[] { "Ancient Egyptians", "Mayans", "Romans" }, 0, "history", "/images/history.png")
            };

            List<Question> geographyQuestions = new List<Question>
            {
                new Question("Which city is located at the confluence of the Danube and Sava rivers and is the capital of Serbia?", new string[] { "Belgrade", "Bucharest", "Zagreb" }, 0, "geography", "/images/georaphy.png"),
                new Question("What is the largest desert in the world after Antarctica?", new string[] { "Sahara Desert", "Gobi Desert", "Arabian Desert" }, 1, "geography", "/images/georaphy.png"),
                new Question("Which country is the southernmost point in Africa?", new string[] { "South Africa", "Namibia", "Botswana" }, 0, "geography", "/images/georaphy.png"),
                new Question("The Great Barrier Reef is located off the coast of which country?", new string[] { "Australia", "Indonesia", "Philippines" }, 0, "geography", "/images/georaphy.png"),
                new Question("Which river runs through Cairo, the capital of Egypt?", new string[] { "Nile", "Tigris", "Euphrates" }, 0, "geography", "/images/georaphy.png"),
                new Question("What is the world's second-largest continent by land area?", new string[] { "Africa", "Asia", "North America" }, 1, "geography", "/images/georaphy.png"),
                new Question("Which country is known as the Land of a Thousand Lakes?", new string[] { "Finland", "Sweden", "Norway" }, 0, "geography", "/images/georaphy.png"),
                new Question("In which country is the city of Machu Picchu, an ancient Incan citadel?", new string[] { "Peru", "Chile", "Ecuador" }, 0, "geography", "/images/georaphy.png"),
                new Question("Which strait separates Asia from North America?", new string[] { "Bering Strait", "Strait of Gibraltar", "Hormuz Strait" }, 0, "geography", "/images/georaphy.png"),
                new Question("What is the largest island in the Mediterranean Sea?", new string[] { "Sicily", "Sardinia", "Cyprus" }, 0, "geography", "/images/georaphy.png")
            };

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "quiz.json");

            List<Question> allQuestions = new List<Question>();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<Question> existingQuestions = JsonSerializer.Deserialize<List<Question>>(json);

                allQuestions.AddRange(existingQuestions);
            }

            allQuestions.AddRange(sportsQuestions);
            allQuestions.AddRange(musicQuestions);
            allQuestions.AddRange(politicsQuestions);
            allQuestions.AddRange(animalQuestions);
            allQuestions.AddRange(generalKnowledgeQuestions);
            allQuestions.AddRange(movieQuestions);
            allQuestions.AddRange(historyQuestions);
            allQuestions.AddRange(geographyQuestions);

            string updatedJson = JsonSerializer.Serialize(allQuestions);

            File.WriteAllText(filePath, updatedJson);
        }

    }

}
