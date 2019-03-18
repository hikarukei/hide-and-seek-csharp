using System;
using System.Collections.Generic;

namespace Stima
{
    class MainClass
    {
        public static int n_house;
        public static List<int>[] way = new List<int>[100];
        public static int[] level = new int[100];
        public static bool[] visited = new bool[100];
        public static int[] history = new int[100];
        public static void getInput()
        {
            //Local Variable 
            int a, b;
            //File Eksternal
            string [] inputMap = System.IO.File.ReadAllLines(@"/home/hanif/Projects/Stima/Stima/inMap.txt");
            //Get number of house
            n_house = inputMap.Length;
            //Initialize Array Of List With Size Of n_house
            for(int i = 0; i < n_house+5; i++)
            {
                way[i] = new List<int>();
            }
            //Get Way
            for (int i = 1; i < n_house; i++)
            {
                string[] tokens = inputMap[i].Split(); //Read line, and split it by whitespace into an array of strings
                a = int.Parse(tokens[0]); //Parse element a
                b = int.Parse(tokens[1]); //Parse element b
                way[a].Add(b);
                way[b].Add(a);
                Console.WriteLine(a + " " + b);
            }
        }
        public static void initialize()
        {
            for(int i = 1; i <= n_house; i++)
            {
                level[i] = -1;
            }
        }
        public static void getLevel(int x,int l)
        {
            int next;
            visited[x] = true;
            level[x] = l;
            for (int i = 0; i < way[x].Count; i++)
            {
                next = way[x][i];
                if (!visited[next])
                {
                    getLevel(next, l + 1);
                }
            }
        }
        public static void makeUnvisited()
        {
            for(int i = 1; i <= n_house; i++)
            {
                visited[i] = false;
                history[i] = -1;
            }
        }
        public static bool dfsCari(int f,int j)
        {
            int next;
            bool isCan;
            if(f == j)
            {
                return true;
            }
            else
            {
                visited[f] = true;
                isCan = false;
                for(int i = 0; i < way[f].Count; i++)
                {
                    next = way[f][i];
                    if (!visited[next])
                    {
                        history[next] = f;
                    }
                    if (!visited[next] && level[next] > level[f])
                        isCan = isCan || dfsCari(next, j);                        
                }
                return isCan;
            }
        }
        public static void printHistory(int x)
        {
            if(history[x] == -1)
            {
                Console.Write("{0} ", x);
            }
            else
            {
                printHistory(history[x]);
                Console.Write("{0} ",x);
            }
        }
        public static void answerQuery()
        {
            int n,a,jose,ferdian;
            string[] inputQuery = System.IO.File.ReadAllLines(@"/home/hanif/Projects/Stima/Stima/inQuery.txt");
            n = inputQuery.Length-1;
            for(int i = 1; i <= n; i++)
            {
                string[] tokens = inputQuery[i].Split(); //Read line, and split it by whitespace into an array of strings
                a = int.Parse(tokens[0]); //Parse element a
                jose = int.Parse(tokens[1]); //Parse element y
                ferdian = int.Parse(tokens[2]);
                if(a == 0)
                {
                    for(int j = 1; j <= n_house; j++)
                    {
                        level[j] *= -1;
                    }
                    makeUnvisited();
                    if (dfsCari(ferdian, jose))
                    {
                        Console.WriteLine("Bisa");
                        printHistory(jose);
                        Console.WriteLine("");
                    } else
                        Console.WriteLine("Tidak Bisa");
                    for (int j = 1; j <= n_house; j++)
                    {
                        level[j] *= -1;
                    }
                }
                else
                {
                    makeUnvisited();
                    if (dfsCari(ferdian, jose))
                    {
                        Console.WriteLine("Bisa");
                        printHistory(jose);
                        Console.WriteLine("");
                        Console.WriteLine("");
                    }
                    else
                        Console.WriteLine("Tidak Bisa");
                }
            }
        }
        public static void Main(string[] args)
        {
            getInput();
            initialize();
            makeUnvisited();
            getLevel(1,1);
            answerQuery();
        }
    }
}
