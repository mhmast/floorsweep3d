using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloorSweep.PathFinding.TestApp
{
    public partial class Form1 : Form
    {
        private PlottedPath _resp;
        private State _state;
        private Task _tsk;

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(PlottedPath resp)
        {
            BackgroundImage = resp.Image;
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        public Form1(State state, Task tsk) : this()
        {
            _state = state;
            InitState(state);
            _tsk = tsk;
        }

        private void InitState(State state)
        {
            var i = 0;
            foreach (var g in state.Graph)
            {
                AddPicureBox(graphPanel, g, true,$"graph{i}" );
                i++;
            }
            AddPicureBox(mapBox, state.Map, true,"map" );
            AddPicureBox(mapBox, state.Vis, true,"Vis" );
            AddPicureBox(mapBox, state.Template, true,"Template" );
        }

        private void AddPicureBox(Control panel, Mat g, bool onesAndZeros,string name)
        {
            var gbox = new GroupBox { Text = name, Size = new System.Drawing.Size(g.Width, g.Height) };
            gbox.MinimumSize = gbox.MaximumSize = gbox.Size;
            panel.Controls.Add(gbox);
            var p = new Panel { Dock = DockStyle.Fill };
            gbox.Controls.Add(p);
            AddEventHandler(p, g, onesAndZeros);
        }

        private void AddEventHandler(Control box, OpenCvSharp.Mat g, bool onesandzeros)
        {
            var img = DrawImage( g, onesandzeros);
            box.BackgroundImage = img;
            box.BackgroundImageLayout = ImageLayout.Stretch;
            if (g.Type() == MatType.CV_8UC1)
            {
                g.RegisterMatChanged((x, y, n) => UpdatePictureBoxb(box,img, x, y, n, onesandzeros));
            }
            else
            {

                g.RegisterMatChanged((int x, int y, double n) => UpdatePictureBox(box,img, x, y, n, onesandzeros)); ;
            }
           
        }

        private Bitmap DrawImage( Mat m, bool onesandzeros)
        {
            Bitmap bmp = new Bitmap(m.Width, m.Height);
            using var g = Graphics.FromImage(bmp);
            for (var r = 0; r < m.Rows; r++)
            {
                for (var c = 0; c < m.Cols; c++)
                {
                    var val = m._<double>(r, c);
                    if (onesandzeros)
                    {
                        g.FillRectangle(new SolidBrush(val == 1 ? Color.White : val == 0 ? Color.Gray : Color.Black), new Rectangle(r, c, 1, 1));
                    }
                    else
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb((int)val, (int)val, (int)val)), new Rectangle(r, c, 1, 1));
                    }
                }
            }
            g.Dispose();
            return bmp;

        }

        private void UpdatePictureBoxb(Control box,Image i, int x, int y, byte n, bool onesandzeros)
        {
            if (x < 0 || y < 0)
            {
                throw new ArgumentException();
            }
            EndInvoke(BeginInvoke(new Action(() =>
            {
                using var g = Graphics.FromImage(i);
                if (onesandzeros)
                {
                    g.FillRectangle(new SolidBrush(n == 1 ? Color.Green : n == 0 ? Color.Blue : Color.Red), new Rectangle(x, y, 1, 1));
                }
                else
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(n, n, n)), new Rectangle(x, y, 1, 1));
                }
                g.Dispose();
                box.BackgroundImage = i;
            })));
        }

        private void UpdatePictureBox(Control box,Image i, int x, int y, double n, bool onesandzeros)
        {
            UpdatePictureBoxb(box,i, x, y, (byte)n, onesandzeros);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _tsk.Start();
        }
    }
}
