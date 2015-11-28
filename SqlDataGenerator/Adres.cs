using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator
{
    class Adres : Encja
    {
        private string Ulica;
        private string Miasto;
        private string KodPocztowy;
        private int LiczbaAdresow;
        private int WielkoscMiasta;

        public Adres(int n)
        {
            LiczbaAdresow = n;
        }

        public override void Create(System.IO.StreamWriter f)
        {
            f.WriteLine("CREATE TABLE Adres");
            f.WriteLine("(");

            f.WriteLine("\tUlica varchar(60),");
            f.WriteLine("\tMiasto varchar(60),");
            f.WriteLine("\tKodPocztowy varchar(8),");
            f.WriteLine("\tWielkoscMiasta int,");
            f.WriteLine("\tPRIMARY KEY (Ulica, KodPocztowy)");

            f.WriteLine(")");
        }

        public override void Randomize()
        {
            System.IO.StreamWriter sw;
            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
                sw = new System.IO.StreamWriter(Program.Path + "Adres.sql");
            else
                sw = new System.IO.StreamWriter(Program.Path + "Adres2.sql");

            string[] miasta = System.IO.File.ReadAllLines(Program.Path + "Miasta.txt");
            string[] ulice = System.IO.File.ReadAllLines(Program.Path + "Ulice.txt");

            string[] numery = new string[LiczbaAdresow];


            Random rnd = new Random();

            string line = String.Empty;

            string n = miasta[0];
            int LiczbaMiast = Int32.Parse(n);

            n = ulice[0];
            int LiczbaUlic = Int32.Parse(n);

            for (int i = 0; i < LiczbaAdresow; i++)
            {
                int im = rnd.Next(1, LiczbaMiast + 1);

                Miasto = miasta[im];

                int naz = rnd.Next(1, LiczbaUlic + 1);

                Ulica = ulice[naz];

                int NrDomu = rnd.Next(1, 201);
                int NrMieszkania = rnd.Next(0, 401);

                Ulica += " ";
                Ulica += NrDomu.ToString();

                if(NrMieszkania != 0)
                {
                    Ulica += "/";
                    Ulica += NrMieszkania.ToString();
                }

                int cyfra = 0;
                for (int j = 0; j < 5; j++)
                {
                    cyfra = rnd.Next(0, 10);
                    KodPocztowy += cyfra.ToString();
                }

                KodPocztowy = KodPocztowy.Insert(2, "-");
                WielkoscMiasta = rnd.Next(5000, 1000001);

                    sw.WriteLine("insert into Adres values('" + Ulica + "'" + "," + "'" + Miasto + "'" + "," + "'" + KodPocztowy + "'" + "," + WielkoscMiasta + ")");

                if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
                    Program.Adresy[i] = new Program.AdresPojedynczy(Ulica, Miasto, KodPocztowy);
                else
                    Program.Adresy2[i] = new Program.AdresPojedynczy(Ulica, Miasto, KodPocztowy);

                KodPocztowy = String.Empty;
                sw.Flush();
            }

            sw.Close();
        }

        public override void Insert(StreamWriter file)
        {
            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
            {
                string[] p = System.IO.File.ReadAllLines(Program.Path + "Adres.sql");

                foreach (string line in p)
                {
                    file.WriteLine(line);
                }
                file.WriteLine();
            }
            else
            {
                string[] p = System.IO.File.ReadAllLines(Program.Path + "Adres2.sql");

                foreach (string line in p)
                {
                    file.WriteLine(line);
                }
                file.WriteLine();
            }
            file.WriteLine();
        }
    }
}
