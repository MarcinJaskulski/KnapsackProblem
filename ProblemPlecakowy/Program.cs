using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProblemPlecakowy
{
    class Program
    {
        static void Generate(string path) // generowanie przedmiotów 
        {
            int C = 10, n = 10; // Pojemność plecaka; ilość elementów
            int p, w; // pojemność; wartość generowanych elementów

            StreamWriter sw = new StreamWriter(path);

            sw.WriteLine(C); 
            sw.WriteLine(n);

            Random rand = new Random();

            for(int i =0; i< n; i++)
            {
                p = rand.Next(1, 11); // generowanie p i w zakresie 1 - 10 
                w = rand.Next(1, 11);
                sw.WriteLine(p + " " + w);

            }
            sw.Close();
        }

        static void ReadTab(string path, out int[,] tab) // wczytywanie danych do tablicy
        {
            int C, n, counterA = 1, counterB = 0; // counterA -> iteracja po tablicy przedmiotów; counterB -> iteracja po p i w
            string s = null;
            string[] words;

            StreamReader sr = new StreamReader(path);

            if (File.Exists(path))
            {

                C = Int32.Parse(sr.ReadLine());
                n = Int32.Parse(sr.ReadLine());
                tab = new int[n+1, 2]; // +1 bo w 0 kolumnie C i n

                tab[0, 0] = C+1; // zerowa kolumna zawiera C i n 
                tab[0, 1] = n+1;

                do
                {
                    s = sr.ReadLine();
                    if (s == null) break;
                    words = s.Split(' '); // rozdzielenie danych ze spacją

                    foreach (string w in words)
                    {
                        tab[counterA, counterB] = Int32.Parse(w);
                        counterB = 1;
                    }
                    counterB = 0;
                    counterA++;

                } while (s != null);
            }
            else
            {
                Console.WriteLine("Blad \n");
                tab = new int[0,0]; // bo inaczej będzie błąd, że nie zainicjalizowałem tabicy
            }

            //for(int i=0; i<tab[0,1]; i++)
            //{
            //    for(int j=0; j<2; j++)
            //    {
            //        Console.Write(tab[i, j] + " " );
            //    }
            //    Console.Write("\n");
            //}

            sr.Close();
        }

        static void CreativeVTab(ref int[,] tab, ref int[,] V_tab) // generowanie macierzy z wartościami z wzoru Bellmana
        {
            for (int i = 0; i <tab[0, 0]; i++) V_tab[i, 0] = 0; // nadanie wierszowi 0 wartości 0
            for (int i = 0; i <tab[0, 1]; i++) V_tab[0, i] = 0; // nadanie kolumnie 0 wartości 0

            // uzupełnianie każdego po kolei korzystająć ze zworu Bellmana
            for (int i = 1; i <tab[0, 1]; i++)
            {
                for(int j = 1; j <tab[0,0]; j++)
                {
                    if (j < tab[i, 1])
                    {
                        V_tab[j, i] = V_tab[j, i - 1];
                    }
                    else if( j >= tab[i, 1])
                    {
                        V_tab[j, i] = Math.Max(V_tab[j, i - 1], V_tab[j - tab[i, 1], i - 1] + tab[i,0]);
                    }

                    Console.Write(V_tab[j, i] + " ");
                }
                Console.Write("\n");
            }
            Console.Write("\n");

        }

        static void ZrobieToW5minut(ref int[,] tab, ref int[,] V_tab) // jednak w 8 ... + szukanie przedmiotów do plecaka
        {
            List<int> result = new List<int>();
            int x = tab[0, 0]-1;
            int y = tab[0, 1]-1;

            // Wzór Bellmana
            while (y > 0)
            {
                if (V_tab[x, y] == V_tab[x, y - 1])
                {
                    y--;
                }
                else
                {
                    Console.Write(y + " " );
                    result.Add(y);

                    x -= tab[y, 1];
                    y--;
                }
            }
        }

        static void BruteForce(ref int[,] tab)
        {
           
        }



        static void Main(string[] args)
        {
            string path = @"dane.txt";
            //Generate(path); // w tej funckji należy zmenić parametry, aby generować więcej, bądź inaczej

            int[,] tab; // [0,0] zawiera C; [0,1] zawiera n . Bardzo ważne, bo tablica nie jest od <0,n>, tylko <1, n+1>
            ReadTab(path, out tab);

            int[,] V_tab = new int[tab[0,0]+1, tab[0,1]+1]; // inicjacja tablicy w której będa zapisywane wynikie ze wzorów Bellmana

            CreativeVTab(ref tab, ref V_tab); // tworzenie macierzy z wartościami ze wzoru Bellmana

            ZrobieToW5minut(ref tab, ref V_tab); // szukanie przedmiotów do plecaka


            Console.Read();
        }
    }
}
