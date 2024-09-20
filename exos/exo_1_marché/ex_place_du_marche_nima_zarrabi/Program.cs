// See https://aka.ms/new-console-template for more information
using System.Text;
using ex_place_du_marche_nima_zarrabi;
using OfficeOpenXml;


//using OfficeOpenXml;
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
string current_emp;
string current_produc;
string current_produit;
int current_quant;
string current_unit;
double current_ppu;

List<Producer> producerList = new();
List<Produits> sellerProduits = new();

using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(@"C:\Users\pu78ifh\Desktop\323-Programmation_fonctionnelle_nima\exos\marché\Place du marché.xlsx")))
{
    for (int i = 1; i < 2; i++)
    {
        var myWorksheet = xlPackage.Workbook.Worksheets[i]; //select sheet here
        var totalRows = myWorksheet.Dimension.End.Row;
        var totalColumns = myWorksheet.Dimension.End.Column;



        var sb = new StringBuilder(); //this is your data
        for (int rowNum = 2; rowNum <= totalRows; rowNum++) //start at 2 to skip first row
        {
            //Console.WriteLine(myWorksheet.Cells[rowNum, 1, rowNum, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString()));
            var emplacement = myWorksheet.Cells[2, 1, totalRows, 1].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
            var producteur = myWorksheet.Cells[2, 2, totalRows, 2].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
            var produit = myWorksheet.Cells[2, 3, totalRows, 3].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
            var quantite = myWorksheet.Cells[2, 4, totalRows, 4].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
            var unite = myWorksheet.Cells[2, 5, totalRows, 5].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
            var prix_par_unite = myWorksheet.Cells[2, 6, totalRows, 6].Select(c => c.Value == null ? string.Empty : c.Value.ToString());

            for (i = 0; i < totalRows; i++)
            {
                foreach (var currentproduit in produit)
                {
                    sellerProduits.Add(new Produits(producteur.ElementAt(i), produit.ElementAt(i), Convert.ToInt16(quantite.ElementAt(i)), unite.ElementAt(i), Convert.ToDouble(prix_par_unite.ElementAt(i))));
                }

                foreach (var producer in producteur)
                {
                    //if (emplacement.ElementAt(i) != producerList)
                    {
                        producerList.Add(new Producer(emplacement.ElementAt(i), producteur.ElementAt(i), null));
                    }
                }

                foreach (var producer in producerList)
                {
                    foreach (var produits in sellerProduits)
                    {
                        if (producer.Producteur == produits.SoldBy)
                        {
                            producer.Produits.Add(produits);
                        }
                    }
                }

                foreach (Producer currentproducer in producerList)
                {
                    Console.WriteLine(currentproducer.Emplacement);
                }

            }



            /*

            */
            //Console.WriteLine(sb.AppendLine(row));
            //sb.AppendLine(string.Join(",", row));
            //sb.AppendLine(string.Join(",", row));

            //var row = myWorksheet.Cells[rowNum, 1].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
            //sb.AppendLine(row.ToString());
            //Console.WriteLine(row.ToString());

            //var data = row.ToString().Split(",");

            //Console.WriteLine(data);
            /*
            for (int countrow = 0; countrow <= totalColumns; countrow++)
            {
                //new Produits
                for (int itemnum = 0; itemnum < 5; itemnum++)
                {
                    current_emp = data[itemnum];
                    current_produc = data[itemnum];
                    current_produit = data[itemnum];
                }
            }*/

            //Console.WriteLine(data);

        }


        //Console.WriteLine("Sheet " + Convert.ToString(i + 1));
        //Console.WriteLine();
        //Console.WriteLine(sb.ToString());


    }

}