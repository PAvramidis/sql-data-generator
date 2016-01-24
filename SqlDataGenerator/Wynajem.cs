using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator
{
    class Wynajem : Encja
    {
        private string Id;
        private string Data;
        private string Godzina;
        private string CenaNajmu;
        private string PeselNajemcy;
        private string FormaPlatnosci;
        private string FormaRezerwacji;
        private string FkAdresUlicaStart;
        private string FkAdresKodPocztowyStart;
        private string FkAdresUlicaStop;
        private string FkAdresKodPocztowyStop;
        private string FkOddzialNumerOddzialu;
        private int LiczbaWynajec;
        private int CzasNajmuNumeric;
        public Wynajem(int n)
        {
            LiczbaWynajec = n;
        }

        public override void Randomize()
        {
            System.IO.StreamWriter sw;
            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
                sw = new System.IO.StreamWriter(Program.Path + "Wynajem.sql");
            else
                sw = new System.IO.StreamWriter(Program.Path + "Wynajem2.sql");

            Random rnd = new Random();

            string line = String.Empty;

            for (int i = 0; i < LiczbaWynajec; i++)
            {
                System.Console.Write("\r{0}%", ((i + 1) * 100) / LiczbaWynajec);
                Data = String.Empty;

                int im = rnd.Next(1990, 2016);

                Data += im.ToString();
                Data += '-';

                im = rnd.Next(1, 13);

                string a = im.ToString();

                if(a.Length == 1)
                {
                    a = a.Insert(0, "0");
                }

                Data += a;
                Data += '-';

                if(im == 1 || im == 3 || i == 5 || i == 7 || i == 8 || i == 10 || i == 12)
                {
                    im = rnd.Next(1, 29);
                }
                else if(im != 2)
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

                Godzina = String.Empty;

                im = rnd.Next(0, 24);

                a = im.ToString();

                if (a.Length == 1)
                {
                    a = a.Insert(0, "0");
                }

                Godzina += a;
                Godzina += ':';

                im = rnd.Next(0, 60);

                a = im.ToString();

                if (a.Length == 1)
                {
                    a = a.Insert(0, "0");
                }

                Godzina += a;
                Godzina += ':';


                Godzina += "00";          

                PeselNajemcy = String.Empty;
                im = rnd.Next(50, 98);
                PeselNajemcy += im.ToString();

                im = rnd.Next(1, 13);

                a = im.ToString();

                if (a.Length == 1)
                {
                    a = a.Insert(0, "0");
                }

                PeselNajemcy += a;

                if (im == 1 || im == 3 || i == 5 || i == 7 || i == 8 || i == 10 || i == 12)
                {
                    im = rnd.Next(1, 32);
                }
                else if (im != 2)
                {
                    im = rnd.Next(1, 31);
                }
                else
                {
                    im = rnd.Next(1, 29);
                }

                a = im.ToString();

                if (a.Length == 1)
                {
                    a = a.Insert(0, "0");
                }

                PeselNajemcy += a;

                int cyfra;
                for(int j = 0; j < 5; j++)
                {
                    cyfra = rnd.Next(0, 10);
                    PeselNajemcy += cyfra.ToString();
                }
                if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
                {
                    Program.update_value(ref Program.shKlienci, Program.IndexPesel, Program.PESEL, PeselNajemcy);
                }
                Program.update_value(ref Program.shKlienci2, Program.IndexPesel++, Program.PESEL, PeselNajemcy);
                im = rnd.Next(0, Program.LiczbaAdresow);

                FkAdresUlicaStart = Program.Adresy[im].Ulica;
                FkAdresKodPocztowyStart = Program.Adresy[im].KodPocztowy;

                int ip;
                do
                {
                    ip = rnd.Next(0, Program.LiczbaAdresow);
                }
                while (ip == im);

                FkAdresUlicaStop = Program.Adresy[ip].Ulica;
                FkAdresKodPocztowyStop = Program.Adresy[ip].KodPocztowy;

                FkOddzialNumerOddzialu = rnd.Next(1, Program.LiczbaOddzialow + 1).ToString();

                int temp = rnd.Next(1, 4);
                FormaPlatnosci = temp == 1 ? "czek" : temp == 2 ? "karta" : "gotówka";

                temp = rnd.Next(1, 4);
                FormaRezerwacji = temp == 1 ? "telefoniczna" : temp == 2 ? "internetowa" : "ustna";

                sw.WriteLine("insert into Wynajem values('" + Data + "', '" + Godzina + "' ," + "'" + PeselNajemcy + "'" + "," + "'" + FormaPlatnosci + "'" + "," + "'" + FormaRezerwacji + "'" + "," +
                    "'" + FkAdresUlicaStart + "'" + "," + "'" + FkAdresKodPocztowyStart + "'" + "," + "'" + FkAdresUlicaStop + "'" + "," + "'" + FkAdresKodPocztowyStop + "'" + "," + FkOddzialNumerOddzialu + ")");

                sw.Flush();
            }

            sw.Close();
        }

        public override void Create(System.IO.StreamWriter f)
        {
            f.WriteLine("CREATE TABLE Wynajem");
            f.WriteLine("(");

            f.WriteLine("\tId INTEGER IDENTITY(1,1) PRIMARY KEY,");
            f.WriteLine("\tData date,");
            f.WriteLine("\tGodzina time,");
            f.WriteLine("\tPeselNajemcy varchar(11),");
            f.WriteLine("\tFormaPlatnosci varchar(15),");
            f.WriteLine("\tFormaRezerwacji varchar(20),");
            f.WriteLine("\tFkAdresUlicaStart varchar(60),");
            f.WriteLine("\tFkAdresKodPocztowyStart varchar(8),");
            f.WriteLine("\tFkAdresUlicaStop varchar(60),");
            f.WriteLine("\tFkAdresKodPocztowyStop varchar(8),");
            f.WriteLine("\tFkOddzialNrOddzialu INTEGER REFERENCES Oddzial,");
            f.WriteLine("\tFOREIGN KEY(FkAdresUlicaStart, FkAdresKodPocztowyStart) REFERENCES Adres,");
            f.WriteLine("\tFOREIGN KEY(FkAdresUlicaStop, FkAdresKodPocztowyStop) REFERENCES Adres");

            f.WriteLine(")");
        }

        public override void Insert(StreamWriter file)
        {
            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
            {
                string[] p = System.IO.File.ReadAllLines(Program.Path + "Wynajem.sql");

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
                string[] p = System.IO.File.ReadAllLines(Program.Path + "Wynajem2.sql");

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
