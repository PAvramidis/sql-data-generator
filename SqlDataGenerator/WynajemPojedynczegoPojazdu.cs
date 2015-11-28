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

            int a, b;

            for (int i = 0; i < LiczbaWynajecPojedynczegoPojazdu; i++)
            {
                do
                {

                    a = rnd.Next(1, Program.LiczbaWynajec + 1);
                    b = rnd.Next(0, Program.LiczbaPojazdow);
                }
                while (Program.zajete[b, a-1] == true);


                int CzasNajmuNumeric = rnd.Next(1, 169);
                CzasNajmu = CzasNajmuNumeric.ToString();

                int CenaInt = CzasNajmuNumeric * Program.Pojazdy[b].Cena;
                Cena = CenaInt.ToString();
                FkWynajemId = a.ToString();
                FkPojazdNrRejestracyjny = Program.Pojazdy[b].NrRejestracyjny.ToString();
                Program.zajete[b, a - 1] = true;


                do
                {
                    b = rnd.Next(0, Program.LiczbaKierowcow);
                }
                while (Program.zajKierowcy[b, a - 1] == true);
                if (b == 0)
                {
                    
                    FkKierowcaId = "NULL";
                    sw.WriteLine("insert into WynajemPojedynczegoPojazdu values(" + FkWynajemId + ", '" + FkPojazdNrRejestracyjny + "'," + FkKierowcaId + "," + Cena + "," + CzasNajmu + ")");
                }
                else
                {
                    FkKierowcaId = (b + 1).ToString();
                    Program.zajKierowcy[b, a - 1] = true;
                    sw.WriteLine("insert into WynajemPojedynczegoPojazdu values(" + FkWynajemId + ", '" + FkPojazdNrRejestracyjny + "',"  + FkKierowcaId + "," + Cena + "," + CzasNajmu + ")");
                }
                    
                

                sw.Flush();
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
            f.WriteLine("\tPRIMARY KEY (FkWynajemId, FkPojazdNrRejestracyjny)");

            f.WriteLine(")");
        }

        public override void Insert(StreamWriter file)
        {
            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
            {
                string[] p = System.IO.File.ReadAllLines(Program.Path + "WynajemPojedynczegoPojazdu.sql");

                foreach (string line in p)
                {
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
