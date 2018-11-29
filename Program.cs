using System;
using System.Threading;
using System.Linq;
using System.Data.SQLite;
using System.IO;
using System.Globalization;
using System.Text;

// repeat:
#region Klasse: Monat in Text
static class DateTimeExtensions
{
    public static string ToMonthName(this DateTime dateTime)
    {
        return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
    }
}
#endregion
class Program
{

   

    #region FinancialNote Struct (FUNKTION VOLLSTÄNDIG)
    public struct FinancialNote
    {
        public string Arg1;
        public string Arg2;
        public string Arg3;
        public string Arg4;

        public string Einnahme_Verwendungszweck;
        public int Einnahme_Wert;

        public string Ausgaben_Verwendungszweck;
        public int Ausgaben_Wert;
       
        public DateTime Datum;
        public int Buchungsnummer;
        public int ID;
    }
    #endregion 

    #region WriteLine Methode (FUNKTION VOLLSTÄNDIG)
    static void WriteLine(string s, Alignments a)
    {
        switch (a)
        {
            case Alignments.Left:
                Console.Write(s);
                break;
            case Alignments.Center:
                Console.Write(s.PadLeft((Console.WindowWidth - s.Length) / 2 + s.Length));
                break;
            case Alignments.Right:
                Console.Write(s.PadLeft(Console.WindowWidth));
                break;
        }
        Console.WriteLine();
    }
    #endregion
    
    #region ASCII Intro (FUNKTION VOLLSTÄNDIG)
    static void Intro()
    {
        Console.WriteLine(" /$$$$$$$$ /$$                                         /$$           /$$ /$$   /$$            | $$ ");
        Console.WriteLine("| $$______/|__/                                        |__/          |$$| $$$ | $$            | $$  ");
        Console.WriteLine("| $$___   / $$/ $$$$$$$   /$$$$$$  /$$$$$$$   /$$$$$$$ /$$  /$$$$$$  |$$| $$$$| $$  /$$$$$$  /$$$$$$|   /$$$$$$");
        Console.WriteLine("| $$$$$|  | $$| $$__  $$ | ____$$|  $$__  $$ /$$_____  /$$ | ____ $$ |$$| $$ $$ $$ /$$__  $$| _ $$_ /  /$$__  $$");
        Console.WriteLine("| $$__ /  | $$| $$  | $$  /$$$$$$$| $$  | $$| $$     |  $$  /$$$$$$$| $$| $$  $$$$| $$  | $$  | $$    | $$$$$$$$");
        Console.WriteLine("| $$      | $$| $$  | $$ /$$__  $$| $$  | $$| $$     |  $$ / $$__ $$| $$| $$   $$$| $$  | $$  | $$ /$$| $$_____/");
        Console.WriteLine("| $$      | $$| $$  | $$|  $$$$$$$| $$  | $$|  $$$$$$$| $$|  $$$$$$$| $$| $$    $$|  $$$$$$/  |  $$$$/|  $$$$$$$");
        Console.WriteLine("| __      |__ |__/  |__   _______/|__/  |__   _______/|__   _______/ __/|__/   __/|_______/   |/____/   /_______/");
        System.Threading.Thread.Sleep(1300);
    }
    #endregion

    #region Startargumente (FUNKTION VOLLSTÄNDIG)
    static void Start(string Ausgabetext, string Ausgabetext2, string Ausgabetext3, string Ausgabetext4)
    {
        // Aufjedenfall Benutzer löschen mit Structs
        Console.WriteLine(Ausgabetext);
        Console.WriteLine(Ausgabetext2);
        Console.WriteLine(Ausgabetext3);
        Console.WriteLine(Ausgabetext4);
    }
    #endregion

