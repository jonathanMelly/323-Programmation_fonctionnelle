using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex_immutable_nima_zarrabi
{
    public class Player
    {
        private readonly string _name;
        private readonly int _age;

        public Player(string name, int age)
        {
            _name = name;
            _age = age;
            if (Program.elder != null)
            {
                if (age >= Program.elder.Age)
                {
                    Program.elder = this;
                }
            }
            else
            {
                Program.elder = this;
            }

        }

        public string Name => _name;

        public int Age => _age;
    }
}
