using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ex_immutable_nima_zarrabi;

namespace ex_immutable_nima_zarrabi
{
    class Program
    {
        static void Main(string[] args)
        {
            // 4 players
            var players = ImmutableList.Create(
                new Player("Joe", 32),
                new Player("Jack", 30),
                new Player("William", 37),
                new Player("Averell", 25));

            players = players.Add(new Player("Nimskow", 99));

            getElder(players);
            Console.WriteLine($"Le plus agé est {getElder(players).Name} qui a {getElder(players).Age} ans");

            Console.ReadKey();
        }

        static Player getElder(IEnumerable<Player> players)
        {
            Player elder = players.First();
            foreach(var p in players)
            {
                if (p.Age >= elder.Age)
                {
                    elder = p;
                }
            }
            return elder;
}
    }
}