    #region Buchungsvorgang Methoden (FUNKTION VOLLSTÄNDIG) 
    static void Einnahmen_buchen(string Monat)
    {

        // EINNAHMEN BUCHEN!

        FinancialNote FN;

        FN.Buchungsnummer = FinancialNoteProg.Properties.Settings.Default.Buchungsnummer;

        Console.Clear();
        Console.WriteLine("FinancialNote - Allgemein - Einnahmen buchen\n");
        Console.WriteLine("Es wird in den Monat {0} gebucht!\n",Monat);

        try
        { 
        Console.WriteLine("Wert der Einnahme:");
        FN.Einnahme_Wert = Convert.ToInt16(Console.ReadLine());
        Console.WriteLine("Verwendungszweck:");
        FN.Einnahme_Verwendungszweck = Console.ReadLine();
        Console.WriteLine("Geben Sie das Belegdatum(yyyy-mm-dd) ein:");
        FN.Datum = DateTime.Parse(Console.ReadLine());

        FinancialNoteProg.Properties.Settings.Default.Buchungsnummer = FinancialNoteProg.Properties.Settings.Default.Buchungsnummer + 1;

        string sql1 = string.Format("insert into {0} (Buchungsnummer,Soll,Haben,Verwendungszweck,Datum) values (@param1,@param2,@param3,@param4,strftime('%Y-%m-%d', @param5))", Monat);

        SQLiteConnection dbConnection = new SQLiteConnection("Data Source=DB_FinancialNote.sqlite;Version=3;");
        dbConnection.Open();

        SQLiteCommand cmd1 = new SQLiteCommand(sql1, dbConnection);
        cmd1.Parameters.Add(new SQLiteParameter("@param1", FN.Buchungsnummer));
        cmd1.Parameters.Add(new SQLiteParameter("@param2", FN.Einnahme_Wert));
        cmd1.Parameters.Add(new SQLiteParameter("@param3", ""));
        cmd1.Parameters.Add(new SQLiteParameter("@param4", FN.Einnahme_Verwendungszweck));
        cmd1.Parameters.Add(new SQLiteParameter("@param5", FN.Datum));

        cmd1.ExecuteNonQuery();
        }
        catch (Exception bug)
        {
            Console.WriteLine("Fehler: {0}\n", bug.Message);
            Console.WriteLine("Formular wird geschlossen!");
            Console.ReadKey();
        }
        
        FinancialNoteProg.Properties.Settings.Default.Save();
    }

    static void Ausgaben_buchen(string Monat)
    {

        // AUSGABEN BUCHEN

        FinancialNote FN;

        FN.Buchungsnummer = FinancialNoteProg.Properties.Settings.Default.Buchungsnummer;

        Console.Clear();
        Console.WriteLine("FinancialNote - Allgemein - Ausgaben buchen\n");
        Console.WriteLine("Es wird in den Monat {0} gebucht!\n", Monat);

        
        try
        {
            Console.WriteLine("Wert der Ausgabe:");
            FN.Ausgaben_Wert = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("Verwendungszweck:");
            FN.Ausgaben_Verwendungszweck = Console.ReadLine();
            Console.WriteLine("Geben Sie das Belegdatum(yyyy-mm-dd) ein:");
            FN.Datum = DateTime.Parse(Console.ReadLine());

            FinancialNoteProg.Properties.Settings.Default.Buchungsnummer = FinancialNoteProg.Properties.Settings.Default.Buchungsnummer + 1;

            string sql1 = string.Format("insert into {0} (Buchungsnummer,Soll,Haben,Verwendungszweck,Datum) values (@param1,@param2,@param3,@param4,strftime('%Y-%m-%d', @param5))", Monat);

            SQLiteConnection dbConnection = new SQLiteConnection("Data Source=DB_FinancialNote.sqlite;Version=3;");
            dbConnection.Open();

            SQLiteCommand cmd1 = new SQLiteCommand(sql1, dbConnection);
            cmd1.Parameters.Add(new SQLiteParameter("@param1", FN.Buchungsnummer));
            cmd1.Parameters.Add(new SQLiteParameter("@param2", ""));
            cmd1.Parameters.Add(new SQLiteParameter("@param3", FN.Ausgaben_Wert));
            cmd1.Parameters.Add(new SQLiteParameter("@param4", FN.Ausgaben_Verwendungszweck));
            cmd1.Parameters.Add(new SQLiteParameter("@param5", FN.Datum));

            cmd1.ExecuteNonQuery();
        }
        catch (Exception bug)
        {
            Console.WriteLine("Fehler: {0}\n", bug.Message);
            Console.WriteLine("Formular wird geschlossen!");
            Console.ReadKey();
        }

        
        FinancialNoteProg.Properties.Settings.Default.Save();
    }
    #endregion

