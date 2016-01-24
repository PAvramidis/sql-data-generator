using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator
{
    class WynajemPojedycznegoPojazdu : Encja
    {
        private string FkWynajemId;
        private string FkPojazdNrRejestracyjny;
        private string FkKierowcaId;
        private string Cena;
        private string CzasNajmu;
        private int LiczbaWynajecPojedynczegoPojazdu;
        private string TypWynajmu;
        public WynajemPojedycznegoPojazdu(int n)
        {
            LiczbaWynajecPojedynczegoPojazdu = n;
        }
        public override void Randomize()
        {
            System.IO.StreamWriter sw;
            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
                sw = new System.IO.StreamWriter(Program.Path + "WynajemPojedynczegoPojazdu.sql");
            else
                sw = new System.IO.StreamWriter(Program.Path + "WynajemPojedynczegoPojazdu2.sql");
            Random rnd = new Random();

            int a, b, temp;

            if (Program.JedenPlikWynikowy == false)
            {
                sw.WriteLine("USE TransakcjeWynajmu");
                sw.WriteLine("GO");
                sw.WriteLine();

                sw.WriteLine("SET ANSI_WARNINGS  OFF;");
            }

            for (int i = 0; i < LiczbaWynajecPojedynczegoPojazdu; i++)
            {
                System.Console.Write("\r{0}%", ((i + 1) * 100) / LiczbaWynajecPojedynczegoPojazdu);
                do
                {

                    a = rnd.Next(1, Program.LiczbaWynajec + 1);
                    b = rnd.Next(0, Program.LiczbaPojazdow);
                }
                //while (SprawdzZajetosc(b, a-1) == true);
                while (Program.TablicaZajetosci[a - 1].Contains(b)) ;

                Program.TablicaZajetosci[a - 1].Add(b);

                int CzasNajmuNumeric = rnd.Next(1, 169);
                CzasNajmu = CzasNajmuNumeric.ToString();

                int CenaInt = CzasNajmuNumeric * Program.Pojazdy[b].Cena;
                Cena = CenaInt.ToString();
                FkWynajemId = a.ToString();
                FkPojazdNrRejestracyjny = Program.Pojazdy[b].NrRejestracyjny.ToString();
                // ZapiszZajetosc(b, a - 1);

                temp = rnd.Next(1, 4);
                switch (temp) 
                { 
                    case 1:
                        TypWynajmu = "Przewoz osob";
                        break;
                    case 2:
                        TypWynajmu = "Przewoz zwierzat";
                        break;
                    case 3:
                        TypWynajmu = "Przewoz towarow";
                        break;
                }

                do
                {
                    b = rnd.Next(0, Program.LiczbaKierowcow);
                }
                //while (SprawdzZajetoscKier(b, a - 1) == true || b == 0);
                while (false);
                if (b == 0)
                {
                    
                    FkKierowcaId = "NULL";
                    sw.WriteLine("insert into WynajemPojedynczegoPojazdu values(" + FkWynajemId + ", '" + FkPojazdNrRejestracyjny + "'," + FkKierowcaId + "," + Cena + "," + CzasNajmu + ",'" + TypWynajmu + "')");
                }
                else
                {
                    FkKierowcaId = (b + 1).ToString();
                    //ZapiszZajetoscKier(b, a - 1);
                    sw.WriteLine("insert into WynajemPojedynczegoPojazdu values(" + FkWynajemId + ", '" + FkPojazdNrRejestracyjny + "'," + FkKierowcaId + "," + Cena + "," + CzasNajmu + ",'" + TypWynajmu + "')");
                }
                    
                

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
            f.WriteLine("CREATE TABLE WynajemPojedynczegoPojazdu");
            f.WriteLine("(");

            f.WriteLine("\tFkWynajemId INTEGER REFERENCES Wynajem,");
            f.WriteLine("\tFkPojazdNrRejestracyjny varchar(12) REFERENCES Pojazd,");
            f.WriteLine("\tFkNumerKierowcy int REFERENCES Kierowca,");
            f.WriteLine("\tCena int,");
            f.WriteLine("\tCzasNajmuWGodzinach varchar(5),");
            f.WriteLine("\tTypWynajmu varchar(20),");
            f.WriteLine("\tPRIMARY KEY (FkWynajemId, FkPojazdNrRejestracyjny)");

            f.WriteLine(")");
        }

        public override void Insert(StreamWriter file)
        {
            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
            {
                string[] p = System.IO.File.ReadAllLines(Program.Path + "WynajemPojedynczegoPojazdu.sql");

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
                string[] p = System.IO.File.ReadAllLines(Program.Path + "WynajemPojedynczegoPojazdu2.sql");

                foreach (string line in p)
                {
                    file.WriteLine(line);
                }
                file.WriteLine();
            }
        }
    }
}
