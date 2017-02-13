using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ConsoleApplication3
{
    public class Matice
    {
        public const string XMLDokument= "matice.xml"; 
        public Random rnd = new Random();
        public double[,] pole;
        public int x, y;
        public string name="Matice";
        public int rozmer;
        public int pocitadlo = 0;
        public Matice(int m, int n) //obdelníková matice
        {
            pole = new double[m, n];
            if (m == n) rozmer = m;
            x = m;
            y = n;
        }
        public Matice(int i) //čtvercová matice
        {
            pole = new double[i, i];
            rozmer = i;
            x = i;
            y = i;
        }
        public void doplnCisla(int from, int to) //doplnění random čísel v rozsahu od from do to
        {
            for (int c = 0; c < pole.GetLength(0); c++)
            {
                for (int l = 0; l < pole.GetLength(1); l++)
                {
                    pole[c, l] = rnd.Next(from, to);
                }
            }
        }
        public static Matice ziskejUlozenouMatici(string jmeno) {
            Matice matice=null;
            int u, v;
            try {
                XmlDocument doc = new XmlDocument();
                doc.Load(XMLDokument);
                string pattern= "/seznam_matic/matice[@name='"+jmeno+"']";
                XmlNodeList nodes = doc.SelectNodes(pattern);
                double[,] pole = null;

                if (nodes.Count == 1)
                {
                    //string a = nodes.Item(0).Attributes.Item(1).Value;
                    u =int.Parse(nodes.Item(0).Attributes.Item(1).Value);
                   // Console.WriteLine(u.ToString());
                    v =int.Parse(nodes.Item(0).Attributes.Item(2).Value);
                    matice = new Matice(u, v);
                   
                    pole = new double[u, v];
                    XmlNodeList cisla = doc.SelectNodes(pattern+"/cislo");
                    foreach (XmlNode uzel in cisla) {
                        int i = int.Parse(uzel.Attributes.Item(0).Value);
                        int j = int.Parse(uzel.Attributes.Item(1).Value);
                        pole[i, j] = double.Parse(uzel.InnerText);
                    }
                    matice.pole = pole;
                }
                else if (nodes.Count > 1) { throw new Exception("Vice matic odpovídá jednomu jménu"); }
                else { matice = null;
                    
                }
                
            } catch (Exception e) { Console.WriteLine(e); }
            return matice;
        }
        public static Matice[] ziskejVsechnyMatice()
        {
            List<Matice> list = new List<Matice>();
            
            
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(XMLDokument);
                string pattern = "/seznam_matic/matice";
                XmlNodeList nodes = doc.SelectNodes(pattern);
                double[,] pole = null;
                for (int i = 0; i < nodes.Count; i++) {
                    string name=nodes.Item(i).Attributes.Item(0).Value;
                    int x= int.Parse(nodes.Item(i).Attributes.Item(1).Value);
                    int y= int.Parse(nodes.Item(i).Attributes.Item(2).Value);

                    Matice matice = new Matice(x,y);
                    matice.name = name;
                    double[,] arr = new double[x, y];
                    XmlNodeList cisla = doc.SelectNodes(pattern);
                    cisla=cisla.Item(i).ChildNodes;
                    foreach (XmlNode uzel in cisla)
                    {
                        int k = int.Parse(uzel.Attributes.Item(1).Value);
                        int j = int.Parse(uzel.Attributes.Item(0).Value);
                        arr[j, k] = double.Parse(uzel.InnerText);
                    }
                    matice.pole = arr;
                    list.Add(matice);
                }

               

            }
            catch (Exception e) { Console.WriteLine(e); }
            return list.ToArray();
        }
        public void ulozMatici(string jmeno) {
            
            if (!File.Exists(XMLDokument)) {
                using (XmlWriter writer = XmlWriter.Create(XMLDokument)) {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("seznam_matic");
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();
                } }
            try
            {
                XElement xml = XElement.Load(XMLDokument);
                if (xml.Elements("matice").Attributes("name").ToString() == jmeno)
                {
                   Console.WriteLine("Zadaný název matice již existuje,zadejte jiný název:");
                   ulozMatici(Console.ReadLine());
                }
                else
                {
                    xml.Add(new XElement("matice", new XAttribute("name", jmeno),
                    new XAttribute("x", y), new XAttribute("y", x)));
                    for (int a = 0; a < pole.GetLength(0); a++)
                    {
                        for (int o = 0; o < pole.GetLength(1); o++)

                        {
                            var element = new XElement("cislo", new XAttribute("x", a), new XAttribute("y", o), pole[a, o]);
                            xml.Elements("matice").Last().Add(element);
                        }
                    }
                    xml.Save(XMLDokument);
                    Console.WriteLine("Matice uložena");
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
           
        }
        public double[,] getData() {
            double[,] pole = new double[x,y];
            Console.WriteLine("Zadejte matici "+y+"x"+x+": ");
            try
            {
                for (int i = 0; i <= x - 1; i++)
                {
                    string vstup = Console.ReadLine();
                    char znak = ' ';
                    for (int k = 0; k <= y - 1; k++)
                    {
                        pole[i, k] = double.Parse(vstup.Split(znak)[k]);
                    }

                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            return pole;
        }
        public double[] getRoots(Matice matice)
        {
            double[] roots = null;
            try
            {
                if (matice.y == matice.x +1 )
                {
                    int x = matice.y;
                    int y = matice.x; int n = Math.Min(x, y);
                    double[,] sub = matice.getSubmatice(new int[0] { }, new int[1] { y - 1 });
                    double detA = matice.getDeterminant(sub, n);
                    Matice A = new Matice(y);
                    Matice b = new Matice(y,1);
                    int[] sloupce = new int[y];
                    for (int i = 0; i < y; i++)
                    {
                        sloupce[i] = i;
                    }
                    A.pole = matice.getSubmatice(sloupce, 0);
                    b.pole = matice.getSubmatice(new int[1] { y }, 0);
                    // Console.WriteLine(A.zobrazMatici(A.pole)+"\n" +b.zobrazMatici(b.pole));//
                    Matice inverseA = A.getInverse(A);
                    //Console.WriteLine(inverseA.zobrazMatici(inverseA.pole));
                    Matice koreny = inverseA * b;
                    // Console.WriteLine(koreny.zobrazMatici(koreny.pole));
                    //  roots =null;
                    roots = new double[koreny.pole.GetLength(0)];
                    
                        for (int e = 0; e < koreny.pole.GetLength(0); e++)
                            roots[e] = koreny.pole[e, 0];

                    
                }
                else
                {
                    roots = null;
                    throw new ArgumentOutOfRangeException("matice musi byt o 1 prvek sirsi nez vyska matice");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return roots;
        }
        public string[] DoubleToStringArr(double[] arr)
        {
            string[] str = Array.ConvertAll(arr, element => element.ToString());
            return str;
        }
        public double[] StringToDoubleArr(string[] arr)
        {
            double[] str = Array.ConvertAll(arr, element => Double.Parse(element));
            return str;
        }
        public string vypisPole(Array pole,string oddelovac)
        {
            string result="";
            if (pole != null) {
                for (int a = 0; a < pole.Length; a++)
                {
                    result+=pole.GetValue(a)+oddelovac;
                } }
            else {
                Console.WriteLine("Pole je nulové");

            }
            return result;
        }
        public string zobrazMatici(double[,] k)
        { //vrátí string matice s daty k
            string vysledek = "";
            // int pocetnul = 0;
            try
            {
                for (int j = 0; j < k.GetLength(0); j++)
                {
                    for (int i = 0; i < k.GetLength(1); i++)
                    {
                        vysledek += k[j, i] + " ";
                    }
                    vysledek += "\n";
                }


            }
            catch (Exception e) { Console.WriteLine(e+"s"); }

            return vysledek;


        }
        public void doplnNuly()
        {
            doplnCisla(0, 0);
        }
        public Matice getInverse(Matice matice)
        {
            Matice vysledek = null;
            int x, y;
            x = matice.x;
            y = matice.y;
            if (x == y)
            {
                vysledek = new Matice(x, y);
                vysledek.doplnNuly();
                for (int i = 0; i < matice.pole.GetLength(0); i++)
                {
                    for (int j = 0; j < matice.pole.GetLength(1); j++)
                    {
                        double[,] submatice = matice.getSubmatice(new int[1] { i }, new int[1] { j });

                vysledek.pole[j, i] =(Math.Pow(-1, (i+1) + (j+1)) * getDeterminant(submatice,x-1))/getDeterminant(matice.pole,x);

                    }
                }

            }
            else
            {

                throw new ArgumentException("Zadaná matice musí být čtvercová.");
            }
            return vysledek;
        }
        public static Matice operator +(Matice matice1, Matice matice2)
        { //sečte dve matice matice1+matice2
            double[,] vysledek = new double[matice1.x, matice1.y];
            Matice matice3 = new Matice(matice1.x, matice1.y);
            if (matice1.x == matice2.x && matice1.y == matice2.y)
            {
                for (int c = 0; c < matice1.x; c++)
                {
                    for (int l = 0; l < matice1.y; l++)
                    {

                        vysledek[c, l] = (Int32)matice1.pole[c, l] + (Int32)matice2.pole[c, l];
                    }
                }
               
                matice3.pole = vysledek;
                
            }
            else { matice3 = null; }
            return matice3;
        }
        public static Matice operator *(Matice A, Matice B)
        {
            Matice C = null;
            double[,] pole1 = A.pole;
            double[,] pole2 = B.pole;
            int n = Convert.ToInt32(Math.Max(Math.Max(Convert.ToSByte(A.x), Convert.ToSByte(A.y)), Math.Max(Convert.ToSByte(B.x), Convert.ToSByte(B.y))));
            int m = Convert.ToInt32(Math.Min(Math.Min(Convert.ToSByte(A.x), Convert.ToSByte(A.y)), Math.Min(Convert.ToSByte(B.x), Convert.ToSByte(B.y))));
            int n1, m1, n2, m2;
            if (A.x == A.y) { n1 = A.x; m1 = A.x; }
            else { n1 = A.x; m1 = A.y; }
            if (B.x == B.y) { n2 = B.x; m2 = B.x; }
            else
            {
                n2 = B.x; m2 = B.y;
            }

            if (A.y == B.x && A.x != B.y) //
            {

                C = new Matice(n, m);
                double[,] pole = C.pole;
                for (int l = 0; l < n; l++)
                {
                   
                        for (int j = 0; j < n; j++)
                        {
                            pole[l, 0] = pole[l, 0] + pole1[l, j] * pole2[j, 0];
                        }
                    
                }
                C.pole = pole;

            }
            else if (A.x == B.y && A.y == B.x)
            {
                C = new Matice(m);
                double[,] pole = C.pole;

                for (int l = 0; l < m; l++)
                {
                    for (int k = 0; k < m; k++)
                    {
                        for (int j = 0; j < m; j++)
                        {

                                pole[l, k] = pole[l, k] + pole1[l, j] * pole2[j, k];
                            
                        }
                    }
                }
                C.pole = pole;

            }

            else
            {
                C = new Matice(m);
                C.doplnNuly();
                Console.WriteLine("Matice A musí mít stejný počet sloupců jak matice B řádků,a zároveň matice A musí mít stejný počet řádků jak matice B.");
            }

            return C;
        }
        public static Matice operator -(Matice matice1, Matice matice2) //odečte dve matice matice1-matice2
        {
            double[,] vysledek = new double[matice1.x, matice1.y];
            Matice matice3 = new Matice(matice1.x, matice1.y);
            if (matice1.x == matice2.x && matice1.y == matice2.y)
            {
                for (int c = 0; c < matice1.x; c++)
                {
                    for (int l = 0; l < matice1.y; l++)
                    {

                        vysledek[c, l] = (Int32)matice1.pole[c, l] - (Int32)matice2.pole[c, l];
                    }
                }

                matice3.pole = vysledek;

            }
            else { matice3 = null; }
            return matice3;
        }
        public double getDeterminant(double[,] matice, int rozmer) //získá determinant matice s daty matice a rozměrem rozmer
        {
            int velikost = rozmer;
            double det = 0;
            if (velikost < 1)
            {
               Console.WriteLine("Matice se zadaným rozměrem neexistuje");
                det = Double.NaN;
                goto BREAK;
            }
            else if (velikost == 1)
            {
                det = matice[0, 0];
            }
            else if (velikost == 2)
            {
                det = matice[0, 0] * matice[1, 1] - matice[0, 1] * matice[1, 0];
            }
            else
            {
                det = 0;
                int aRow = 1;
                for (int i = 0; i < velikost; i++)
                {
                    double[,] minor = new double[velikost - 1, velikost - 1];
                    for (int s = 1; s < velikost; s++)
                    {
                        int j2 = 0;
                        for (int j = 0; j < velikost; j++)
                        {
                            if (j == i)
                                continue;
                            minor[s - 1, j2] = matice[s, j];
                            j2++;
                        }
                    }
                    det += (int)Math.Pow(-1, aRow + i + 1) * matice[aRow - 1, i] * getDeterminant(minor, velikost - 1);
                    pocitadlo++;

                }
            }

            BREAK: return det;
        }
        public  double[,] getSubmatice(int[] sloupce, int top)
        { //získá submatici ve formatu pole double, argumenty: pole s pořadím sloupců,které chceme získat(od indexu 0 )
            int m = sloupce.Length;//sirka
            int n = this.pole.GetLength(0)-top;//vyska
            int[,] submatice = new int[n, m];
            double[,] sloupec = new double[n, m];//vysledek
            try
            {
                for (int i = 0; i < n ; i++)
                {
                    if (top + m - 1 >= pole.GetLength(1))
                    {
                        throw new System.IndexOutOfRangeException("Špatně zadaná submatice");
                    }
                    else
                    {
                        for (int q = 0; q < m; q++)
                        {


                            sloupec[i, q] = pole[i + top,sloupce[q]];

                        }
                    }
                }
            }
            catch (System.IndexOutOfRangeException message)
            {
                string sl = "";
                foreach (int cislo in sloupce)
                {
                    sl += cislo.ToString();
                    sl += " , ";
                }
                Console.WriteLine("Chyba: {0} , paramtery: top: {1} , sloupce: {2}", message, top.ToString(), sl);
            }
            return sloupec;

        }
        public double[,] getSubmatice(int[] chybejici_radky, int[] chybejici_sloupce)
        {
            int x = pole.GetLength(0);
            int y = pole.GetLength(1);
            double[,] arr=new double[x - chybejici_radky.Length, y - chybejici_sloupce.Length];
            // arr = null;
            int[] radky;
            int[] sloupce;
            radky = new int[x];
            sloupce = new int[y];
            for (int o = 0; o <x ; o++)
            {
                radky[o] = o;
            }
            for (int p = 0; p < y ; p++)
            {
                sloupce[p] = p;
            }

            for (int i = 0; i < chybejici_radky.Length; i++) {
                if (radky.Contains(chybejici_radky[i])) {
                    List<int> list = radky.ToList();
                    list.Remove(chybejici_radky[i]);
                    radky=list.ToArray();
                }
                
            }
            for (int j = 0; j <chybejici_sloupce.Length; j++) {
                if (sloupce.Contains(chybejici_sloupce[j]))
                {
                    List<int> list = sloupce.ToList();
                    list.Remove(chybejici_sloupce[j]);
                    sloupce = list.ToArray();
                }
            }

            for (int k = 0; k < radky.Length; k++)
            {

                for (int l = 0; l < sloupce.Length; l++)
                {
                    arr[k, l] = pole[radky[k], sloupce[l]];
                }
            }
                return arr;
        }
        public static int[] odectiPole(int[] pole1,int[] pole2) {

            int[] pole3 = pole1;
            foreach (int i in pole2) {
                if (pole3.Contains(i)) {
                    var a = pole1.Except(pole2);
                 pole3=a.ToArray();
                    
                }
            }
            return pole3;
        }
    }

    }