    #region Datenbank anlegen (FUNKTION VOLLSTÄNDIG)
    static void Datenbank_anlegen()

    {
        if (File.Exists(@"DB_FinancialNote.sqlite"))
        {
            SQLiteConnection dbConnection = new SQLiteConnection("Data Source=DB_FinancialNote.sqlite;Version=3;");

            dbConnection.Open();
        }
        else
        {
            SQLiteConnection.CreateFile(Environment.CurrentDirectory + @"\DB_FinancialNote.sqlite");

            SQLiteConnection dbConnection = new SQLiteConnection("Data Source=DB_FinancialNote.sqlite;Version=3;");

            dbConnection.Open();
        }

    }
    #endregion

    #region Datenbank SQL Table Create(FUNKTION VOLLSTÄNDIG)
    static void SQL_Table_CREATE()

    {

        string SQLA0 = "create table if not exists Januar (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Buchungsnummer INTEGER(6), Soll INTEGER(20), Haben INTEGER(20), Verwendungszweck string(30), Datum datetime)";
        string SQLA1 = "create table if not exists Februar (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Buchungsnummer INTEGER(6), Soll INTEGER(20), Haben INTEGER(20), Verwendungszweck string(30), Datum datetime)";
        string SQLA2 = "create table if not exists März (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Buchungsnummer INTEGER(6), Soll INTEGER(20), Haben INTEGER(20), Verwendungszweck string(30), Datum datetime)";
        string SQLA3 = "create table if not exists April (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Buchungsnummer INTEGER(6), Soll INTEGER(20), Haben INTEGER(20), Verwendungszweck string(30), Datum datetime)";
        string SQLA4 = "create table if not exists Mai (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Buchungsnummer INTEGER(6), Soll INTEGER(20), Haben INTEGER(20), Verwendungszweck string(30), Datum datetime)";
        string SQLA5 = "create table if not exists Juni (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Buchungsnummer INTEGER(6), Soll INTEGER(20), Haben INTEGER(20), Verwendungszweck string(30), Datum datetime)";
        string SQLA6 = "create table if not exists Juli (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Buchungsnummer INTEGER(6), Soll INTEGER(20), Haben INTEGER(20), Verwendungszweck string(30), Datum datetime)";
        string SQLA7 = "create table if not exists August (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Buchungsnummer INTEGER(6), Soll INTEGER(20), Haben INTEGER(20), Verwendungszweck string(30), Datum datetime)";
        string SQLA8 = "create table if not exists September (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Buchungsnummer INTEGER(6), Soll INTEGER(20), Haben INTEGER(20), Verwendungszweck string(30), Datum datetime)";
        string SQLA9 = "create table if not exists Oktober (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Buchungsnummer INTEGER(6), Soll INTEGER(20), Haben INTEGER(20), Verwendungszweck string(30), Datum datetime)";
        string SQLA10 = "create table if not exists November (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Buchungsnummer INTEGER(6), Soll INTEGER(20), Haben INTEGER(20), Verwendungszweck string(30), Datum datetime)";
        string SQLA11 = "create table if not exists Dezember (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Buchungsnummer INTEGER(6), Soll INTEGER(20), Haben INTEGER(20), Verwendungszweck string(30), Datum datetime)";

        SQLiteConnection dbConnection = new SQLiteConnection("Data Source=DB_FinancialNote.sqlite;Version=3;");
        dbConnection.Open();

        string [] sql = new string[12];
        int sqlcount;

     for (sqlcount = 0; sqlcount < 12; sqlcount++)
            {
        sql[sqlcount] = "SQLiteCommand cmd1 = new SQLiteCommand(SQLA" + sqlcount + ", dbConnection)";
            }

        string[] cmd = new string[12];
        int cmdcount;

        for (cmdcount = 0; cmdcount < 12; cmdcount++)
        {
            sql[cmdcount] = "cmdA" + cmdcount + ", dbConnection)";
        }


        SQLiteCommand cmd1 = new SQLiteCommand(SQLA0, dbConnection);
        cmd1.ExecuteNonQuery();

        SQLiteCommand cmd2 = new SQLiteCommand(SQLA1, dbConnection);
        cmd2.ExecuteNonQuery();

        SQLiteCommand cmd3 = new SQLiteCommand(SQLA2, dbConnection);
        cmd3.ExecuteNonQuery();

        SQLiteCommand cmd4 = new SQLiteCommand(SQLA3, dbConnection);
        cmd4.ExecuteNonQuery();

        SQLiteCommand cmd5 = new SQLiteCommand(SQLA4, dbConnection);
        cmd5.ExecuteNonQuery();

        SQLiteCommand cmd6 = new SQLiteCommand(SQLA5, dbConnection);
        cmd6.ExecuteNonQuery();

        SQLiteCommand cmd7 = new SQLiteCommand(SQLA6, dbConnection);
        cmd7.ExecuteNonQuery();

        SQLiteCommand cmd8 = new SQLiteCommand(SQLA7, dbConnection);
        cmd8.ExecuteNonQuery();

        SQLiteCommand cmd9 = new SQLiteCommand(SQLA8, dbConnection);
        cmd9.ExecuteNonQuery();

        SQLiteCommand cmd10 = new SQLiteCommand(SQLA9, dbConnection);
        cmd10.ExecuteNonQuery();

        SQLiteCommand cmd11 = new SQLiteCommand(SQLA10, dbConnection);
        cmd11.ExecuteNonQuery();

        SQLiteCommand cmd12 = new SQLiteCommand(SQLA11, dbConnection);
        cmd12.ExecuteNonQuery();
    }
    #endregion

