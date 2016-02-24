using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Graphics graph;
        Random rnd = new Random();
        List<Points> pointList = new List<Points>();
        List<Points> classList = new List<Points>();
        List<Points>[] pointInClassList = new List<Points>[20];

        public Form1()
        {
            InitializeComponent();
            graph = pictureBox1.CreateGraphics();
        }

        private void RandomPoints(Point[] p, int obj)
        {
            for (int i = 0; i < obj; i++)
                p[i] = new Point(rnd.Next(0, pictureBox1.Width), rnd.Next(0, pictureBox1.Height));
        }

        private void FillK(Points[] k)
        {
            int ind = 0;
            foreach (var temp in classList)
            {
                k[ind] = temp;
                ind++;
            }
        }

        private double EnterPointLenght(Point point1, Point point2)
        {
            double Lenght;
            var katets = new double[2];
            katets[0] = Math.Abs(point1.Y - point2.Y);
            katets[1] = Math.Abs(point1.X - point2.X);
            Lenght = Math.Sqrt(Math.Pow(katets[0], 2) + Math.Pow(katets[1], 2));
            return Lenght;
        }

        private void RecountKCenter(int n, Points[] k)
        {
            float sumOfElemX, sumOfElemY, sum;
            for (int i = 0; i < n; i++)
            {
                sum = sumOfElemX = sumOfElemY = 0;
                foreach (var temp in pointInClassList[i])
                {
                    sum++;
                    sumOfElemX += temp.X;
                    sumOfElemY += temp.Y;
                }
                k[i].X = (int)(sumOfElemX / sum);
                k[i].Y = (int)(sumOfElemY / sum);
            }
        }

        int ind;

        private void ReplacePoints(int numberOfClasses, int screenWidth, int screenHeight, Graphics graph, Points[] k)
        {
            double minDistance;
            Points point_0 = new Points(0, 0);
            ind = 0;
            foreach (var temp in classList)
            {
                if (pointInClassList[ind] != null)
                    pointInClassList[ind].Clear();
                pointInClassList[ind] = new List<Points>();
                ind++;
            }
            ind = 1;
            foreach (var temp in pointList)
            {
                minDistance = Math.Sqrt((Math.Pow(screenWidth, 2) + Math.Pow(screenHeight, 2)));
                for (int i = 0; i < numberOfClasses; i++)
                {
                    if (Math.Sqrt(Math.Pow((temp.X - k[i].X), 2) + Math.Pow((temp.Y - k[i].Y), 2)) <= minDistance)
                    {
                        minDistance = Math.Sqrt(Math.Pow((temp.X - k[i].X), 2) + Math.Pow((temp.Y - k[i].Y), 2));
                        point_0 = k[i];
                        ind = i;
                    }
                }
                pointInClassList[ind].Add(temp);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int numberOfObjects = int.Parse(this.numericUpDown2.Text);
            int numberOfClasses = 0;

            var brush = new SolidBrush(Color.Black);
            bool notFinished = true;
            var pt1 = new Point(0, 0);
            var pt2 = new Point(0, 0);
            int count = 0;

            Graphics g = this.pictureBox1.CreateGraphics();
            int solution = 0;
            int i, j, kInd = 0;
            Point[] point = new Point[numberOfObjects];
            Point[] core = new Point[1];
            var random = new Random();
            int lenghtCore = core.Length;

            graph.FillRectangle(brush, 0, 0, this.pictureBox1.Width, this.pictureBox1.Height);

            pointList.Clear();
            classList.Clear();

            RandomPoints(point, numberOfObjects);

            core[0] = point[rnd.Next(0, numberOfObjects - 1)];

            int index = 0;
            double max = 0;
            for (i = 0; i < point.Length; i++)
            {
                double len;

                len = EnterPointLenght(core[0], point[i]);
                if (len > max)
                {
                    max = len;
                    index = i;
                }
                if (i == (numberOfObjects - 1))
                {
                    lenghtCore += 1;
                    Array.Resize(ref core, lenghtCore);
                    core[1] = point[index];
                }
            }

            var masColor = new SolidBrush[50];
            masColor[0] = new SolidBrush(Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
            masColor[1] = new SolidBrush(Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
            int[,] masClass = new int[50, 150000];

            int zna4Color = 0;
            while (solution == 0)
            {
                if (zna4Color != 0)
                    masColor[lenghtCore - 1] = new SolidBrush(Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));

                zna4Color = 1;

                for (i = 0; i < 50; i++)
                    for (j = 0; j < 150000; j++)
                        masClass[i, j] = 0;

                for (i = 0; i < lenghtCore; i++)
                    g.DrawEllipse((Pens.White), new Rectangle(core[i].X, core[i].Y, 5, 5));

                for (i = 0; i < numberOfObjects; i++)
                {
                    double len = 10000;
                    for (j = 0; j < lenghtCore; j++)
                    {
                        double lenPoint = EnterPointLenght(point[i], core[j]);
                        if (lenPoint < len)
                        {
                            kInd = j;
                            len = lenPoint;
                        }
                    }
                    masClass[kInd, i]++;
                    g.FillRectangle(new SolidBrush(Color.White), new Rectangle(point[i].X, point[i].Y, 2, 2));
                    g.FillRectangle(masColor[kInd], new Rectangle(point[i].X, point[i].Y, 2, 2));
                }

                var kondidatCore = new Point[50];
                var masLenKondidateCore = new double[50];
                
                for (i = 0; i < lenghtCore; i++)
                {
                    double len = 0;
                    for (j = 0; j < numberOfObjects; j++)
                    {
                        if (masClass[i, j] >= 1)
                        {
                            double lenPoint = EnterPointLenght(core[i], point[j]);
                            if (lenPoint > len)
                            {
                                kInd = j;
                                len = lenPoint;
                            }
                        }
                    }
                    masLenKondidateCore[i] = len;
                    kondidatCore[i] = point[kInd];
                }

                int maxLenght = 0;
                for (i = 0; i < lenghtCore - 1; i++)
                    if (masLenKondidateCore[maxLenght] < masLenKondidateCore[i + 1])
                        maxLenght = i + 1;

                int a = 1;
                double sum = 0;
                int s4et = 0;
                for (i = 0; i < lenghtCore - 1; i++)
                {
                    for (j = a; j < lenghtCore; j++)
                    {
                        sum += EnterPointLenght(core[i], core[j]);
                        s4et++;
                        if (j == lenghtCore - 1)
                            a++;
                    }
                }

                double proverka = sum / (2 * s4et);
                if (proverka > masLenKondidateCore[maxLenght])
                    solution = 1;
                else
                {
                    lenghtCore += 1;
                    Array.Resize(ref core, lenghtCore);
                    core[lenghtCore - 1] = kondidatCore[maxLenght];
                }

            }

            numericUpDown1.Text = Convert.ToString(core.Length);
            numberOfClasses = core.Length;

            var point_0 = new Points[numberOfClasses];
            var k0 = new tempvalue[numberOfClasses];
            var k = new Points[numberOfClasses];

            foreach (var temp in point)
                pointList.Add(new Points(temp.X, temp.Y));

            foreach (var temp in core)
                classList.Add(new Points(temp.X, temp.Y));

            j = 0;
            foreach (var temp in classList)
            {
                temp.drawPoint(graph, masColor[j], true);
                point_0[j] = temp;
                j++;
            }

            FillK(k);

            for (i = 0; i < numberOfClasses; i++)
            {
                graph.DrawEllipse(new Pen(Color.Black, 3), k0[i].X, k0[i].Y, 5, 5);
                graph.DrawEllipse(new Pen(Color.White, 3), k[i].X, k[i].Y, 5, 5);
            }

            {
                count = 0;
                while (notFinished == true)
                {
                    count++;

                    ReplacePoints(numberOfClasses, this.pictureBox1.Width, this.pictureBox1.Height, graph, k);

                    for (i = 0; i < numberOfClasses; i++) //prev centers
                    {
                        k0[i].X = k[i].X;
                        k0[i].Y = k[i].Y;
                    }

                    RecountKCenter(numberOfClasses, k);
                    notFinished = false;

                    for (i = 0; i < numberOfClasses; i++)
                        if (Math.Sqrt(Math.Pow((k0[i].X - k[i].X), 2) + Math.Pow((k0[i].Y - k[i].Y), 2)) > 2)
                            notFinished = true;

                    for (i = 0; i < numberOfClasses; i++)
                    {
                        if ((count % 5 == 0) && (count != 0))
                            foreach (var temp in pointInClassList[i])
                                temp.drawLink(graph, temp, point_0[i]);
  
                        graph.DrawEllipse(new Pen(Color.Black, 3), k0[i].X, k0[i].Y, 5, 5);
                        graph.DrawEllipse(new Pen(Color.White, 3), k[i].X, k[i].Y, 5, 5);
                    }
                }

                for (i = 0; i < numberOfClasses; i++)
                {
                    foreach (var temp in pointInClassList[i])
                        temp.drawLink(graph, temp, point_0[i]);

                    graph.DrawEllipse(new Pen(Color.Black, 3), k0[i].X, k0[i].Y, 5, 5);
                    graph.DrawEllipse(new Pen(Color.White, 3), k[i].X, k[i].Y, 5, 5);
                }
            }
            MessageBox.Show("Done!");
        }
    }
}