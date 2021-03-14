
using FloorSweep.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloorSweep.PathFinding.TestApp
{
    public partial class Form1 : Form
    {
        private readonly State _state;
        private Func<State, string> _tsk;

        public Form1()
        {
            InitializeComponent();
        }



        public Form1(State state, Func<State, string> tsk) : this()
        {
            _state = state;
            _tsk = tsk;
            InitState(_state);
#if !DEBUG
            splitContainer1.Panel1Collapsed = true;
            splitContainer2.Panel1Collapsed = true;
#endif
        }

        private void InitState(State state)
        {
#if DEBUG

            var i = 0;

            foreach (var gr in state.Graph)
            {
                var stateLabel = i == 2 ? "nodestate" : "";
                AddPicureBox(graphPanel, gr, true, $"graph{i}{stateLabel}");
                i++;
            }
            AddPicureBox(mapBox, state.Image, false, "Image");
            AddPicureBox(mapBox, state.Vis, true, "Vis");
            AddPicureBox(mapBox, state.Template, true, "Template");

#endif
            AddPicureBox(mapBox, state.Map, true, "map", false);
            state.PathFound += () => State_PathFound(state, pathbox, "path");

        }

        private void State_PathFound(State obj, Control panel, string name)
        {
            EndInvoke(BeginInvoke(new Action(() =>
            {
                var bmp = DrawImage(obj.Map, true);
                var gbox = new GroupBox { Text = name, Size = bmp.Size };
                gbox.MinimumSize = gbox.MaximumSize = gbox.Size;
                panel.Controls.Add(gbox);
                var p = new Panel { Dock = DockStyle.Fill, Name = name };
                p.Click += P_Click;
                gbox.Controls.Add(p);
                var g = Graphics.FromImage(bmp);
                g.DrawLines(Pens.Green, obj.Path.Select(p => (PointF)p).ToArray());
                p.BackgroundImage = bmp;
                p.Refresh();



            }
                )));
        }


        private void AddPicureBox(Control panel, Mat g, bool onesAndZeros, string name, bool autoUpdate = true)
        {
            var gbox = new GroupBox { Text = name, Size = new Size(g.Cols, g.Rows) };
            gbox.MinimumSize = gbox.MaximumSize = gbox.Size;
            panel.Controls.Add(gbox);
            var p = new Panel { Dock = DockStyle.Fill, Name = name };
            p.Click += P_Click;
            gbox.Controls.Add(p);
            var img = DrawImage(g, onesAndZeros);
            p.BackgroundImage = img;
            p.BackgroundImageLayout = ImageLayout.Stretch;

            if (autoUpdate)
            {
                AddEventHandler(p, g, onesAndZeros, name, img);
            }
        }

        private void P_Click(object sender, EventArgs e)
        {
            var panel = sender as Panel;
            new Form { BackgroundImage = panel.BackgroundImage, BackgroundImageLayout = ImageLayout.Stretch }.Show();
        }

        private void AddEventHandler(Control box, Mat g, bool onesandzeros, string name, Bitmap img)
        {
            //if (g.Type() == MatType.CV_8UC1)
            //{
            //    g.RegisterMatChanged((x, y, n) => UpdatePictureBoxb(box, img, x, y, n, onesandzeros, name));
            //}
            //else
            //{

            g.RegisterMatChanged((int x, int y, double n) => UpdatePictureBox(box, img, x, y, n, onesandzeros, name)); ;
            //}

        }

        private Bitmap DrawImage(Mat m, bool onesandzeros)
        {
            Bitmap bmp = new Bitmap(m.Cols, m.Rows);
            using var g = Graphics.FromImage(bmp);
            for (var r = 1; r <= m.Rows; r++)
            {
                for (var c = 1; c <= m.Cols; c++)
                {
                    var val = m._<double>(r, c);
                    if (onesandzeros || double.IsInfinity(val))
                    {
                        g.FillRectangle(new SolidBrush(val == -1 ? Color.White : val == 0 ? Color.Gray : Color.Black), new Rectangle(r, c, 1, 1));
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

        private void UpdatePictureBoxb(Control box, Bitmap i, int x, int y, int n, bool onesandzeros, string name)
        {

            if (x < 0 || y < 0)
            {
                throw new ArgumentException();
            }
            EndInvoke(BeginInvoke(new Action(() =>
            {
                Color c = onesandzeros ? (n == 1 ? Color.Green : n == 0 ? Color.Red : Color.Blue) : Color.FromArgb(n, n, n);
                i.SetPixel(x, y, c);
                box.BackgroundImage = i;
                box.Invalidate();
            })));
        }

        private void UpdatePictureBox(Control box, Bitmap i, int x, int y, double n, bool onesandzeros, string name)
        {
            UpdatePictureBoxb(box, i, x, y, (int)n, onesandzeros, name);
        }


        bool running;
        private async void button1_Click(object sender, EventArgs e)
        {
            if (running) return;
            running = true;
            var ms = await Task.Run(() => _tsk(_state));
            label1.Text = ms;
            running = false;
        }
    }
}