    #region Datenbank SQL Table Call - Monatsauswertung(FUNKTION VOLLSTÄNDIG)
    static void SQL_TABLE_CALL(string Monat)
    {
        // Monatsauswertung call the right Table
        Console.OutputEncoding = Encoding.UTF8;
        SQLiteConnection dbConnection = new SQLiteConnection("Data Source=DB_FinancialNote.sqlite;Version=3;");
        dbConnection.Open();
        Console.Clear();
        Console.WriteLine("FinancialNote - Allgemein - Monatsauswertung\n");
        Console.WriteLine("Monatauswertung für den Monat {0}\n", Monat);
        string sql2 = string.Format("select * from {0}", Monat);
        SQLiteCommand command2 = new SQLiteCommand(sql2, dbConnection);
        SQLiteDataReader reader = command2.ExecuteReader();
        Console.WriteLine("ID\tBuchungsnummer\t\tSoll\t\tHaben\t\tVerwendungszweck\tDatum\n---------------------------------------------------------------------------------------------------");
        while (reader.Read())
        { 
            Console.WriteLine("{0}\t{1}\t\t\t{2}\t\t{3}\t\t{4}\t\t\t{5}", reader[0].ToString(), reader[1].ToString(),reader[2].ToString() + "€", reader[3].ToString() + "€", reader[4].ToString(), (DateTime.Parse(reader[5].ToString())).ToString("d"));
        }
        reader.Close();
        Console.WriteLine("---------------------------------------------------------------------------------------------------");
        Console.ReadKey();
    }
    #endregion

