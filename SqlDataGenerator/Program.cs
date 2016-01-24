using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

namespace SqlDataGenerator
{
    class Program
    {
        //generator excel
        public const int DATE = 2;
        public const int REG_NUM = 3;
        public const int COST = 4;

        public const int DRIVER_ID = 1;
        public const int CATEGORY = 2;

        public const int NUM_OF_RES = 2;
        public const int SURFACE = 3;
        public const int NUM_OF_FIRMS = 4;

        public const int NAME = 1;
        public const int SURNAME = 2;
        public static int PESEL = 3;
        public const int TOWN = 4;
        public const int POSTALCODE = 5;
        public const int ADDRESS = 6;
        public const int TELNO = 7;

        public static bool JedenPlikWynikowy = false;
        public static bool JedenPunktCzasowy = true;

        public static int IndexPesel = 2;
        public static int IndexNrRej = 2;

        public static string[] CategoryArray = { "A", "B", "C", "D", "A1", "AM", "B1", "C1", "D1", "A2", "T", "BE", "CE", "DE", "C1E", "D1E" };

        private static string excel_destination_path = @"D:\Generator\wygenerowane1.xlsx";
        private static string excel_destination_data_2_path = @"D:\Generator\dane2.xlsx";
        private static Excel.Application excel_destination = new Excel.Application();
        private static Excel.Workbook wb_dest = excel_destination.Workbooks.Open(excel_destination_path);
        private static Excel.Application excel_destination2 = new Excel.Application();
        private static Excel.Workbook wb_dest_data2 = excel_destination2.Workbooks.Open(excel_destination_data_2_path);

        public static Excel.Worksheet shMiasta = (Microsoft.Office.Interop.Excel.Worksheet)excel_destination2.Worksheets.get_Item("Miasta");
        public static Excel.Worksheet shKlienci = wb_dest.Sheets.Add();
        public static Excel.Worksheet shKlienci2 = wb_dest.Sheets.Add();
        public static Excel.Worksheet shKupno = wb_dest.Sheets.Add();
        public static Excel.Worksheet shKupno2 = wb_dest.Sheets.Add();
        public static Excel.Worksheet shKierowcy = wb_dest.Sheets.Add();
        public static Excel.Worksheet shKierowcy2 = wb_dest.Sheets.Add();
        //koniec generatora

        private const int LiczbaEncji = 6;
        private const string PathToCreateSql = "D:\\Generator\\CreateTables.sql";  
        private const string PathToInsertSql = "D:\\Generator\\Insert.sql";
        private const string PathToSqlT2 = "D:\\Generator\\T2.sql";
        public static string Path = "D:\\Generator\\";

        public static TimePoint PunktCzasowy = TimePoint.FirstTimePoint;
        private static Encja[] Encje;
        private static StreamWriter FileWriter;
        private static StreamWriter FileWriterInsert;
        private static StreamWriter FileWriterTwo;

        public static int LiczbaWynajec = 1000000;
        public static List<int>[] TablicaZajetosci;
        public static int LiczbaWynajecPojedynczegoPojazdu;
        public static int LiczbaAdresow;
        public static int LiczbaOddzialow;
        public static int LiczbaKierowcow;
        public static int LiczbaPojazdow;

        public static int LiczbaWynajecDodane;

        public static int LiczbaWynajecPojedynczegoPojazduDodane;
        public static int LiczbaAdresowDodane;
        public static int LiczbaOddzialowDodane;
        public static int LiczbaKierowcowDodane;
        public static int LiczbaPojazdowDodane;

        public static bool[] zajeteAdresy;
        public static bool[] zajeciKierowcy;
        public static bool[] takenRejestracje;
        public static string[] rejestracje;

        public static AdresPojedynczy[] Adresy;
        public static PojazdPojedynczy[] Pojazdy;

        public static AdresPojedynczy[] Adresy2;
        public static PojazdPojedynczy[] Pojazdy2;

        public class AdresPojedynczy
        {
            public string Ulica;
            public string Miasto;
            public string KodPocztowy;

            public AdresPojedynczy(string ul, string m, string kp)
            {
                Ulica = ul;
                Miasto = m;
                KodPocztowy = kp;
            }
        }

        public class PojazdPojedynczy
        {
            public string NrRejestracyjny;
            public string Stan;
            public int Cena;

            public PojazdPojedynczy(string nr, string s, int c)
            {
                NrRejestracyjny = nr;
                Stan = s;
                Cena = c;
            }
        }


