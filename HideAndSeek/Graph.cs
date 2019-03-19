using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edge = Microsoft.Msagl.Drawing.Edge;

namespace HideAndSeek
{
    public class Graph
    {
        public int n_house;
        public List<int>[] way = new List<int>[100008];
        public int[] level = new int[100008];
        public bool[] visited = new bool[100008];
        public int[] history = new int[100008];
        public bool found;
        public List<int> finalPath;

        public void drawGraph()
        {
            //create a form 
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content 
            for (int i = 1; i <= n_house; i++)
            {
                for (int j = 0; j < way[i].Count; j++)
                {
                    if (way[i][j] > i)
                    {
                        Edge edge = (Edge)
                        graph.AddEdge("" + i, "" + way[i][j]);
                        edge.Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                    }
                    if (visited[i])
                        if (found)
                            graph.FindNode("" + i).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
                        else
                            graph.FindNode("" + i).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
                }
            }
            viewer.Graph = graph;
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form 
            form.ShowDialog();
            //System.Threading.Thread.Sleep(3000);
        }

        public void getInput(string filePath)
        {
            //Local Variable 
            int a, b;
            //File Eksternal
            string[] inputMap = System.IO.File.ReadAllLines(@filePath);
            //Get number of house
            n_house = inputMap.Length;
            //Initialize Array Of List With Size Of n_house
            for (int i = 0; i < n_house + 5; i++)
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
            }
        }

        public void initialize()
        {
            for (int i = 1; i <= n_house; i++)
            {
                level[i] = -1;
            }
        }

        public void getLevel(int x, int l)
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

        public void makeUnvisited()
        {
            for (int i = 1; i <= n_house; i++)
            {
                visited[i] = false;
                history[i] = -1;
            }
            found = false;
        }

        public bool dfsCari(int f, int j, bool showGraph)
        {

            int next;
            bool isCan, drawagain;
            if (f == j)
            {
                found = true;
                if(showGraph) drawGraph();
                visited[f] = false;
                return true;
            }
            else
            {
                if(showGraph) drawGraph();
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
                        isCan = isCan || dfsCari(next, j, showGraph);
                    }
                }
                if (drawagain && !found && showGraph) drawGraph();
                visited[f] = false;
                return isCan;
            }
        }

        public void findFinalPath(int x)
        {
            if (history[x] == -1)
            {
                finalPath.Add(x);
            }
            else
            {
                findFinalPath(history[x]);
                finalPath.Add(x);
            }
        }

        public bool answerQuery(string query, bool showGraph)
        {
            int n, a, jose, ferdian;
            finalPath = new List<int>();
            string[] tokens = query.Split(); //Read line, and split it by whitespace into an array of strings
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
                //Console.WriteLine("Tidak Bisa");
                bool hasil = dfsCari(ferdian, jose, showGraph);
                for (int j = 1; j <= n_house; j++)
                {
                    level[j] *= -1;
                }
                return hasil;
            }
            else
            {
                makeUnvisited();
                visited[ferdian] = true;
                return (dfsCari(ferdian, jose, showGraph));
            }
        }
    }
}
