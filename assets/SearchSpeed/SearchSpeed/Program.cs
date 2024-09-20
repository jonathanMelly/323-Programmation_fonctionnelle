/// Test de performance de recherche dans une liste ou dans un dictionnaire
/// On a 10000 personnes, on choisit 200000 fois un vainqueur aléatoirement dans cette liste
/// Qu'est-ce qui est le plus efficace ? .Where ou le dictionnaire ?
/// 
/// X. Carrel 


using System.Diagnostics;

Random rand = new Random();
Stopwatch sw = Stopwatch.StartNew();

List<Person> people = new List<Person>();
Dictionary<int, Person> dico = new Dictionary<int, Person>();

// Build list of people
sw.Start();
for (int i = 1; i <= 10000; i++)
{
    Person bob = new Person(i, $"Bob{i}", rand.Next(10, 60), rand.Next(0, 5), rand.Next(0, 5));
    people.Add(bob);
}
sw.Stop();
Console.WriteLine($"Création de la liste: {sw.ElapsedMilliseconds} ms");

// Build dictionnary
sw.Reset();
sw.Start();
foreach (Person person in people) dico.Add((int)person.Id, person);
sw.Stop();
Console.WriteLine($"Création du dictionnaire: {sw.ElapsedMilliseconds} ms");



// find by where
sw.Reset();
sw.Start();
for (int i = 0; i < 200000; i++)
{
    int winner = rand.Next(1, people.Count);
    Person toto = people.Where(p => p.Id == winner).First();
}
sw.Stop();
Console.WriteLine($"Where: {sw.ElapsedMilliseconds} ms");


// find by dictionnary
sw.Reset();
sw.Start();
for (int i = 0; i < 200000; i++)
{
    int winner = rand.Next(1, people.Count);
    Person toto = dico[winner];
}
sw.Stop();
Console.WriteLine($"Dico: {sw.ElapsedMilliseconds} ms");

Console.ReadLine();

class Person
{
    public int Id;

    public Person(int id, string name, int age, int sisters, int brothers)
    {
        Id = id;
        Name = name;
        Age = age;
        Sisters = sisters;
        Brothers = brothers;
    }

    public string Name { get; set; }
    public int Age { get; set; }
    public int Sisters { get; set; }
    public int Brothers { get; set; }
}