        static void Main(string[] args)
        {
            for(int i = 0; i < args.Length; i++)
            {
                switch(args[i])
                {
                    case "-n":
                        i++;
                        int n = Int32.Parse(args[i]);
                        if (n < 0)
                        {
                            System.Console.WriteLine("Error parsing arguments!");
                            return;
                        }
                        else
                        {
                            LiczbaWynajec = n;
                        }
                        break;
                    default:
                        break;
                }
            }

            
            LiczbaWynajecPojedynczegoPojazdu = (LiczbaWynajec * 3) / 2;
            LiczbaAdresow = 2 * LiczbaWynajec;
            LiczbaOddzialow = LiczbaAdresow / 20;
            LiczbaKierowcow = 5 * LiczbaOddzialow;
            LiczbaPojazdow = 2 * LiczbaKierowcow;

            LiczbaWynajecDodane = LiczbaWynajec / 100;

            LiczbaWynajecPojedynczegoPojazduDodane = (LiczbaWynajecDodane * 3) / 2;
            LiczbaAdresowDodane = 2 * LiczbaWynajecDodane;
            LiczbaOddzialowDodane = LiczbaAdresowDodane / 20;
            LiczbaKierowcowDodane = 5 * LiczbaOddzialowDodane;
            LiczbaPojazdowDodane = 3 * LiczbaKierowcowDodane;

            shKlienci.Name = "KlienciT1";
            shKlienci2.Name = "KlienciT2";
            /*
            try
            {
                make_sheets();
            }
            catch (Exception ex)
            {
                excel_destination.Quit();
            }
            System.Console.WriteLine("Skonczyl sie excel");
            */

            Adresy = new AdresPojedynczy[LiczbaAdresow];
            Pojazdy = new PojazdPojedynczy[LiczbaPojazdow];

            zajeteAdresy = new bool[Program.LiczbaAdresow];
            zajeciKierowcy = new bool[Program.LiczbaKierowcow];



            Encje = new Encja[LiczbaEncji];

            Encje[2] = new Kierowca(LiczbaKierowcow);
            Encje[0] = new Adres(LiczbaAdresow);
            Encje[1] = new Oddzial(LiczbaOddzialow);
            Encje[3] = new Pojazd(LiczbaPojazdow);
            Encje[4] = new Wynajem(LiczbaWynajec);
            Encje[5] = new WynajemPojedycznegoPojazdu(LiczbaWynajecPojedynczegoPojazdu);

            TablicaZajetosci = new List<int>[LiczbaWynajec];

            for (int i = 0; i < TablicaZajetosci.Length; i++)
            {
                TablicaZajetosci[i] = new List<int>();
            }

            FileWriter = new StreamWriter(PathToCreateSql);
            FileWriter.AutoFlush = true;

            FileWriterInsert = new StreamWriter(PathToInsertSql);
            FileWriterInsert.AutoFlush = true;

            FileWriterTwo = new StreamWriter(PathToSqlT2);
            FileWriterTwo.AutoFlush = true;

            FileWriter.WriteLine("CREATE DATABASE TransakcjeWynajmu");
            FileWriter.WriteLine("GO");
            FileWriter.WriteLine();
            FileWriter.WriteLine("USE TransakcjeWynajmu");
            FileWriter.WriteLine("GO");
            FileWriter.WriteLine();

            int u = 0;

            foreach (Encja e in Encje)
            {
                e.Create(FileWriter);
                
                if (e != Encje.Last())
                {
                    FileWriter.WriteLine("GO");
                    FileWriter.WriteLine();
                }
                else
                {
                    FileWriter.Write("GO");
                } 
            }

            foreach (Encja e in Encje)
            {
                System.Console.WriteLine("Encja randomize {0}", u);
                u++;
                e.Randomize();
                System.Console.WriteLine();
            }
            if (JedenPlikWynikowy == true)
            {
                FileWriterInsert.WriteLine("USE TransakcjeWynajmu");
                FileWriterInsert.WriteLine("GO");
                FileWriterInsert.WriteLine();

                FileWriterInsert.WriteLine("SET ANSI_WARNINGS  OFF;");
                u = 0;
                foreach (Encja e in Encje)
                {
                    System.Console.WriteLine("Encja insert {0}", u);
                    u++;
                    e.Insert(FileWriterInsert);
                    System.Console.WriteLine();
                }

                FileWriterInsert.WriteLine("SET ANSI_WARNINGS  OFF;");
            }

            if (JedenPunktCzasowy == false)
            {

                PunktCzasowy = TimePoint.SecondTimePoint;

                Encja[] EncjeT2 = new Encja[LiczbaEncji];

                LiczbaKierowcow = LiczbaKierowcowDodane;
                zajeciKierowcy = new bool[LiczbaKierowcow];

                Adresy2 = new AdresPojedynczy[LiczbaAdresowDodane];
                Pojazdy2 = new PojazdPojedynczy[LiczbaPojazdowDodane];

                EncjeT2[2] = new Kierowca(LiczbaKierowcowDodane);
                EncjeT2[0] = new Adres(LiczbaAdresowDodane);
                EncjeT2[1] = new Oddzial(LiczbaOddzialowDodane);
                EncjeT2[3] = new Pojazd(LiczbaPojazdowDodane);
                EncjeT2[4] = new Wynajem(LiczbaWynajecDodane);
                EncjeT2[5] = new WynajemPojedycznegoPojazdu(LiczbaWynajecPojedynczegoPojazduDodane);
                foreach (Encja e in EncjeT2)
                {

                    e.Randomize();
                }

                FileWriterTwo.WriteLine("USE TransakcjeWynajmu");
                FileWriterTwo.WriteLine("GO");
                FileWriterTwo.WriteLine();

                foreach (Encja e in EncjeT2)
                {
                    e.Update(FileWriterTwo);
                }

                FileWriterTwo.WriteLine("GO");
                FileWriterTwo.WriteLine();

                foreach (Encja e in EncjeT2)
                {
                    e.Insert(FileWriterTwo);
                }
            }
            wb_dest.Save();
            excel_destination.Quit();
            wb_dest_data2.Save();
            excel_destination2.Quit();
        }