    #region Init
    static void Main(string[] args)
    {
        
        FinancialNote FN;

        FinancialNoteProg.Properties.Settings.Default.firststartb = true;

        FN.ID = 0;
        FN.Buchungsnummer = 1;

        //ASCII WELCOME
        Intro();

        //DB
        Datenbank_anlegen();

        //SQL CREATE TABLES
        SQL_Table_CREATE();

        FN.Arg1 = "Willkommen bei FinancialNote!\n";
        FN.Arg2 = "Es wurde festgestellt das Sie, dass Programm zum ersten Mal starten!\n";
        FN.Arg3 = "Bitte tragen Sie nachfolgend Ihren gewünschten Benutzernamen ein!\n";

        if (FinancialNoteProg.Properties.Settings.Default.firststartb == true)
        {
            again:
            try
            {
                Console.Clear();
                Start(FN.Arg1, FN.Arg2, FN.Arg3, "Benutzername:");
                FinancialNoteProg.Properties.Settings.Default.Benutzername = Console.ReadLine();
                FinancialNoteProg.Properties.Settings.Default.Save();
                Console.WriteLine("");
                Console.WriteLine("Verbuchen Sie Ihre erste Einnahme:");
                FN.Einnahme_Wert = Convert.ToInt16(Console.ReadLine());
                FinancialNoteProg.Properties.Settings.Default.Buchungsnummer = 0;

                Console.WriteLine("");
                Console.WriteLine("Geben Sie einen Verwendungszweck an\nBsp: Gehalt, Lohn oder eine sonstige Einnahme.");
                Console.WriteLine("Vewendungszweck:");
                FN.Einnahme_Verwendungszweck = Console.ReadLine();

                Console.WriteLine("");
                Console.WriteLine("Geben Sie das Datum(yyyy-mm-dd) der Einnahme ein:");
                FN.Datum = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("");
                Console.WriteLine("Setup abgeschlossen!");

                string sql1 = string.Format("insert into {0} (Buchungsnummer,Soll,Haben,Verwendungszweck,Datum) values (@param1,@param2,@param3,@param4,strftime('%Y-%m-%d', @param5))", DateTime.Now.ToMonthName());


                SQLiteConnection dbConnection = new SQLiteConnection("Data Source=DB_FinancialNote.sqlite;Version=3;");
                dbConnection.Open();

                SQLiteCommand cmd1 = new SQLiteCommand(sql1, dbConnection);
                cmd1.Parameters.Add(new SQLiteParameter("@param1", FN.Buchungsnummer));
                cmd1.Parameters.Add(new SQLiteParameter("@param2", FN.Einnahme_Wert));
                cmd1.Parameters.Add(new SQLiteParameter("@param3", ""));
                cmd1.Parameters.Add(new SQLiteParameter("@param4", FN.Einnahme_Verwendungszweck));
                cmd1.Parameters.Add(new SQLiteParameter("@param5", FN.Datum));

                cmd1.ExecuteNonQuery();

                FinancialNoteProg.Properties.Settings.Default.Benutzernameb = false;
                FinancialNoteProg.Properties.Settings.Default.firststartb = false;
                FinancialNoteProg.Properties.Settings.Default.Save();
                FinancialNoteProg.Properties.Settings.Default.Buchungsnummer = FinancialNoteProg.Properties.Settings.Default.Buchungsnummer + 1;

            }
            catch (Exception bug)
            {
                Console.WriteLine("Fehler: {0}\n", bug.Message);
                Console.WriteLine("Bitte erneut versuchen!");
                FinancialNoteProg.Properties.Settings.Default.Benutzernameb = false;
                FinancialNoteProg.Properties.Settings.Default.firststartb = true;
                Console.ReadKey();
                goto again;
            }

            Thread.Sleep(800);
            


        }



        ZeichenUngueltig:
        
      if (FinancialNoteProg.Properties.Settings.Default.Benutzernameb == true)
            {
                Console.Clear();
                FN.Arg1 = "Benutzername ohne Zeichen nicht zulässig!\n";
                FN.Arg2 = "Bitte tragen Sie nachfolgend Ihren gewünschten Benutzernamen ein!";
                FN.Arg3 = "";
                Start(FN.Arg1, FN.Arg2, FN.Arg3, "Benutzername:");
                FinancialNoteProg.Properties.Settings.Default.Benutzername = Console.ReadLine();
            }

        if (FinancialNoteProg.Properties.Settings.Default.Benutzername.Any(Char.IsLetter))
        {

        }
        else
        {
            FinancialNoteProg.Properties.Settings.Default.Benutzernameb = true;
            goto ZeichenUngueltig;
        }
        #endregion

    #region Hauptmenü
        while (true)
            {
                if (FinancialNoteProg.Properties.Settings.Default.firststartb == false)
                {
            backMain:
                    int curItem = 0, c;
                    ConsoleKeyInfo key;
                
                    do
                    {
                    
                    Console.Clear();
                    
                    string[] menuItems = { "[1] Notiere", "[2] Einstellungen", "[3] Programm beenden" };
                    Console.WriteLine("Guten Tag {0}!, Datum: {1}.\n", FinancialNoteProg.Properties.Settings.Default.Benutzername,DateTime.Now.ToShortDateString());
                    WriteLine("FinancialNote - Hauptmenü\n", Alignments.Left);
                    
                        

                        for (c = 0; c < menuItems.Length; c++)
                        {

                            if (curItem == c)
                            {
                                Console.Write(">>");
                                Console.WriteLine(menuItems[c]);
                            }

                            else
                            {
                                Console.WriteLine(menuItems[c]);
                            }
                        }


                        key = Console.ReadKey(true);

                        if (key.Key.ToString() == "DownArrow")
                        {
                            curItem++;
                            if (curItem > menuItems.Length - 1) curItem = 0;
                        }
                        else if (key.Key.ToString() == "UpArrow")
                        {
                            curItem--;
                            if (curItem < 0) curItem = Convert.ToInt16(menuItems.Length - 1);
                        }
                    }
                    
                    while (key.KeyChar != 13);
            #endregion

    #region Buchungsfunktionen   
            
               
                switch (curItem)
                    {
                     case 0:
                       while (true)
                        {
                            buchungsfunktionen:
                            curItem = 0;
                            do
                            {
                                Console.Clear();
                                WriteLine("FinancialNote - Allgemein\n", Alignments.Left);

                                Console.WriteLine("Wählen Sie zwischen einer der Optionen:\n");
                                string[] menuItems1 = { "[1] Einnahmen einbuchen", "[2] Ausgaben einbuchen","[3] Monatsauswertung", "[4] Zurück" };
                                for (c = 0; c < menuItems1.Length; c++)
                                {

                                    if (curItem == c)
                                    {
                                        Console.Write(">>");

                                        Console.WriteLine(menuItems1[c]);
                                    }

                                    else
                                    {
                                        Console.WriteLine(menuItems1[c]);
                                    }
                                }

                                key = Console.ReadKey(true);

                                if (key.Key.ToString() == "DownArrow")
                                {
                                    curItem++;
                                    if (curItem > menuItems1.Length - 1) curItem = 0;
                                }
                                else if (key.Key.ToString() == "UpArrow")
                                {
                                    curItem--;
                                    if (curItem < 0) curItem = Convert.ToInt16(menuItems1.Length - 1);

                                }


                            }
                            while (key.KeyChar != 13);

                            switch (curItem)
                            {
                                case 0:
                                    Einnahmen_buchen(DateTime.Now.ToMonthName());
                                    break;

                                case 1:
                                    Ausgaben_buchen(DateTime.Now.ToMonthName());
                                    break;
                                #endregion

    #region Monatsauswertung
                                case 2:
                                    curItem = 0;
                                    while (true)
                                    {

                                        do
                                        {
                                            Console.Clear();
                                            Console.WriteLine("FinancialNote - Monatsauswertung\n");

                                            Console.WriteLine("Wählen Sie zwischen einer der Monate:\n");
                                            string[] menuItems1 = { "[1] Januar", "[2] Februar", "[3] März", "[4] April", "[5] Mai", "[6] Juni", "[7] Juli", "[8] August", "[9] September", "[10] Oktober", "[11] November", "[12] Dezember", "[13] Zurück" };
                                            for (c = 0; c < menuItems1.Length; c++)
                                            {

                                                if (curItem == c)
                                                {
                                                    Console.Write(">>");

                                                    Console.WriteLine(menuItems1[c]);
                                                }

                                                else
                                                {
                                                    Console.WriteLine(menuItems1[c]);
                                                }
                                            }

                                            key = Console.ReadKey(true);

                                            if (key.Key.ToString() == "DownArrow")
                                            {
                                                curItem++;
                                                if (curItem > menuItems1.Length - 1) curItem = 0;
                                            }
                                            else if (key.Key.ToString() == "UpArrow")
                                            {
                                                curItem--;
                                                if (curItem < 0) curItem = Convert.ToInt16(menuItems1.Length - 1);

                                            }


                                        }
                                        while (key.KeyChar != 13);

                                        switch (curItem)
                                        {
                                            case 0: // Januar                                              
                                                SQL_TABLE_CALL("Januar");
                                            
                                                break;

                                            case 1: // Februar
                                                SQL_TABLE_CALL("Februar");
                                                break; 

                                            case 2: // März
                                                SQL_TABLE_CALL("März");
                                                break;

                                            case 3: // April
                                                SQL_TABLE_CALL("April");
                                                break;

                                            case 4: // Mai
                                                SQL_TABLE_CALL("Mai");
                                                break;

                                            case 5: // Juni
                                                SQL_TABLE_CALL("Juni");
                                                break;

                                            case 6: // Juli
                                                SQL_TABLE_CALL("Juli");
                                                break;

                                            case 7: // August
                                                SQL_TABLE_CALL("August");
                                                break;

                                            case 8: // September
                                                SQL_TABLE_CALL("September");
                                                break;

                                            case 9: // Oktober
                                                SQL_TABLE_CALL("Oktober");
                                                break;

                                            case 10: // November
                                                SQL_TABLE_CALL("November");
                                                break;

                                            case 11: // Dezember
                                                SQL_TABLE_CALL("Dezember");
                                                break;
                                            case 12: // Zurück
                                                goto buchungsfunktionen;
                                        }

                                    }

                                #endregion

    #region Allgemein Back to Hauptmenü
                                case 3:
                                    goto backMain;
                                    
                                    
                            }

                        }
                    #endregion

    #region Einstellungen (FUNKTION VOLLSTÄNDIG)

                    case 1:
                        curItem = 0;
                        while (true)
                        {
                            
                            do
                            {
                                Console.Clear();
                                WriteLine("FinancialNote - Einstellungen\n", Alignments.Left);
                                
                                Console.WriteLine("Wählen Sie zwischen einer der Optionen:\n");
                                string[] menuItems2 = { "[1] Benutzername ändern", "[2] Zurück"};

                                for (c = 0; c < menuItems2.Length; c++)
                                {

                                    if (curItem == c)
                                    {
                                        Console.Write(">>");

                                        Console.WriteLine(menuItems2[c]);
                                    }
                                    else
                                    {
                                        Console.WriteLine(menuItems2[c]);
                                    }
                                }

                                key = Console.ReadKey(true);

                                if (key.Key.ToString() == "DownArrow")
                                {
                                    curItem++;
                                    if (curItem > menuItems2.Length - 1) curItem = 0;
                                }
                                else if (key.Key.ToString() == "UpArrow")
                                {
                                    curItem--;
                                    if (curItem < 0) curItem = Convert.ToInt16(menuItems2.Length - 1);

                                }


                            }
                            while (key.KeyChar != 13);
                            
                            switch (curItem)
                            {
                                case 0://Benutzernamen ändern
                                    Console.Clear();
                                    Console.WriteLine("FinancialNote - Einstellungen - Benutzernamen ändern\n");
                                    Console.WriteLine("Alter Benutzername:{0}\n", FinancialNoteProg.Properties.Settings.Default.Benutzername);
                                    Console.WriteLine("Bitte vergeben Sie einen neuen Benutzernamen:");
                                    FinancialNoteProg.Properties.Settings.Default.Benutzername = Console.ReadLine();
                                    FinancialNoteProg.Properties.Settings.Default.Benutzernameb = false;
                                    goto ZeichenUngueltig;

                                case 1:
                                    goto backMain;                        
                                }

                        }
                    #endregion

    #region Hauptmenü Programm beenden (FUNKTION VOLLSTÄNDIG)
                    case 2:
                        SQLiteConnection dbConnection = new SQLiteConnection("Data Source=DB_FinancialNote.sqlite;Version=3;");
                        
                        dbConnection.Close();
                        FinancialNoteProg.Properties.Settings.Default.Save();
                        Environment.Exit(0);
                        break;

                    }
                

            }
            }
    }
    #endregion

    #region Alignments (FUNKTION VOLLSTÄNDIG) 
    enum Alignments
    {
        Left,
        Center,
        Right,
    }
}
#endregion


    
