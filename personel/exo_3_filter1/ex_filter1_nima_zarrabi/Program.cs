using ex_filter1_nima_zarrabi;
using System;
using System.IO;
using System.Linq;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] bannedGenreList = ["Drame", "Comédie"];

            const int MAXDATE = 2000;
            const double MOVIEMAXRATING = 8;
            const double MOVIEMINRATING = 8.2;
            const int INFOMARGIN = 45;

            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            List<string> frenchWords = new List<string>()
            {
                "Merci",
                "Hotdog",
                "Oui",
                "Non",
                "Désolé",
                "Réunion",
                "Manger",
                "Boire",
                "Téléphone",
                "Ordinateur",
                "Internet",
                "Email",
                "Sandwich",
                "Hello",
                "Taxi",
                "Hotel",
                "Gare",
                "Train",
                "Bus",
                "Métro",
                "Tramway",
                "Vélo",
                "Voiture",
                "Piéton",
                "Feu rouge",
                "Cédez",
                "Ralentir",
                "gauche",
                "droite",
                "Continuer",
                "Sandwich",
                "Retourner",
                "Arrêter",
                "Stationnement",
                "Parking",
                "Interdit",
                "Péage",
                "Trafic",
                "Route",
                "Rond-point",
                "Football",
                "Carrefour",
                "Feu",
                "Panneau",
                "Vitesse",
                "Tramway",
                "Aéroport",
                "Héliport",
                "Port",
                "Ferry",
                "Bateau",
                "Canot",
                "Kayak",
                "Paddle",
                "Surf",
                "Plage",
                "Mer",
                "Océan",
                "Rivière",
                "Lac",
                "Étang",
                "Marais",
                "Forêt",
                "Hello",
                "Montagne",
                "Vallée",
                "Plaine",
                "Désert",
                "Jungle",
                "Savane",
                "Volleyball",
                "Tundra",
                "Glacier",
                "Neige",
                "Pluie",
                "Soleil",
                "Nuage",
                "Vent",
                "Tempête",
                "Ouragan",
                "Tornade",
                "Séisme",
                "Tsunami",
                "Volcan",
                "Éruption",
                "Ciel",
            };

            List<Movie> frenchMovies = new List<Movie>() {
new Movie(0) { Title = "Le fabuleux destin d'Amélie Poulain", Genre = "Comédie", Rating = 8.3, Year = 2001, LanguageOptions = new string[] {"Français", "English"}, StreamingPlatforms = new string[] {"Netflix", "Hulu"} },
new Movie(1) { Title = "Intouchables", Genre = "Comédie", Rating = 8.5, Year = 2011, LanguageOptions = new string[] {"Français"}, StreamingPlatforms = new string[] {"Netflix", "Amazon"} },
new Movie(2) { Title = "The Matrix", Genre = "Science-Fiction", Rating = 8.7, Year = 1999, LanguageOptions = new string[] {"English", "Español"}, StreamingPlatforms = new string[] {"Hulu", "Amazon"} },
new Movie(3) { Title = "La Vie est belle", Genre = "Drame", Rating = 8.6, Year = 1946, LanguageOptions = new string[] {"Français", "Italiano"}, StreamingPlatforms = new string[] {"Netflix"} },
new Movie(4) { Title = "Gran Torino", Genre = "Drame", Rating = 8.2, Year = 2008, LanguageOptions = new string[] {"English"}, StreamingPlatforms = new string[] {"Hulu"} },
new Movie(5) { Title = "La Haine", Genre = "Drame", Rating = 8.1, Year = 1995, LanguageOptions = new string[] {"Français"}, StreamingPlatforms = new string[] {"Netflix"} },
new Movie(6) { Title = "Oldboy", Genre = "Thriller", Rating = 8.4, Year = 2003, LanguageOptions = new string[] {"Coréen", "English"}, StreamingPlatforms = new string[] {"Amazon"} },
new Movie(7) { Title = "Les Choristes", Genre = "Drame", Rating = 7.9, Year = 2004, LanguageOptions = new string[] {"Français", "English"}, StreamingPlatforms = new string[] {"Netflix", "Amazon"} },
new Movie(8) { Title = "Amour", Genre = "Romance", Rating = 7.9, Year = 2012, LanguageOptions = new string[] {"Français"}, StreamingPlatforms = new string[] {"Netflix", "Hulu"} },
new Movie(9) { Title = "Le Samouraï", Genre = "Thriller", Rating = 8.1, Year = 1967, LanguageOptions = new string[] {"Français", "English"}, StreamingPlatforms = new string[] {"Amazon"} },
new Movie(10) { Title = "Les Misérables", Genre = "Drame", Rating = 7.6, Year = 2019, LanguageOptions = new string[] {"Français"}, StreamingPlatforms = new string[] {"Netflix"} },
new Movie(11) { Title = "Un Prophète", Genre = "Crime", Rating = 7.9, Year = 2009, LanguageOptions = new string[] {"Français", "Arabic"}, StreamingPlatforms = new string[] {"Hulu"} },
new Movie(12) { Title = "Persepolis", Genre = "Animation", Rating = 8.0, Year = 2007, LanguageOptions = new string[] {"Français", "English"}, StreamingPlatforms = new string[] {"Amazon", "Hulu"} },
new Movie(13) { Title = "Léon: The Professional", Genre = "Action", Rating = 8.5, Year = 1994, LanguageOptions = new string[] {"Français", "English"}, StreamingPlatforms = new string[] {"Netflix", "Amazon"} },
new Movie(14) { Title = "La Règle du jeu", Genre = "Comédie", Rating = 8.0, Year = 1939, LanguageOptions = new string[] {"Français"}, StreamingPlatforms = new string[] {"Netflix", "Hulu"} },
new Movie(15) { Title = "Le Salaire de la peur", Genre = "Thriller", Rating = 8.2, Year = 1953, LanguageOptions = new string[] {"Français", "English"}, StreamingPlatforms = new string[] {"Amazon"} },
new Movie(16) { Title = "À bout de souffle", Genre = "Drame", Rating = 7.8, Year = 1960, LanguageOptions = new string[] {"Français", "English"}, StreamingPlatforms = new string[] {"Netflix"} },
new Movie(17) { Title = "Les Enfants du Paradis", Genre = "Drame", Rating = 8.3, Year = 1945, LanguageOptions = new string[] {"Français"}, StreamingPlatforms = new string[] {"Hulu"} },
new Movie(18) { Title = "La Jetée", Genre = "Science-Fiction", Rating = 8.3, Year = 1962, LanguageOptions = new string[] {"Français"}, StreamingPlatforms = new string[] {"Amazon"} },
new Movie(19) { Title = "La Femme Nikita", Genre = "Action", Rating = 7.4, Year = 1990, LanguageOptions = new string[] {"Français", "English"}, StreamingPlatforms = new string[] {"Netflix"} },
new Movie(20) { Title = "L'Armée des ombres", Genre = "Drame", Rating = 8.2, Year = 1969, LanguageOptions = new string[] {"Français", "English"}, StreamingPlatforms = new string[] {"Amazon", "Hulu"} },
new Movie(21) { Title = "Les Quatre Cents Coups", Genre = "Drame", Rating = 8.1, Year = 1959, LanguageOptions = new string[] {"Français", "English"}, StreamingPlatforms = new string[] {"Netflix"} },
new Movie(22) { Title = "La Grande Illusion", Genre = "Guerre", Rating = 8.1, Year = 1937, LanguageOptions = new string[] {"Français"}, StreamingPlatforms = new string[] {"Amazon"} },
new Movie(23) { Title = "Le Mépris", Genre = "Drame", Rating = 7.6, Year = 1963, LanguageOptions = new string[] {"Français", "Italiano"}, StreamingPlatforms = new string[] {"Netflix", "Amazon"} },
};

            WordsStartingWithA(frenchWords);

            wordsWithNoEs(frenchWords);

            wordsWithEightOrMoreChar(frenchWords);

            averageLengthWord(frenchWords);

            Console.WriteLine("\nNumbers");

            evenNumbers(numbers);

            Console.WriteLine("\nMovies");

            FiltreMoviesGenre(frenchMovies, bannedGenreList, INFOMARGIN);

            FilterMoviesRatingOver(frenchMovies, MOVIEMINRATING, INFOMARGIN);

            FilterMoviesRatingBelow(frenchMovies, MOVIEMAXRATING, INFOMARGIN);
        }

        static void FiltreMoviesGenre(List<Movie> moviesList, string[] bannedGenre, int margin)
        {
            List<Movie> filteredMovies = moviesList;
            foreach (string genre in bannedGenre)
            {
                filteredMovies = filteredMovies.Where(m => m.Genre != genre).ToList();
            }

            filteredMovies = (from m in filteredMovies
                              orderby m.Genre ascending
                              select m).ToList();

            Console.Write("\nlist of Movies that are not of the type(s): ");
            foreach (string genre in bannedGenre)
            {
                Console.Write(genre);
                Console.Write(" ");
            }
            Console.Write("\nTitle");
            Console.SetCursorPosition(margin, Console.GetCursorPosition().Top);
            Console.Write("   Genre");
            foreach (Movie filteredMovie in filteredMovies)
            {
                Console.WriteLine("\n");
                Console.Write(filteredMovie.Title);
                Console.SetCursorPosition(margin, Console.GetCursorPosition().Top);
                Console.Write("   " + filteredMovie.Genre);
            }
            Console.WriteLine();
        }

        static void FilterMoviesRatingOver(List<Movie> moviesList, double minRating, int margin)
        {

            List<Movie> filteredMovies = moviesList.Where(m => m.Rating > minRating).ToList();

            filteredMovies = (from m in filteredMovies
                              orderby m.Rating descending
                              select m).ToList();

            Console.Write("\nList of Movies that have a rating over " + minRating + ":");
            Console.Write("\nTitle");
            Console.SetCursorPosition(margin, Console.GetCursorPosition().Top);
            Console.Write("   Rating");
            foreach (Movie filteredMovie in filteredMovies)
            {
                Console.WriteLine("\n");
                Console.Write(filteredMovie.Title);
                Console.SetCursorPosition(margin, Console.GetCursorPosition().Top);
                Console.Write("   " + filteredMovie.Rating);
            }
            Console.WriteLine();
        }

        static void FilterMoviesRatingBelow(List<Movie> moviesList, double maxRating, int margin)
        {

            List<Movie> filteredMovies = moviesList.Where(m => m.Rating < maxRating).ToList();

            filteredMovies = (from m in filteredMovies
                             orderby m.Rating descending
                                          select m).ToList();

            Console.Write("\nList of Movies that have a rating below " + maxRating + ":");
            Console.Write("\nTitle");
            Console.SetCursorPosition(margin, Console.GetCursorPosition().Top);
            Console.Write("   Rating");
            foreach (Movie filteredMovie in filteredMovies)
            {
                Console.WriteLine("\n");
                Console.Write(filteredMovie.Title);
                Console.SetCursorPosition(margin, Console.GetCursorPosition().Top);
                Console.Write("   " + filteredMovie.Rating);
            }
            Console.WriteLine();
        }

        static void WordsStartingWithA(List<string> wordList)
        {

            List<string> wordsStartingWithA = wordList.Where(w => w.StartsWith("A") || w.StartsWith("a")).ToList();

            Console.WriteLine("\nlist of A words :");
            foreach (string AWord in wordsStartingWithA)
            {
                Console.WriteLine(AWord);
            }
        }

        static void wordsWithNoEs(List<string> wordList)
        {

            List<string> wordsWithNoEs = wordList.Where(w => !w.Contains("E") && !w.Contains("É") && !w.Contains("e") && !w.Contains("é") && !w.Contains("è") && !w.Contains("ê")).ToList();

            Console.WriteLine("\nlist of words with no x:");

            foreach (string noEWord in wordsWithNoEs)
            {
                Console.WriteLine(noEWord);
            }
        }

        static void wordsWithEightOrMoreChar(List<string> wordList)
        {

            List<string> wordsWithEightOrMoreChar = wordList.Where(w => w.Length >= 8).ToList();

            Console.WriteLine("\nlist of words with 8 or more characters:");

            foreach (string EightOrMoreCharWord in wordsWithEightOrMoreChar)
            {
                Console.WriteLine(EightOrMoreCharWord);
            }
        }

        static void evenNumbers(List<int> numbersList)
        {
            
            List<int> evenNumbers = numbersList.Where(n => n % 2 == 0).ToList();


            Console.WriteLine("\nlist of even numbers :");
            foreach (int even in evenNumbers)
            {
                Console.WriteLine(even);
            }
        }

        static void averageLengthWord(List<string> wordList)
        {
            int average = GetAverage(wordList);
            List<string> averageStrings = wordList.Where(w => w.Length == average).ToList();

            Console.WriteLine("\nlist of words that have the average length:");

            foreach (string averageLengthWord in averageStrings)
            {
                Console.WriteLine(averageLengthWord);
            }
        }

        static int GetAverage(List<string> list)
        {
            List<int> listCount = list.Select(w => w.Length).ToList();

            double doubleAverage = listCount.Average();

            int intAverage = Convert.ToInt16(listCount.Average());
            Console.WriteLine("\nInt average: " + intAverage + " | " + " Double average " + doubleAverage);
            return intAverage;
        }
    }
}

