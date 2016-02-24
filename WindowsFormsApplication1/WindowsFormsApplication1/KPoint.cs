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
    public class Points
    {
        private int x, y;
        private Pen myPen = new Pen(Color.Black, 3);

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public Points(int newX, int newY)
        {
            x = newX;
            y = newY;
        }

        public void drawPoint(Graphics gr, int myColor)
        {
            Pen p = new Pen(Color.FromArgb(Math.Abs(x - myColor) % 255, Math.Abs(y - myColor) % 255, myColor), 2);
            gr.DrawEllipse(Pens.Black, x, y, 1, 1);
            myPen = p;
        }

        public void drawPoint(Graphics gr, SolidBrush color, bool flag)
        {
            Pen p = new Pen(color, 2);
            gr.DrawEllipse(Pens.Black, x, y, 1, 1);
            myPen = p;
        }

        public void drawLink(Graphics gr, Points p0, Points p1)
        {
            if (p1.myPen != myPen)
                gr.DrawEllipse(p1.myPen, p0.x, p0.y, 3, 3);
        }
    }

    public struct tempvalue
    {
        private int x;
        private int y;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
    }
}
