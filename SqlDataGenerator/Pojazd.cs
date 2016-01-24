using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator
{
    class Pojazd : Encja
    {
        private string NrRejestracyjny;
        private string Typ;
        private string Model;
        private string StanTechniczny;
        private string FkKierowcaNrPracownika;
        private string FkOddzialNumerOddzialu;
        private int LiczbaPojazdow;


        public static int Cena;

        public Pojazd(int n)
        {
            LiczbaPojazdow = n;
        }

        public override void Randomize()
        {
            System.IO.StreamWriter sw;
            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
                sw = new System.IO.StreamWriter(Program.Path + "Pojazd.sql");
            else
                sw = new System.IO.StreamWriter(Program.Path + "Pojazd2.sql");

            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
            {
                Program.rejestracje = System.IO.File.ReadAllLines(Program.Path + "Pojazdy.txt");

                Program.takenRejestracje = new bool[Program.rejestracje.Length];
            }

            string[] modele = System.IO.File.ReadAllLines(Program.Path + "Modele.txt");

            Random rnd = new Random();

            int LiczbaRejestracji = Int32.Parse(Program.rejestracje[0]);
            int LiczbaModeli = Int32.Parse(modele[0]);
            int im;

            if (Program.JedenPlikWynikowy == false)
            {
                sw.WriteLine("USE TransakcjeWynajmu");
                sw.WriteLine("GO");
                sw.WriteLine();

                sw.WriteLine("SET ANSI_WARNINGS  OFF;");
            }

            for (int i = 0; i < LiczbaPojazdow; i++)
            {
                System.Console.Write("\r{0}%", ((i + 1) * 100) / LiczbaPojazdow);
                do
                {
                    im = rnd.Next(1, LiczbaRejestracji + 1);
                }
                while (Program.takenRejestracje[im] == true);

                Program.takenRejestracje[im] = true;

                NrRejestracyjny = Program.rejestracje[im];

                if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
                {
                    Program.update_value(ref Program.shKupno, Program.IndexNrRej, Program.REG_NUM, NrRejestracyjny);
                }
                Program.update_value(ref Program.shKupno2, Program.IndexNrRej++, Program.REG_NUM, NrRejestracyjny);

                int t = rnd.Next(1, 4);

                switch(t)
                {
                    case 1:
                        Typ = "osobowy";
                        Cena = 80;
                        break;
                    case 2:
                        Typ = "ciezarowy";
                        Cena = 200;
                        break;
                    case 3:
                        Typ = "ekskluzywny";
                        Cena = 180;
                        break;
                }

                t = rnd.Next(1, 11);

                if(t <= 5)
                {
                    StanTechniczny = "dobry";
                    Cena *= 4;
                }
                else if(t <= 9)
                {
                    Cena *= 2;
                    StanTechniczny = "sredni";
                }
                else
                {
                    StanTechniczny = "zly";
                }

                im = rnd.Next(1, LiczbaModeli + 1);

                Model = modele[im];

                FkOddzialNumerOddzialu = rnd.Next(1, Program.LiczbaOddzialow + 1).ToString();
                int nr;

                im = rnd.Next(0, 3);

                if (i%3 == 0 || i%3 == 1)
                {
                    FkKierowcaNrPracownika = "";

                    sw.WriteLine("insert into Pojazd values('" + NrRejestracyjny + "'" + "," + "'" + Typ + "'" + "," + "'" + Model + "'" + "," + "'" + StanTechniczny + "'" + ","
                    + FkOddzialNumerOddzialu + ");");
                }
                else
                {
                    do
                    {
                        nr = rnd.Next(1, Program.LiczbaKierowcow + 1);
                    }
                    while (Program.zajeciKierowcy[nr - 1] == true);

                    Program.zajeciKierowcy[nr - 1] = true;
                    FkKierowcaNrPracownika = nr.ToString();

                    sw.WriteLine("insert into Pojazd values('" + NrRejestracyjny + "'" + "," + "'" + Typ + "'" + "," + "'" + Model + "'" + "," + "'" + StanTechniczny + "'" + ","
                     + FkOddzialNumerOddzialu + ");");
                }

                

                if(Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
                    Program.Pojazdy[i] = new Program.PojazdPojedynczy(NrRejestracyjny, StanTechniczny, Cena);
                else
                    Program.Pojazdy2[i] = new Program.PojazdPojedynczy(NrRejestracyjny, StanTechniczny, Cena);

                sw.Flush();
            }

            if (Program.JedenPlikWynikowy == false)
            {
                sw.WriteLine("SET ANSI_WARNINGS  OFF;");
            }
            sw.Close();
        }

        public override void Create(System.IO.StreamWriter f)
        {
            f.WriteLine("CREATE TABLE Pojazd");
            f.WriteLine("(");

            f.WriteLine("\tNrRejestracyjny varchar(12) PRIMARY KEY,");
            f.WriteLine("\tTyp varchar(25) CHECK (Typ='osobowy' or Typ='ciezarowy' or Typ='ekskluzywny'),");
            f.WriteLine("\tModel varchar(35),");
            f.WriteLine("\tStanTechniczny varchar(10) CHECK (StanTechniczny='dobry' or StanTechniczny='zly' or StanTechniczny='sredni'),");
            f.WriteLine("\tFkOddzialNrOddzialu INTEGER REFERENCES Oddzial,");

            f.WriteLine(")");
        }

        public override void Insert(StreamWriter file)
        {
            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
            {
                string[] p = System.IO.File.ReadAllLines(Program.Path + "Pojazd.sql");
                int u = 1;

                foreach (string line in p)
                {
                    System.Console.Write("\r{0}%", (u * 100) / p.Length);
                    u++;
                    file.WriteLine(line);
                }
                file.WriteLine();
            }
            else
            {
                string[] p = System.IO.File.ReadAllLines(Program.Path + "Pojazd2.sql");

                foreach (string line in p)
                {
                    file.WriteLine(line);
                }
                file.WriteLine();
            }
        }

        public override void Update(System.IO.StreamWriter file)
        {
            Random rnd = new Random();
            for (int i = 0; i < 25; i++)
            {
                int index;
                do
                {
                    index = rnd.Next(0, Program.LiczbaPojazdow);
                }
                while (Program.Pojazdy[index].Stan == "dobry");


                file.WriteLine("update Pojazd set \"StanTechniczny\" = 'dobry' where \"NrRejestracyjny\" = '{0}';",
                    Program.Pojazdy[index].NrRejestracyjny);
            }
        }
    }
}
