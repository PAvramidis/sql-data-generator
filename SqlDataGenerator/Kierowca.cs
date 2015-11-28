using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator
{
    class Kierowca : Encja
    {
        private string Imie;
        private string Nazwisko;
        private string NrPracownika;
        private string Etat;
        private string Data;
        private string NumerPrawaJazdy;
        private string FkNumerOddzialu;

        private int LiczbaKierowcow;

        public const int NUMERIC = 0;
        public const int LITERAL = 1;
        public Kierowca(int n)
        {
            LiczbaKierowcow = n;
        }
        public override void Randomize()
        {
            System.IO.StreamWriter sw;
            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
                sw = new System.IO.StreamWriter(Program.Path + "Kierowca.sql");
            else
                sw = new System.IO.StreamWriter(Program.Path + "Kierowca2.sql");

            string[] imiona = System.IO.File.ReadAllLines(Program.Path + "Imiona.txt");
            string[] nazwiska = System.IO.File.ReadAllLines(Program.Path + "Nazwiska.txt");

            string[] numery = new string[LiczbaKierowcow];

            Random rnd = new Random();

            string line = String.Empty;

            string n = imiona[0];
            int LiczbaImion = Int32.Parse(n);

            n = nazwiska[0];
            int LiczbaNazwisk = Int32.Parse(n);

            for (int i = 1; i <= LiczbaKierowcow; i++)
            {
                int im = rnd.Next(1, LiczbaImion + 1);

                Imie = imiona[im];

                int naz = rnd.Next(1, LiczbaNazwisk + 1);

                Nazwisko = nazwiska[naz];

                NrPracownika = i.ToString();

                Data = String.Empty;

                im = rnd.Next(1950, 1990);

                Data += im.ToString();
                Data += '-';

                im = rnd.Next(1, 13);

                string a = im.ToString();

                if (a.Length == 1)
                {
                    a = a.Insert(0, "0");
                }

                Data += a;
                Data += '-';

                if (im == 1 || im == 3 || i == 5 || i == 7 || i == 8 || i == 10 || i == 12)
                {
                    im = rnd.Next(1, 29);
                }
                else if (im != 2)
                {
                    im = rnd.Next(1, 29);
                }
                else
                {
                    im = rnd.Next(1, 28);
                }

                a = im.ToString();

                if (a.Length == 1)
                {
                    a = a.Insert(0, "0");
                }

                Data += a;

                int temp = rnd.Next(0, 5);
                switch(temp)
                {
                    case 0:
                        Etat = "1/3";
                        break;
                    case 1:
                        Etat = "1/2";
                        break;
                    case 2:
                        Etat = "3/4";
                        break;
                    case 3:
                        Etat = "1";
                        break;
                    default:
                        //chuj
                        break;
                }
                NumerPrawaJazdy = String.Empty;
                NumerPrawaJazdy += RandomString(4, LITERAL);
                NumerPrawaJazdy += RandomString(5, NUMERIC);
                FkNumerOddzialu = rnd.Next(1, Program.LiczbaOddzialow).ToString();

                sw.WriteLine("insert into Kierowca values('" + Imie + "'" + ", " + "'" + Nazwisko + "'" + "," + "'" + Data + "'" + "," + "'" + Etat + "'" + "," + "'" + NumerPrawaJazdy + "',"  + FkNumerOddzialu  + ")");
                sw.Flush();
            }

            sw.Close();
        }

        public override void Create(System.IO.StreamWriter f)
        {
            f.WriteLine("CREATE TABLE Kierowca");
            f.WriteLine("(");

            f.WriteLine("\tNrPracownika INTEGER IDENTITY(1,1) PRIMARY KEY,");
            f.WriteLine("\tImie varchar(20),");
            f.WriteLine("\tNazwisko varchar(20),");
            f.WriteLine("\tData date,");
            f.WriteLine("\tEtat varchar(3),");
            f.WriteLine("\tNumerPrawaJazdy varchar(9),");
            f.WriteLine("\tFkNumerOddzialu int REFERENCES Oddzial");

            f.WriteLine(")");
        }

        public override void Insert(StreamWriter file)
        {
            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
            {
                string[] p = System.IO.File.ReadAllLines(Program.Path + "Kierowca.sql");

                foreach (string line in p)
                {
                    file.WriteLine(line);
                }
                file.WriteLine();
            }
            else
            {
                string[] p = System.IO.File.ReadAllLines(Program.Path + "Kierowca2.sql");

                foreach (string line in p)
                {
                    file.WriteLine(line);
                }
                file.WriteLine();
            }
            file.WriteLine();
        }

        public static string RandomString(int length, int type)
        {
            const string charsLiteral = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string charsNumeric = "123456789";
            var random = new Random();
            if (type == LITERAL)
            {
                return new string(Enumerable.Repeat(charsLiteral, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            else if (type == NUMERIC)
            {
                return new string(Enumerable.Repeat(charsNumeric, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            else {
                return "";
            }
        }
    }
}
