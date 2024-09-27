using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex_place_du_marche_nima_zarrabi
{
    public class Producer
    {
        public string Emplacement;
        public string Producteur;
        public List<Produits>? Produits;

        public Producer(string emp, string produc, List<Produits>? produits)
        {

            this.Emplacement = emp;
            this.Producteur = produc;
            this.Produits = produits;
        }
    }
}
