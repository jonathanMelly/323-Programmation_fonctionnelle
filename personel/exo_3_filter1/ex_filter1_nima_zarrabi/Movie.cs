using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ex_filter1_nima_zarrabi
{
    internal class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public double Rating { get; set; }
        public int Year { get; set; }
        public string[] LanguageOptions { get; set; }
        public string[] StreamingPlatforms { get; set; }


        internal Movie(int id)
        {
            this.Id = id;
        }
    }
}
