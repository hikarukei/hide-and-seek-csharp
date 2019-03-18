using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Stima
{
    class MainClass
    {
        public static int n_house;
        public static List<int>[] way = new List<int>[100];
        public static int[] level = new int[100];
        public static bool[] visited = new bool[100];
        public static int[] history = new int[100];
        public static bool found; 
        public static void drawGraph() {
            //create a form 
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content 
            for (int i = 1; i <= n_house; i++) {
                for (int j = 0; j < way[i].Count; j++) {
                    if (way[i][j] > i) {
                        graph.AddEdge("" + i, "" + way[i][j]);
                       
                    }
                    if (visited[i]) 
                        if(found)
                            graph.FindNode("" + i).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
                        else
                            graph.FindNode("" + i).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
                }
            }
    

    /* graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
            graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
            graph.FindNode("B").Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;


            Microsoft.Msagl.Drawing.Node c = graph.FindNode("C");
        
            c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
            c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
            //bind the graph to the viewer */
            viewer.Graph = graph;
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form 
            form.ShowDialog();
            //System.Threading.Thread.Sleep(3000);
            form.Close();
        }
        public static void getInput()
        {
            //Local Variable 
            int a, b;
            //Get number of house
            Console.Write("Masukkan jumlah rumah : ");
            n_house = Convert.ToInt32(Console.ReadLine());
            //Initialize Array Of List With Size Of n_house
            for (int i = 0; i < n_house + 5; i++)
            {
                way[i] = new List<int>();
            }
            //Get Way
            for (int i = 1; i < n_house; i++)
            {
                string[] tokens = Console.ReadLine().Split(); //Read line, and split it by whitespace into an array of strings
                a = int.Parse(tokens[0]); //Parse element a
                b = int.Parse(tokens[1]); //Parse element b
                way[a].Add(b);
                way[b].Add(a);
            }
        }
        public static void initialize()
        {
            for (int i = 1; i <= n_house; i++)
            {
                level[i] = -1;
            }
        }
        public static void getLevel(int x, int l)
        {
            int next;
            visited[x] = true;
            level[x] = l;
            for (int i = 0; i < way[x].Count; i++)
            {
                next = way[x][i];
                if (!visited[next])
                {
                    Console.WriteLine("{0}", way[x][i]);
                    getLevel(next, l + 1);
                }
            }
        }
        public static void makeUnvisited()
        {
            for (int i = 1; i <= n_house; i++)
            {
                visited[i] = false;
                history[i] = -1;
            }
            found = false;
        }
        public static bool dfsCari(int f, int j)
        {
            
            int next;
            bool isCan,drawagain;
            if (f == j)
            {
                found = true;
                drawGraph();
                visited[f] = false;
                return true;
            }
            else
            {
                drawGraph();
                isCan = false;
                drawagain = false;
                for (int i = 0; i < way[f].Count; i++)
                {
                    next = way[f][i];
                    drawagain = false;
                    if (!visited[next])
                    {
                        history[next] = f;
                    }
                    if (!visited[next] && level[next] > level[f])
                    {
                        drawagain = true;
                        visited[next] = true;
                        isCan = isCan || dfsCari(next, j);
                    }
                }
               if(drawagain && !found) drawGraph();
                visited[f] = false;
                return isCan;
            }
        }
        public static void printHistory(int x)
        {
            if (history[x] == -1)
            {
                Console.Write("{0} ", x);
            }
            else
            {
                printHistory(history[x]);
                Console.Write("{0} ", x);
            }
        }
        public static void answerQuery()
        {
            int n, a, jose, ferdian;
            Console.Write("Masukkan jumlah query : ");
            n = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                string[] tokens = Console.ReadLine().Split(); //Read line, and split it by whitespace into an array of strings
                a = int.Parse(tokens[0]); //Parse element a
                jose = int.Parse(tokens[1]); //Parse element y
                ferdian = int.Parse(tokens[2]);
                if (a == 0)
                {
                    for (int j = 1; j <= n_house; j++)
                    {
                        level[j] *= -1;
                    }
                    makeUnvisited();
                    visited[ferdian] = true;
                    if (dfsCari(ferdian, jose))
                    {
                        Console.WriteLine("Bisa");
                        
                        printHistory(jose);
                        Console.WriteLine("");
                    }
                    else
                        Console.WriteLine("Tidak Bisa");
                    for (int j = 1; j <= n_house; j++)
                    {
                        level[j] *= -1;
                    }
                }
                else
                {
                    makeUnvisited();
                    visited[ferdian] = true;
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
            drawGraph();
            makeUnvisited();
            getLevel(1, 1);
            answerQuery();
        }
    }
}