        public enum TimePoint
        {
            FirstTimePoint = 1,
            SecondTimePoint = 2
        }

        public static void make_sheets()
        {

            string excel_data_path = @"D:\Generator\dane.xlsx";

            excel_destination.Visible = false;

            System.Array arrayClients = null;
            System.Array arrayForms = null;
            System.Array arrayPurchases = null;
            System.Array arrayDrivers = null;
            make_arrays(ref arrayClients, excel_data_path, "Klienci", "A1", "G301");
            make_arrays(ref arrayForms, excel_data_path, "Ankiety", "A1", "B1");
            make_arrays(ref arrayPurchases, excel_data_path, "Kupno", "A1", "D1");
            make_arrays(ref arrayDrivers, excel_data_path, "Kierowcy", "A1", "B1");

            Excel.Worksheet sh3 = wb_dest.Sheets.Add();
            sh3.Name = "FormularzeT1";
            Excel.Worksheet sh4 = wb_dest.Sheets.Add();
            sh4.Name = "FormularzeT2";
            shKupno.Name = "KupnoT1";
            shKupno2.Name = "KupnoT2";
            shKierowcy.Name = "KierowcyT1";
            shKierowcy2.Name = "KierowcyT2";

            add_sheet(ref shKlienci, "Klienci", ref shKlienci2, arrayClients, LiczbaWynajec);
            add_sheet(ref sh3, "Formularze", ref sh4, arrayForms, LiczbaWynajec);
            add_sheet(ref shKupno, "Kupno", ref shKupno2, arrayPurchases, LiczbaPojazdow);
            add_sheet(ref shKierowcy, "Kierowcy", ref shKierowcy2, arrayDrivers, LiczbaKierowcow);
            fill_towns(ref shMiasta);

        }

        public static void make_arrays(ref System.Array a, string path, string sheet, string R1, string R2)
        {

            Excel.Application excel_data = new Excel.Application();
            excel_data.Visible = false;

            Excel.Workbook wb_data = excel_data.Workbooks.Open(path);
            Excel.Sheets sheets = excel_data.Worksheets;
            Excel.Worksheet worksheet = (Excel.Worksheet)sheets.get_Item(sheet);
            Excel.Range range = worksheet.get_Range(R1, R2.ToString());
            a = (System.Array)range.Cells.Value;
            excel_data.Quit();
        }

