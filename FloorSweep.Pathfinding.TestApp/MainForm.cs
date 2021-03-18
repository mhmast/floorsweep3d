
using FloorSweep.Math;
using FloorSweep.Pathfinding.TestApp;
using FloorSweep.PathFinding.Interfaces;
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
    public partial class MainForm : Form
    {
        private List<LoadedMap> Maps = new  List<LoadedMap>();
        private LoadedMap _currentMap = null;

        public MainForm(IPathFindingAlgorithm algorithm, IDictionary<string, Mat> matrices)
        {
            InitializeComponent();
            InitState(matrices);
            _algorithm = algorithm;
#if !DEBUG
            splitContainer1.Panel1Collapsed = true;
            splitContainer2.Panel1Collapsed = true;
#endif
        }

      
        private void InitState(IDictionary<string,Mat> matrices, IDictionary<string, bool> isBinary)
        {
#if DEBUG
            foreach (var gr in matrices)
            {
                AddPicureBox(graphPanel, gr.Value, isBinary[gr.Key], gr.Key);
            }
#endif   
        }

        private void OnPathFound(Mat map,IEnumerable<Math.Point> points, Control panel, string name)
        {
            EndInvoke(BeginInvoke(new Action(() =>
            {
                var bmp = DrawImage(map, true);
                var gbox = new GroupBox { Text = name, Size = bmp.Size };
                gbox.MinimumSize = gbox.MaximumSize = gbox.Size;
                panel.Controls.Add(gbox);
                var p = new Panel { Dock = DockStyle.Fill, Name = name };
                p.Click += P_Click;
                gbox.Controls.Add(p);
                var g = Graphics.FromImage(bmp);
                g.DrawLines(Pens.Green, points.Select(p => (PointF)p).ToArray());
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
#if DEBUG
            g.MatChanged += (int x, int y, double n) => UpdatePictureBox(box, img, x, y, n, onesandzeros, name); ;
#endif
        }

        private Bitmap DrawImage(Mat m, bool onesandzeros)
        {
            Bitmap bmp = new Bitmap(m.Cols, m.Rows);
            using var g = Graphics.FromImage(bmp);
            for (var r = 1; r <= m.Rows; r++)
            {
                for (var c = 1; c <= m.Cols; c++)
                {
                    var val = m[r, c];
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
        private readonly IPathFindingAlgorithm _algorithm;

        private async Task Run()
        {
            if (running) return;
            running = true;
            var path = await _algorithm.CreateSession(_currentMap.Data).FindPathAsync();
            _currentMap.Mean.Add(path.CalculationStatistics.Total);
            var builder = new StringBuilder("Results:");
            foreach(var s in path.CalculationStatistics)
            {
                builder.Append($" {s.Key}:{s.Value} ms");
            }
            builder.Append($" Mean: {_currentMap.Mean.Average()} ms");
            label1.Text = builder.ToString();
            OnPathFound(_currentMap.Data.Map, path.Path, graphPanel, "Path");
            running = false;
        }

        private async void OnItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem == loadMapButton)
            {
                LoadMap();
            }
            if(e.ClickedItem == runButton)
            {
                await Run();
            }
        }

        private void LoadMap()
        {
            using(var selectForm = new LoadMapForm())
            {
                if(selectForm.ShowDialog() == DialogResult.OK)
                {
                    if(!Maps.Any(m=>m.File == selectForm.LoadedMap.File))
                    {
                        Maps.Add(selectForm.LoadedMap);
                        _currentMap = selectForm.LoadedMap;
                    }
                }
            }
        }
    }
}
