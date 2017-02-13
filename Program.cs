using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
//chybí: ulozeni poslednich matic
//funguje:  lineární rovnice,determinant, násobení,násobení o jiném rozmeru, sčítání,odčítání, submatice(zadat sloupce a řádky,nebo sloupce a řádky které ve výsledku nemají být), inverzní matice,
namespace ConsoleApplication3
{
    class Program
    {
        public static List<Matice> posledni_vysledky=new List<Matice>(); //list s posledními maticemi 
        public static string[] Operace = 
            {"uložit vlastní matici",
            "získat determinant matice",
            "získat inverzní matici",
            "součet matic",
            "rozdíl matic",
            "součin matic",
            "získat submatici",
            "výpočet kořenů soustavy lineárních rovnic o n neznámých",
        "zobrazit všechny uložené matice"};// konstantni pole názvů operací
        public static char[] pismena = { 'a', 'á', 'b', 'c', 'č', 'd', 'ď', 'e', 'é', 'ě', 'f', 'g', 'h', 'i', 'í', 'j', 'k', 'l', 'm', 'n', 'ň', 'o', 'ó', 'p', 'q', 'r', 'ř', 's', 'š', 't', 'ť', 'u', 'ú', 'ů', 'v', 'w', 'x', 'y', 'ý', 'z', 'ž' };
        public static char[] cislice = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static int getKey(string source) {
            int result = 0;
            try {
                result = Int32.Parse(source[source.Length-1].ToString());
            }
            catch (Exception e) {
                Console.WriteLine("\nŠpatný formát vstupu");
                result = -1;
            }
            return result; }//získání klávesy
       
