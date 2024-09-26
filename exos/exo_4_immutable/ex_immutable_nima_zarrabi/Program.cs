using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ex_immutable_nima_zarrabi;

namespace ex_immutable_nima_zarrabi
{
    class Program
    {
        internal static Player? elder;

        static void Main(string[] args)
        {
            // 4 players
            List<Player> players = new List<Player>()
            {
                new Player("Joe", 32),
                new Player("Jack", 30),
                new Player("William", 37),
                new Player("Averell", 25),
                new Player("Nimskow", 99)
            };

            Console.WriteLine($"Le plus agé est {elder.Name} qui a {elder.Age} ans");

            Console.ReadKey();
        }
    }
}