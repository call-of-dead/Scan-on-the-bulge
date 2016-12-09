using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintandScan
{
    public partial class Form1 : Form
    {
        private bool[,] Matr;
        private int w;
        private int h;
        private Graphics graph;
        private SolidBrush brush;
        private int rez = 10;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text!="" && textBox2.Text!=""){
                w = Int32.Parse(textBox1.Text);
                h = Int32.Parse(textBox2.Text);
                Matr = new bool[w, h];
                pictureBox1.Enabled = true;
                graph=pictureBox1.CreateGraphics();
                brush = new SolidBrush(Color.White);
                graph.FillRectangle(brush, 0, 0, w*rez, h*rez);
                brush.Color = Color.Red;
                this.rez = Int32.Parse(textBox4.Text);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //textBox3.Text = e.X.ToString() + " " + e.Y.ToString();
            //return;
            if (e.Button == MouseButtons.Left)
            {
                int r = 10;
                int x = e.X;// e.X/rez;// -pictureBox1.Location.X;
                int y = e.Y;// e.Y / rez;// -pictureBox1.Location.Y;
                if (x < 0 || y < 0)
                    return;
                if ((x/rez) >= w || (y/rez) >= h)
                    return;
                graph.FillEllipse(brush, (x - r), (y - r), 2 * r, 2 * r);
                Matr[x/rez,y/rez] = true;
                System.IO.File.WriteAllText("tmp.txt",this.convMatrToStr());
                //textBox3.Text = this.convMatrToStr();
            }
        }
        private string convMatrToStr()
        {
            string str = "";
            for (int j = 0; j <h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    if (Matr[i, j])
                    {
                        str += "1";
                    }
                    else
                    {
                        str += " ";
                    }
                    str += " ";
                }
                str += System.Environment.NewLine;
            }
            return str;
        }
        private int Convex(Point[] p, int n)
        {
            int CONVEX = 1;
            int  CONCAVE = -1;
            int i, j, k;
            int flag = 0;
            double z;

            if (n < 3)
                return (0);

            for (i = 0; i < n; i++)
            {
                j = (i + 1) % n;
                k = (i + 2) % n;
                z = (p[j].X - p[i].X) * (p[k].Y - p[j].Y);
                z -= (p[j].Y - p[i].Y) * (p[k].X - p[j].X);
                if (z < 0)
                    flag |= 1;
                else if (z > 0)
                    flag |= 2;
                if (flag == 3)
                    return (CONCAVE);
            }
            if (flag != 0)
                return (CONVEX);
            else
                return (0);
        }

        private bool Scan1()
        {
            string str = "";
            int kol = 0;
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    if (Matr[i, j])
                        kol++;
            Point[] mass = new Point[kol*2];
            int k = 0;
            Point tmp=new Point();
            for (int x1 = 0; x1 < w; x1++)
            {
                for (int y1 = 0; y1 < h; y1++)
                {
                    if (Matr[x1, y1])
                    {
                        tmp = new Point(x1, y1);
                        break;
                    }  
                }
                if (!tmp.Equals(new Point()))
                    break;
            }
            mass[k++]=tmp;
            int y = tmp.Y;
            int x;
            for (x = tmp.X + 1; x < w; x++)
            {
                if (Matr[x, y + 1])
                {
                    if (!mass.Contains(new Point(x - 1, y)))
                        mass[k++] = new Point(x, y);
                    //mass[k++] = new Point(x, y + 1);
                    y = y + 1;
                    continue;
                }
                if (Matr[x, y - 1])
                {
                    if (!mass.Contains(new Point(x - 1, y)))
                        mass[k++] = new Point(x, y);
                    //mass[k++] = new Point(x, y - 1);
                    y = y - 1;
                    continue;
                }
                if (Matr[x, y + 2])
                {
                    if (!mass.Contains(new Point(x - 1, y)))
                        mass[k++] = new Point(x, y);
                    //mass[k++] = new Point(x, y + 2);
                    y = y + 2;
                    continue;
                }
                if (Matr[x, y - 2])
                {
                    if (!mass.Contains(new Point(x - 1, y)))
                        mass[k++] = new Point(x, y);
                    //mass[k++] = new Point(x, y - 2);
                    y = y - 2;
                    continue;
                }
            }
            for (x=mass[k-1].X; x > tmp.X; x--)
            {
                if (Matr[x, y + 1])
                {
                    if (!mass.Contains(new Point(x - 1, y)))
                        mass[k++] = new Point(x, y);
                    //mass[k++] = new Point(x, y + 1);
                    y = y + 1;
                    continue;
                }
                if (Matr[x, y - 1])
                {
                    if (!mass.Contains(new Point(x - 1, y)))
                        mass[k++] = new Point(x, y);
                    //mass[k++] = new Point(x, y - 1);
                    y = y - 1;
                    continue;
                }
                if (Matr[x, y + 2])
                {
                    if (!mass.Contains(new Point(x - 1, y)))
                        mass[k++] = new Point(x, y);
                   // mass[k++] = new Point(x, y + 2);
                    y = y + 2;
                    continue;
                }
                if (Matr[x, y - 2])
                {
                    if (!mass.Contains(new Point(x - 1, y)))
                        mass[k++] = new Point(x, y);
                    //mass[k++] = new Point(x, y - 2);
                    y = y - 2;
                    continue;
                }
            }
            for (int i = 0; i < k; i++)
            {
                str += " (" +mass[i].X.ToString() + ", " + mass[i].Y.ToString() + ") ";
            }
            textBox3.Text = str;
            int re = Convex(mass, k);
            return (re==-1);

            //bool[,] tmpM = (bool[,]) Matr.Clone();
            //int x = 0;
            //for (; x < w; x++)
            //{
            //    Point pp;
            //    for (int y1 = 0; y1 < h; y1++)
            //        if (Matr[x, y1])
            //            pp = new Point(x, y1);
            //    if (pp == null)
            //        continue;

            //    for (int x1 = x + 1; x1 < w; x1++)
            //    {
            //        bool tr = false;
            //        for (int y = 0; y < h; y++)
            //        {
            //            if (Matr[x, y])
            //            {
            //                tr = true;
            //                if (pp.Y == y)
            //                    continue;
            //                double six = Math.Abs(pp.X - x) / Math.Abs(pp.Y - y);
            //                int tmpy = pp.Y;
            //                for (int tmpx = pp.X; tmpx != x; tmpx++)
            //                {

            //                }

            //            }
            //            tr = true;

            //            if (tr)
            //                tmpM[x, y] = true;

            //        }
            //    }

                
            //}

        }

        private bool Scan2()
        {
            
            for (int x = 0; x < w/2; x++)
            {
                bool prev = false;
                bool tr = false;
                for (int y = 0; y < h; y++)
                {
                    if (Matr[x, y])
                    {
                        if (tr && !prev)
                        {
                            return true;
                        }
                        prev = true;
                        tr = true;
                    }
                    else
                    {
                        prev = false;
                    }
                }

            }

            for (int x = w/2; x < w; x++)
            {
                bool prev = false;
                bool tr = false;
                for (int y = 0; y < h; y++)
                {
                    if (Matr[x, y])
                    {
                        if (tr && !prev)
                        {
                            return true;
                        }
                        prev = true;
                        tr = true;
                    }
                    else
                    {
                        prev = false;
                    }
                }

            }

            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Scan1())
            {
                label1.Text = "Вогнутое";
            } else 
            label1.Text = "Выпуклое";
        }
    }
}
