//Auteur : JMY
//Date   : 03.9.2024 
//Lieu   : ETML
//Descr. : programmation fonctionnelle

Console.WriteLine("+-------------------+");
Console.WriteLine("|JOUER AVEC LES MOTS|");
Console.WriteLine("+-------------------+");

string[] words = { "aba","bonjour", "hello", "monde", "vert", "rouge", "bleu", "jaune" };

//Version 1
words.Where(w => !w.Contains('x')).Print("ne contiennent pas la lettre x");
words.Where(w => w.Count()>=4).Print("ont 4 caractères ou plus");
words.Where(w => w.Count()==Math.Round(words.Average(w=>w.Length),0)).Print("ont autant de caractères que la moyenne du nombre de caractères de la liste");

//Version 2
//source de stat: https://www.apprendre-en-ligne.net/crypto/stat/francais.html
var stats = new List<double>() {8.15, 0.97, 3.15, 3.73, 17.39, 1.12, 0.97,0.85, 7.31, 0.45, 0.02, 5.69, 2.87, 7.12, 5.28, 2.8, 1.21, 6.64, 8.14, 7.22, 6.38, 1.64,0.03, 0.41, 0.28, 0.15 };

words.Select(
    //Transformation sous forme de tuple pour l’affichage à la fin
    word => Tuple.Create(word, 
        word
            //Calcul de Epsilon
            .GroupBy(character => character)
            .Sum(group => stats[(int)group.Key - 97] / 100 / group.Count()))
    )
    //Filter
    .Where(tuple => tuple.Item2 >= 0.0 && tuple.Item2 <= 10.95)
    
    //Print
    .ToList().ForEach(tuple => Console.WriteLine($"Epsilon de {tuple.Item1}: {tuple.Item2}")
);

/*On verra ça plut tard...*/
static class MagicExtension
{

    public static void Print<T> (this IEnumerable<T> collection, string prompt)
    {
        string toPrint = $"{prompt}: ";
        if (collection == null || collection.Count()==0)
        {
            toPrint +="[EMPTY]";
        }
        else
        {
            toPrint +=  string.Join(",", collection.ToArray<T>());
            toPrint += $" [{collection.Count()}]";
        }

        Console.WriteLine(toPrint);
    }
}