        public static TOutput[,] ConvertAll<TInput, TOutput>(TInput[,] array, Converter<TInput, TOutput> converter)
            //konvertování dvojrozměrného pole
    {
        if (array == null)
        {
            throw new ArgumentNullException("array");
        }
        if (converter == null)
        {
            throw new ArgumentNullException("converter");
        }
        int height = array.GetLength(0);
        int width = array.GetLength(1);
        TOutput[,] localArray = new TOutput[width, height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
                localArray[j, i] = converter(array[i, j]);
        }
        return localArray;
    }
        static void SwapElements(string[,] array, int x1,int y1,int x2,int y2) //výměna dvou elementů v dvojrozměrném poli
        {
            string temp = array[x1,y1]; // Copy the first position's element
            array[x1,y1] = array[x2,y2]; // Assign to the second element
            array[x2,y2] = temp; // Assign to the first element
        }
        public struct GetArrays { //struktura pro levou a pravou stranu soustavy linearnich rovnic
            public string[,] left;
            public string[,] right;

        }
        public static bool ContainsValue(char[] arr, string zdroj) // vrací true jestliže je ve zdroji alespoň jeden znak z pole arr
        {
            bool boo=false;
            foreach (char k in arr) {
                if (zdroj.Contains(k)) {
                    boo = true;
                    break;
                }
            }
            return boo; }
        public static string[,] DeleteLetters(string[,] pole) //vymaže všechny písmena z dvojrozměrného pole 
        {
            for (int a =0;a<pole.GetLength(0);a++) {
                for (int b=0;b<pole.GetLength(1);b++) {
                    if (ContainsValue(pismena, pole[a, b])) {
                        foreach (char key in pismena)
                        {
                           pole[a,b]= pole[a, b].Replace(key, ' ');
                            pole[a,b]=pole[a, b].Trim(' ');
                        }
                    } } }
            return pole;
        }
        public static GetArrays GetLeftAndRightArray(string[] pole,int n)//získá levou a pravou stranu soustavy rovnic
        {
            string[,] vlevo = new string[n, n];
            string[,] vpravo = new string[n, 1];
            int count = 0;
            string mezera = "/";
            string[] q = null;
            for (int k = 0; k < pole.Length; k++)
            {
                string[] pole1 = pole[k].Split('=');
                
                foreach (char i in pole1[0])
                {
                    for (int a = 0; a < pismena.Length; a++)
                    {
                        if (i == pismena[a])
                        {
                            int index = pole1[0].IndexOf(i);
                            pole1[0] = pole1[0].Insert(index + 1, mezera);
                            count = pole1[0].Count(f => f == pismena[a]);
                            if (count > 1)
                            {
                                Console.WriteLine("Vice stejnych promennych na jednom radku");
                               // goto BREAKS;
                                break;
                            }
                        }
                    }

                }
            
                string arr = pole1[0];
                q = arr.Split(mezera[0]);
                Array.Resize(ref q, q.Length - 1);
            int[] t = new int[n];

                for (int r = 0; r < q.Length; r++)
                {
                    vlevo[k, r] = q[r];
}
                foreach (char key in pismena) { //kdyz prava strana obsahuje nejake pismeno, stop
                    if (pole1[1].Contains(key))
                    {
                        break;
                    }
                    else { continue; }
                            }


                vpravo[k, 0] = pole1[1];
            
            }

            var result = new GetArrays{
            left = vlevo,
            right = vpravo};
            return result; 
        }
        public static string[,] ReplaceBadCharacters(string[,] pole)//nahrazení - a + členu 1 nebo -1
        {
            string znamenko = "";
            string[,] vlevo = pole;

            for (int k = 0; k < pole.GetLength(0); k++)
            {
                for (int r = 0; r < pole.GetLength(0); r++)
                {
                    for (int s = 0; s < pole.GetLength(1); s++)
                    {
                        int[] t = new int[pole.GetLength(0)];
                        pole[r, s] = pole[r, s].Replace('+', ' ');    ///nahrazeni - a + členu 1 a -1
                        pole[r, s] = pole[r, s].Trim(' ');

                        if (!ContainsValue(cislice, pole[r, s]))
                        {
                            if (pole[r, s].Contains("-"))
                            {
                                vlevo[r, s] = "-1";
                            }
                            else { vlevo[r, s] = "1"; }

                        }//nahrazeni clenu bez koeficientu 1 nebo -1
                    }
                }
                
            }return vlevo;
        }
        static void Main(string[] args){
            Console.Title = "Matice";
            while (true) {
                    Console.WriteLine("Vyberte, kterou operaci chcete provést:");
                    for (int a = 0; a < Operace.Length; a++) {
                        Console.WriteLine(a + " - " + Operace[a]);
                    }
                    int cislo_operace = getKey(Console.ReadKey().Key.ToString());
                    switch (cislo_operace) {
                        case 0:
                            {
                                Console.WriteLine("\nZadejte nazev vaší matice: ");
                                string nazev_matice = Console.ReadLine();
                                try
                                {
                                    Console.WriteLine("Zadejte rozměr X vaši matice: ");
                                    int x = Int32.Parse(Console.ReadLine());
                                    Console.WriteLine("Zadejte rozměr Y vaši matice: ");
                                    int y = Int32.Parse(Console.ReadLine());
                                    Matice matice = new Matice(y, x);
                                    matice.pole = matice.getData();
                                    matice.ulozMatici(nazev_matice);
                                matice.name = nazev_matice;
                                posledni_vysledky.Add(matice);
                                }
                                catch (Exception e) { Console.WriteLine("Špatně zadané čísla"); break; }

                                break;
                            }
                        case 1:
                            {
                                Console.WriteLine("\nDeterminant matice n x n: nacist matici? : a/n/p -posledni matice ");
                                Matice matice = null;
                                char klavesa = Convert.ToChar(Console.ReadKey().Key);
                            if (klavesa == 'A')
                            {
                                Console.WriteLine("\nJméno uložené matice: ");
                                string jmeno = Console.ReadLine();
                                matice = Matice.ziskejUlozenouMatici(jmeno);
                                if (matice == null) {
                                    Console.WriteLine("Nenalezena žádná matice");
                                    break;
                                }
                            }
                            else if (klavesa == 'N') {
                                Console.WriteLine("Zadejte rozměr N vaši matice: ");
                                int n = Int32.Parse(Console.ReadLine());

                                matice = new Matice(n);
                                matice.pole = matice.getData();
                            } else if (klavesa=='P') {
                                if (posledni_vysledky.Count==0) { Console.WriteLine("\nPosledni matice je prazdna."); break; }
                                else {
                                     matice = posledni_vysledky.Last();
                                }
                            }
                            else {
                                Console.WriteLine("Zadejte a nebo n");
                                break; }


                                double det = matice.getDeterminant(matice.pole, matice.rozmer);
                                if (klavesa == 'A')
                                {
                                    Console.WriteLine(matice.zobrazMatici(matice.pole));
                                }
                                Console.WriteLine("Determinant matice o rozměru {0}: {1}\nLoops: {2}", matice.rozmer.ToString(), det.ToString(),matice.pocitadlo);
                            posledni_vysledky.Add(matice);

                            break; }

                        case 2: {
                                Console.WriteLine("\nInverzni matice: nacist matici?: a/n/p - načíst poslední matici");
                                Matice original = null;
                                char klavesa = Convert.ToChar(Console.ReadKey().Key);
                                if (klavesa == 'A')
                                {
                                    Console.WriteLine("\nJméno uložené matice: ");
                                    string jmeno = Console.ReadLine();
                                    original = Matice.ziskejUlozenouMatici(jmeno);
                                    if (original.x != original.y) {
                                        Console.WriteLine("Matice musí být n x n");
                                        break;
                                    }
                                    if (original == null)
                                    {
                                        Console.WriteLine("Nenalezena žádná matice");
                                        break;
                                    } Console.WriteLine(original.zobrazMatici(original.pole));
                            }
                            else if (klavesa == 'P')
                            {
                                if (posledni_vysledky.Count == 0) { Console.WriteLine("\nPosledni matice je prazdna."); break; }
                                else
                                {
                                    original = posledni_vysledky.Last();
                                }
                            }
                            else if (klavesa == 'N')
                                {
                                    Console.WriteLine("Zadejte rozměr N vaši matice: ");
                                    int n = Int32.Parse(Console.ReadLine());

                                    Console.WriteLine();

                                    original = new Matice(n);
                                    original.pole = original.getData();
                                }

                                Matice inverze = original.getInverse(original);
                                posledni_vysledky.Add(inverze);
                                Console.WriteLine("Inverzni matice:\n{0}", inverze.zobrazMatici(inverze.pole));
                            posledni_vysledky.Add(inverze);
                            break; }
                        case 3: {
                                Matice soucet = null;
                                Matice[] scitance = new Matice[2];
                                int x = 0, y = 0;
                            REPEAT:
                                for (int i = 1; i <= 2; i++) {
                                BREAK: Console.WriteLine("Sčítání: přejete si načíst {0}.sčítanec: a/n ", i.ToString());
                                    Matice scitanec = null;
                                    ConsoleKey klavesa = Console.ReadKey().Key;
                                    if (klavesa == ConsoleKey.A) {
                                        Console.WriteLine("\nJméno uložené matice: ");
                                        string jmeno = Console.ReadLine();
                                        scitanec = Matice.ziskejUlozenouMatici(jmeno);

                                        if (scitanec == null)
                                        {
                                            Console.WriteLine("Nenalezena žádná matice");
                                            goto BREAK;
                                        }
                                        Console.WriteLine(scitanec.zobrazMatici(scitanec.pole));
                                        scitance[i - 1] = scitanec;
                                    }
                                    else if (klavesa == ConsoleKey.N) {
                                        try
                                        {

                                            if (i == 1)
                                            {
                                                Console.WriteLine("Zadejte rozměr X vaši matice: ");
                                                y = Int32.Parse(Console.ReadLine());
                                                Console.WriteLine("Zadejte rozměr Y vaši matice: ");
                                                x = Int32.Parse(Console.ReadLine());
                                                Console.WriteLine();
                                            }
                                            scitanec = new Matice(x, y);
                                            scitanec.pole = scitanec.getData();
                                            scitance[i - 1] = scitanec;
                                        }
                                        catch (Exception ex) { Console.WriteLine(ex); break; }
                                    }
                                    else if (klavesa == ConsoleKey.Escape)
                                    { goto END; }
                                    else
                                    {

                                        Console.WriteLine("Vyberte a nebo n");
                                        goto BREAK;
                                    }

                                }
                                if (scitance[0] == null || scitance[1] == null || (scitance[0].x != scitance[1].x && scitance[0].y != scitance[1].y))
                                {
                                    Console.WriteLine("Sčítance musí být o stejném rozměru");
                                    if (Console.ReadKey().Key == ConsoleKey.Escape) { break; }
                                    goto REPEAT;
                                }
                                try
                                {
                                    soucet = scitance[0] + scitance[1];
                                    posledni_vysledky.Add(soucet);
                                    Console.WriteLine("Výsledek je: \n" + soucet.zobrazMatici(soucet.pole));
                                posledni_vysledky.Add(soucet);
                            }
                                catch (Exception ex) { Console.WriteLine(ex); break; }

                            END: break; }
                        case 4: {
                                Matice rozdil = null;
                                Matice[] mensence = new Matice[2];
                                int x = 0, y = 0;
                            REPEAT: for (int i = 1; i <= 2; i++)
                                {
                                BREAK: Console.WriteLine("Odčítání: přejete si načíst {0}.menšenec: a/n ", i.ToString());
                                    Matice mensitel = null;
                                    ConsoleKey klavesa = Console.ReadKey().Key;
                                    if (klavesa == ConsoleKey.A)
                                    {
                                        Console.WriteLine("\nJméno uložené matice: ");
                                        string jmeno = Console.ReadLine();
                                        mensitel = Matice.ziskejUlozenouMatici(jmeno);

                                        if (mensitel == null)
                                        {
                                            Console.WriteLine("Nenalezena žádná matice");
                                            goto BREAK;
                                        }
                                        Console.WriteLine(mensitel.zobrazMatici(mensitel.pole));
                                        mensence[i - 1] = mensitel;
                                    }
                                    else if (klavesa == ConsoleKey.N)
                                    {
                                        try
                                        {

                                            if (i == 1)
                                            {
                                                Console.WriteLine("Zadejte rozměr X vaši matice: ");
                                                y = Int32.Parse(Console.ReadLine());
                                                Console.WriteLine("Zadejte rozměr Y vaši matice: ");
                                                x = Int32.Parse(Console.ReadLine());
                                                Console.WriteLine();
                                            }
                                            mensitel = new Matice(x, y);
                                            mensitel.pole = mensitel.getData();
                                            mensence[i - 1] = mensitel;
                                        }
                                        catch (Exception ex) { Console.WriteLine(ex); break; }
                                    }

                                    else if (klavesa == ConsoleKey.Escape)
                                    { goto END; } else {

                                        Console.WriteLine("Vyberte a nebo n");
                                        goto BREAK;
                                    }

                                }
                                if (mensence[0] == null || mensence[1] == null || (mensence[0].x != mensence[1].x && mensence[0].y != mensence[1].y))
                                {
                                    Console.WriteLine("Menšence musí být o stejném rozměru");
                                    if (Console.ReadKey().Key == ConsoleKey.Escape) { break; }
                                    goto REPEAT;
                                }
                                try
                                {
                                    rozdil = mensence[0] - mensence[1];
                                    posledni_vysledky.Add(rozdil);
                                    Console.WriteLine("Výsledek je: \n" + rozdil.zobrazMatici(rozdil.pole));
                                posledni_vysledky.Add(rozdil);
                            }
                                catch (Exception ex) { Console.WriteLine(ex); break; }


                            END: break; }
                        case 5: {
                                Matice soucin = null;
                                Matice[] cinitele = new Matice[2];
                                int x = 0, y = 0;
                            REPEAT:
                                for (int i = 1; i <= 2; i++)
                                {
                                BREAK: Console.WriteLine("Násobení: přejete si načíst {0}.činitel: a/n ", i.ToString());
                                    Matice cinitel = null;
                                    ConsoleKey klavesa = Console.ReadKey().Key;
                                    if (klavesa == ConsoleKey.A)
                                    {
                                        Console.WriteLine("\nJméno uložené matice: ");
                                        string jmeno = Console.ReadLine();
                                        cinitel = Matice.ziskejUlozenouMatici(jmeno);

                                        if (cinitel == null)
                                        {
                                            Console.WriteLine("Nenalezena žádná matice");
                                            goto BREAK;
                                        }
                                        Console.WriteLine(cinitel.zobrazMatici(cinitel.pole));
                                        cinitele[i - 1] = cinitel;
                                    }
                                    else if (klavesa == ConsoleKey.N)
                                    {
                                        try
                                        {


                                            Console.WriteLine("Zadejte rozměr X vaši matice: ");
                                            y = Int32.Parse(Console.ReadLine());
                                            Console.WriteLine("Zadejte rozměr Y vaši matice: ");
                                            x = Int32.Parse(Console.ReadLine());
                                            Console.WriteLine();

                                            cinitel = new Matice(x, y);
                                            cinitel.pole = cinitel.getData();
                                            cinitele[i - 1] = cinitel;
                                        }
                                        catch (Exception ex) { Console.WriteLine(ex); break; }
                                    }
                                    else if (klavesa == ConsoleKey.Escape)
                                    { goto END; }
                                    else
                                    {

                                        Console.WriteLine("Vyberte a nebo n");
                                        goto BREAK;
                                    }

                                }
                                /*  if (cinitele[0] == null || cinitele[1] == null || (cinitele[0].x != cinitele[1].y))
                                  {
                                      Console.WriteLine("Rozmer x 1.činitele musí být shodný s rozměrem 2.činitele. ");
                                      if (Console.ReadKey().Key == ConsoleKey.Escape) { break; }
                                      goto REPEAT;
                                  }*/
                                try
                                {
                                    soucin = cinitele[0] * cinitele[1];
                                    posledni_vysledky.Add(soucin);
                                    Console.WriteLine("Výsledek je: \n" + soucin.zobrazMatici(soucin.pole));
                                posledni_vysledky.Add(soucin);
                            }
                                catch (Exception ex) { Console.WriteLine(ex); break; }

                            END: break;

                            }
                        case 6: {
                                int x = 0, y = 0;
                            BREAK: Console.WriteLine("Získání submatice z matice: zadejte původní matici:načíst matici?: a/n/p - načíst poslední matici");
                                ConsoleKey klavesa = Console.ReadKey().Key;
                                Matice puvodni_matice = null;
                                Matice submatice = null;
                                if (klavesa == ConsoleKey.A)
                                {
                                    Console.WriteLine("\nJméno uložené matice: ");
                                    string jmeno = Console.ReadLine();
                                    puvodni_matice = Matice.ziskejUlozenouMatici(jmeno);

                                    if (puvodni_matice == null)
                                    {
                                        Console.WriteLine("Nenalezena žádná matice");
                                        goto BREAK;
                                    }
                                    Console.WriteLine(puvodni_matice.zobrazMatici(puvodni_matice.pole));
                                }
                                else if (klavesa == ConsoleKey.N)
                                {
                                    try
                                    {

                                        Console.WriteLine("Zadejte rozměr X vaši matice: ");
                                        y = Int32.Parse(Console.ReadLine());
                                        Console.WriteLine("Zadejte rozměr Y vaši matice: ");
                                        x = Int32.Parse(Console.ReadLine());
                                        Console.WriteLine();

                                        puvodni_matice = new Matice(x, y);
                                        puvodni_matice.pole = puvodni_matice.getData();

                                    }
                                    catch (Exception ex) { Console.WriteLine(ex); break; }
                                }
                            else if (klavesa == ConsoleKey.P)
                            {
                                if (posledni_vysledky.Count == 0) { Console.WriteLine("\nPosledni matice je prazdna."); break; }
                                else
                                {
                                    puvodni_matice = posledni_vysledky.Last();
                                }
                            }
                            else if (klavesa == ConsoleKey.Escape)
                                { goto END; }
                                else
                                {

                                    Console.WriteLine("Vyberte a nebo n");
                                    goto BREAK;
                                }
                                Console.WriteLine("Vyberte způsob výběru submatice:\n0 - vybrat sloupce a vzdálenost zvrchu\n1 - vybrat řádky a sloupce\n2 - vybrat řádky a sloupce, které nebudou v submatici\n");
                                ConsoleKey key = Console.ReadKey().Key;
                                switch (key) {
                                    case ConsoleKey.NumPad0:
                                        try
                                        {

                                            Console.WriteLine("Zadejte sloupce oddelené čárkou:");
                                            string col = Console.ReadLine();
                                            Console.WriteLine("Zadejte vzdálenost zvrchu: ");
                                            int top = int.Parse(Console.ReadLine());
                                            int[] cols = Array.ConvertAll(col.Split(','), int.Parse);
                                            for (int i = 0; i < cols.Length; i++) {
                                                cols[i]--;
                                            }
                                            submatice = new Matice(cols.Length, puvodni_matice.y - top);
                                            submatice.pole = puvodni_matice.getSubmatice(cols, top);
                                            Console.WriteLine("Výsledná submatice: \n" + submatice.zobrazMatici(submatice.pole));
                                        }
                                        catch (Exception e) { Console.WriteLine(e); }
                                        break;


                                    case ConsoleKey.NumPad1:
                                        try
                                        {
                                            Console.WriteLine("Zadejte řádky oddělené čárkou: ");
                                            int[] radky = Array.ConvertAll(Console.ReadLine().Split(','), int.Parse);
                                            Console.WriteLine("Zadejte sloupce oddělené čárkou: ");
                                            int[] sloupce = Array.ConvertAll(Console.ReadLine().Split(','), int.Parse);
                                            int[] puvodni_radky = new int[puvodni_matice.x];
                                            int[] puvodni_sloupce = new int[puvodni_matice.y];
                                            for (int a = 0; a < puvodni_radky.Length; a++) {
                                                puvodni_radky[a] = a;
                                            }
                                            for (int b = 0; b < puvodni_sloupce.Length; b++) {
                                                puvodni_sloupce[b] = b;
                                            }
                                            for (int i = 0; i < radky.Length; i++)
                                            {
                                                radky[i]--;
                                            }
                                            for (int i = 0; i < sloupce.Length; i++)
                                            {
                                                sloupce[i]--;
                                            }

                                        int[] row = Matice.odectiPole(puvodni_radky, radky);
                                            int[] col = Matice.odectiPole(puvodni_sloupce, sloupce);
                                            submatice = new Matice(puvodni_matice.x - sloupce.Length, puvodni_matice.y - radky.Length);
                                            submatice.pole = puvodni_matice.getSubmatice(row, col);
                                            Console.WriteLine(submatice.zobrazMatici(submatice.pole));
                                        }
                                        catch (Exception e) { Console.WriteLine(e); }
                                        break;


                                    case ConsoleKey.NumPad2:
                                        try
                                        {
                                            Console.WriteLine("Zadejte řádky ,které nebudou v submatici,oddělené čárkou: ");
                                            int[] radek = Array.ConvertAll(Console.ReadLine().Split(','), int.Parse);
                                            Console.WriteLine("Zadejte sloupce ,které nebudou v submatici,oddělené čárkou: ");
                                            int[] sloupec = Array.ConvertAll(Console.ReadLine().Split(','), int.Parse);
                                            for (int i = 0; i < radek.Length; i++)
                                            {
                                                radek[i]--;
                                            }
                                            for (int i = 0; i < sloupec.Length; i++)
                                            {
                                                sloupec[i]--;
                                            }
                                            submatice = new Matice(sloupec.Length, radek.Length);
                                            submatice.pole = puvodni_matice.getSubmatice(radek, sloupec);
                                            Console.WriteLine(submatice.zobrazMatici(submatice.pole));
                                        posledni_vysledky.Add(submatice);
                                    }
                                        catch (Exception e) { Console.WriteLine(e); }
                                        break;
                                }
                            END: break; }
                        case 7: {
                                try
                                {
                            SOUSTAVA: Console.WriteLine("Výpočet kořenů soustavy lineárních rovnic o n neznámých: zadejte počet neznámých: ");
                                string vstup = Console.ReadLine();
                                if (ContainsValue(pismena, vstup)) {
                                    Console.WriteLine("N musí být číslo");
                                    goto SOUSTAVA;
                                }
                                int n = int.Parse(vstup);
                                    Console.WriteLine("Zadejte soustavu lineárních rovnic např. 2x+6y-25z=89: ");
                                List<string> list = new List<string>();
                                for (int al = 0; al < n; al++) {
                                    list.Add(Console.ReadLine()); }
                                string[] rce=list.ToArray();
                                    string[,] pole_pismen = new string[n, n];
                                    var Pole = GetLeftAndRightArray(rce, n);
                                    string[,] vlevo = Pole.left;
                                    string[,] vpravo = Pole.right;
                                

                                    for (int w = 0; w < vlevo.GetLength(0); w++)
                                    {
                                        for (int v = 0; v < vlevo.GetLength(1); v++)
                                        {
                                        if (vlevo[w,v]==null) {
                                            // vlevo[w, v] = "0";
                                            Console.WriteLine("Prázdné členy pište ve formátu 0x...");
                                            goto SOUSTAVA;
                                        }
                                            foreach (char aq in pismena)
                                            {
                                                if (vlevo[w, v].Contains(aq))
                                                {
                                                    int index = vlevo[w, v].IndexOf(aq);
                                                    pole_pismen[w, v] = vlevo[w, v].Substring(index, 1);
                                                }
                                            }
                                        }
                                    }//zjistím na kterých pozicích jsou písmena v prvni rovnici
                                    var list_radek = new List<string>();
                                    for (int u = 0; u < pole_pismen.GetLength(0); u++)
                                    {
                                        list_radek.Add(pole_pismen[0, u]);
                                    }

                                    list_radek.Sort(); //prerovnani pismen podle abecedy

                                    var vyraz = "";
                                    List<int> a = new List<int>();
                                    for (int se = 0; se < pole_pismen.GetLength(0); se++)
                                    {
                                        for (int es = 0; es < pole_pismen.GetLength(1); es++)
                                        {
                                            string je = "";
                                            je = list_radek.ElementAt(es);
                                            vyraz = vlevo[se, es];
                                            if (vyraz.Contains(je))
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                for (int x = 0; x < pole_pismen.GetLength(0); x++)
                                                {
                                                    if (pole_pismen[se, x].Contains(je))
                                                    {
                                                        a.Add(x);
                                                        SwapElements(vlevo, se, es, se, x);

                                                    }
                                                }

                                            }
                                        }
                                    }

                                    vlevo = ReplaceBadCharacters(vlevo);
                                    vlevo = DeleteLetters(vlevo);

                                    Matice matice = new Matice(n, n + 1);
                                    double[,] left = ConvertAll(vlevo, Double.Parse);
                                    double[,] right = ConvertAll(vpravo, Double.Parse);

                                    for (int ah = 0; ah < n; ah++)
                                    {
                                        for (int j = 0; j < left.GetLength(0); j++)
                                            matice.pole[ah, j] = left[j, ah];

                                        for (int y = 0; y < right.GetLength(0); y++)
                                            matice.pole[ah, left.GetLength(0)] = right[0, ah];
                                    }
                                    //spojeni do jednoho pole vlevo a vpravo
                                    Console.WriteLine("Matice soustavy rovnic: \n"+matice.zobrazMatici(matice.pole));
                                    double[] roots = matice.getRoots(matice);

                                    for (int ol = 0; ol < roots.Length; ol++)
                                    {
                                        Console.WriteLine("Koren {0}: {1}", ol + 1, roots[ol]);
                                    }

                                }
                                catch (Exception e) { Console.WriteLine(e); }
                           END: break;
                            }

                        
                        case 8: {
                                Matice[] pole_matic = Matice.ziskejVsechnyMatice();
                                foreach (Matice mat in pole_matic) {
                                    Console.WriteLine("{0}, x:{1},y:{2}\n{3}", mat.name, mat.x, mat.y, mat.zobrazMatici(mat.pole));
                                }
                                break; }
                        default: {
                                Console.WriteLine("Vyberte z jednoho z moznosti ");
                                break; }

                    }
                    Console.ReadKey();
                } }
        }
    }
    
