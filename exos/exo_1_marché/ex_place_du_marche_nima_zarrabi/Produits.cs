using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex_place_du_marche_nima_zarrabi
{
    public class Produits
    {
        public string SoldBy;
        public string Produit;
        public int Quantité;
        public string Unité;
        public double Prix_par_unité;


        public Produits(string soldby, string produit, int quant, string unit, double ppu)
        {
            this.SoldBy = soldby;
            this.Produit = produit;
            this.Quantité = quant;
            this.Unité = unit;
            this.Prix_par_unité = ppu;
        }
    }
}