        public static void add_sheet(ref Excel.Worksheet sh1, string name, ref Excel.Worksheet sh2, System.Array a, int rang)
        {
            Random rnd = new Random();
            int rand_number;
            int rand_number2;
            int addition = (int)(rang / 100);
            StringBuilder telNo = new StringBuilder(10);
            DateTime start = new DateTime(1995, 1, 1);
            Random gen = new Random();
            int rangeDate = (DateTime.Today - start).Days;
            int range = rang;
            int range2 = range;
            string date = null;

            int number_of_columns = a.GetLength(1);
            int raw_number = a.GetLength(0);
            int j;

            for (int i = 1; i <= number_of_columns; i++)
            {
                sh1.Cells[1, i].Value2 = a.GetValue(1, i).ToString();
                sh2.Cells[1, i].Value2 = a.GetValue(1, i).ToString();
            }
            if (name == "Klienci" || name == "Formularze")
            {
                range = LiczbaWynajec;
                range2 = LiczbaWynajec + LiczbaWynajecDodane;
                for (int i = 0; i < range2; i++)
                {
                    if (name != "Klienci")
                    {
                        if (i < range)
                        {
                            sh1.Cells[i + 2, 1].Value2 = (i + 1).ToString();
                        }
                        sh2.Cells[i + 2, 1].Value2 = (i + 1).ToString();
                    }
                    if (name == "Klienci")
                    {
                        for (int k = 0; k < 10; k++)
                        {
                            rand_number = rnd.Next(0, 9);
                            telNo = telNo.Append(rand_number.ToString());
                        }
                        if (i < range)
                        {
                            sh1.Cells[i + 2, TELNO].Value2 = telNo.ToString();
                        }
                        sh2.Cells[i + 2, TELNO].Value2 = telNo.ToString();
                        telNo.Clear();

                        rand_number = rnd.Next(2, raw_number - 1);
                        if (i < range)
                        {
                            sh1.Cells[i + 2, NAME].Value2 = a.GetValue(rand_number, NAME).ToString();
                        }
                        sh2.Cells[i + 2, NAME].Value2 = a.GetValue(rand_number, NAME).ToString();

                        rand_number = rnd.Next(2, raw_number - 1);
                        if (i < range)
                        {
                            sh1.Cells[i + 2, SURNAME].Value2 = a.GetValue(rand_number, SURNAME).ToString();
                        }
                        sh2.Cells[i + 2, SURNAME].Value2 = a.GetValue(rand_number, SURNAME).ToString();

                        rand_number = rnd.Next(2, raw_number - 1);
                        if (i < range)
                        {
                            sh1.Cells[i + 2, TOWN].Value2 = a.GetValue(rand_number, TOWN).ToString();
                            sh1.Cells[i + 2, POSTALCODE].Value2 = a.GetValue(rand_number, POSTALCODE).ToString();
                        }
                        sh2.Cells[i + 2, TOWN].Value2 = a.GetValue(rand_number, TOWN).ToString();
                        sh2.Cells[i + 2, POSTALCODE].Value2 = a.GetValue(rand_number, POSTALCODE).ToString();

                        rand_number = rnd.Next(2, raw_number - 1);
                        if (i < range)
                        {
                            sh1.Cells[i + 2, ADDRESS].Value2 = a.GetValue(rand_number, ADDRESS).ToString();
                        }
                        sh2.Cells[i + 2, ADDRESS].Value2 = a.GetValue(rand_number, ADDRESS).ToString();
                    }
                    else if (name == "Formularze")
                    {
                        rand_number = rnd.Next(1, 10);
                        if (i < range)
                        {
                            sh1.Cells[i + 2, 2].Value2 = rand_number.ToString();
                        }
                        sh2.Cells[i + 2, 2].Value2 = rand_number.ToString();
                    }
                }
            }
            else if (name == "Kupno")
            {
                range = LiczbaPojazdow;
                range2 = LiczbaPojazdow + LiczbaPojazdowDodane;
                for (int i = 0; i < range2; i++)
                {
                    if (name != "Klienci")
                    {
                        if (i < range)
                        {
                            sh1.Cells[i + 2, 1].Value2 = (i + 1).ToString();
                        }
                        sh2.Cells[i + 2, 1].Value2 = (i + 1).ToString();
                    }
                    date = start.AddDays(gen.Next(rangeDate)).ToString();

                    if (i < range)
                    {
                        sh1.Cells[i + 2, DATE].Value2 = date.Remove(10, 9);
                    }
                    sh2.Cells[i + 2, DATE].Value2 = date.Remove(10, 9);

                    rand_number = rnd.Next(50000, 2000001);
                    if (i < range)
                    {
                        sh1.Cells[i + 2, COST].Value2 = rand_number.ToString();
                    }
                    sh2.Cells[i + 2, COST].Value2 = rand_number.ToString();

                }
            }
            else if (name == "Kierowcy")
            {
                bool[] takenCategories = new bool[16];
                range = LiczbaKierowcow;
                range2 = LiczbaKierowcow + LiczbaKierowcowDodane;
                int index = 0;
                int categories_number_one_driver;
                for (int i = 0; i < range2; i++)
                {
                    for (int k = 0; k < 16; k++)
                    {
                        takenCategories[k] = false;
                    }
                    categories_number_one_driver = rnd.Next(1, 5);
                    for (int k = 0; k < categories_number_one_driver; k++, index++)
                    {
                        if (name != "Klienci")
                        {
                            if (i < range)
                            {
                                sh1.Cells[index + 2, DRIVER_ID].Value2 = (i + 1).ToString();
                            }
                            sh2.Cells[index + 2, DRIVER_ID].Value2 = (i + 1).ToString();
                        }

                        do
                        {
                            rand_number = rnd.Next(0, 16);
                        } while (takenCategories[rand_number] != false);

                        if (i < range)
                        {
                            sh1.Cells[index + 2, CATEGORY].Value2 = CategoryArray[rand_number].ToString();
                        }
                        sh2.Cells[index + 2, CATEGORY].Value2 = CategoryArray[rand_number].ToString();
                        takenCategories[rand_number] = true;
                    }
                }
            }
            int max;
            int min;
            int col;
            //update 
            if (name == "Klienci" || name == "Formularze")
            {
                range = LiczbaWynajec;
                range2 = LiczbaWynajec + LiczbaWynajecDodane;
                for (int i = 0; i < LiczbaWynajecDodane; i++)
                {
                    max = 0;
                    min = 0;
                    col = 0;
                    if (name == "Klienci")
                    {
                        max = TELNO;
                        min = NAME;
                        rand_number = rnd.Next(min, max);
                        if (rand_number == NAME)
                        {
                            col = NAME;
                        }
                        else if (rand_number == SURNAME)
                        {
                            col = SURNAME;
                        }
                        else if (rand_number == ADDRESS)
                        {
                            col = ADDRESS;
                        }
                        else if (rand_number == TOWN)
                        {
                            rand_number = rnd.Next(2, raw_number - 1);
                            rand_number2 = rnd.Next(2, range - 1);
                            sh2.Cells[rand_number2, TOWN].Value2 = a.GetValue(rand_number, TOWN).ToString();
                            sh2.Cells[rand_number2, POSTALCODE].Value2 = a.GetValue(rand_number, POSTALCODE).ToString();
                            continue;
                        }
                        else if (rand_number == TELNO)
                        {
                            for (int k = 0; k < 10; k++)
                            {
                                rand_number = rnd.Next(0, 9);
                                telNo = telNo.Append(rand_number.ToString());
                            }
                            rand_number = rnd.Next(2, raw_number - 1);
                            rand_number2 = rnd.Next(2, range - 1);
                            sh2.Cells[rand_number2, TELNO].Value2 = telNo.ToString();
                            telNo.Clear();
                            continue;
                        }
                        else
                        {
                            i--;
                            continue;
                        }
                        rand_number = rnd.Next(2, raw_number - 1);
                        rand_number2 = rnd.Next(2, range - 1);
                        sh2.Cells[rand_number2, col].Value2 = a.GetValue(rand_number, col).ToString();
                    }
                    else if (name == "Formularze")
                    {
                        rand_number = rnd.Next(1, 10);
                        rand_number2 = rnd.Next(2, range - 1);
                        sh2.Cells[rand_number2, 2].Value2 = rand_number.ToString();
                    }
                }
            }
            else if (name == "Kupno")
            {
                range = LiczbaPojazdow;
                range2 = LiczbaPojazdow + LiczbaPojazdowDodane;
                for (int i = 0; i < LiczbaPojazdowDodane; i++)
                {
                    min = DATE;
                    max = COST;
                    rand_number = rnd.Next(min, max + 1);
                    date = start.AddDays(gen.Next(rangeDate)).ToString();
                    if (rand_number == DATE)
                    {
                        rand_number2 = rnd.Next(2, range - 1);
                        sh2.Cells[rand_number2, DATE].Value2 = date.Remove(10, 9).ToString();
                    }
                    else if (rand_number == COST)
                    {
                        double old = 0;
                        rand_number = rnd.Next(50000, 2000001);
                        rand_number2 = rnd.Next(2, range - 1);
                        old = sh2.Cells[rand_number2, COST].Value2;
                        sh2.Cells[rand_number2, COST].Value2 = rand_number.ToString();
                    }
                }
            }
        }
        public static void fill_towns(ref Excel.Worksheet sh)
        {
            Random rnd = new Random();
            // 915 - number or towns
            for (int i = 2; i <= 916; i++)
            {
                update_value(ref sh, i, NUM_OF_FIRMS, rnd.Next(1, 20).ToString());
            }
        }
        public static void update_value(ref Excel.Worksheet sh, int raw, int column, string value)
        {
            sh.Cells[raw, column].Value2 = value;
        }
    }
}
