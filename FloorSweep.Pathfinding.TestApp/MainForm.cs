﻿
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
        private List<LoadedMap> Maps = new List<LoadedMap>();
        private LoadedMap CurrentMap => loadedMapsBox?.SelectedItem as LoadedMap;

        public MainForm(IPathFindingAlgorithm algorithm)
        {
            InitializeComponent();
            _algorithm = algorithm;

        }


        private void InitState(IReadOnlyDictionary<string, Mat> matrices, IReadOnlyDictionary<string, bool> isBinary)
        {
#if DEBUG
            EndInvoke(BeginInvoke(new Action(() =>
            {
                debugPanel.Controls.Clear();
                foreach (var gr in matrices)
                {
                    AddPicureBox(debugPanel, gr.Value, isBinary[gr.Key], gr.Key);
                }
            })));
#endif   
        }

        private void OnPathFound(IEnumerable<Math.Point> points, Control panel)
        {
            EndInvoke(BeginInvoke(new Action(() =>
            {
                if (!points.Any())
                {
                    MessageBox.Show("Path not found!");
                }
                var g = panel.CreateGraphics();
                g.DrawLines(Pens.Green, points.Select(p => (PointF)p).ToArray());
            }
                )));
        }


        private void AddPicureBox(Control panel, Mat g, bool onesAndZeros, string name, bool autoUpdate = true)
        {
            var gbox = new GroupBox { Text = name, Size = new Size(g.Cols, g.Rows) };
            gbox.MinimumSize = gbox.MaximumSize = gbox.Size;
            panel.Controls.Add(gbox);
            var p = new Panel { Dock = DockStyle.Fill, Name = name };
            p.Cursor = Cursors.Hand;
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
            var path = await _algorithm.CreateSession(CurrentMap.Data).FindPathAsync(InitState);

            CurrentMap.Mean.Add(path.CalculationStatistics.Total);
            var builder = new StringBuilder("Results:");
            foreach (var s in path.CalculationStatistics)
            {
                builder.Append($" {s.Key}:{s.Value} ms");
            }
            builder.Append($" Mean: {CurrentMap.Mean.Average()} ms");
            label1.Text = builder.ToString();
            OnPathFound(path.Path, mapPanel);
            running = false;
        }

        private async void OnItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == loadMapButton)
            {
                LoadMap();
            }
            if (e.ClickedItem == runButton)
            {
                await Run();
            }
        }

        private void LoadMap()
        {
            using (var selectForm = new LoadMapForm())
            {
                if (selectForm.ShowDialog() == DialogResult.OK)
                {
                    if (!Maps.Any(m => m.File == selectForm.LoadedMap.File))
                    {
                        Maps.Add(selectForm.LoadedMap);
                        loadedMapsBox.Items.Add(selectForm.LoadedMap);
                        loadedMapsBox.SelectedItem = selectForm.LoadedMap;
                        runButton.Enabled = true;
                    }
                }
            }
        }

        private void OnSelectedMapChanged(object sender, EventArgs e)
        {
            mapPanel.Size = CurrentMap.Image.Size;
            mapPanel.Location = new System.Drawing.Point(0, 0);
            mapPanel.BackgroundImage = CurrentMap.Image;
            //using (var g = mapPanel.CreateGraphics())
            //{
            //    g.Clear(Color.Red);
            //    g.DrawImageUnscaled(CurrentMap.Image, new System.Drawing.Point());
            //    g.Flush();
            //}
            mapPanel.Invalidate();
        }

        bool _imgMouseDown = false;
        System.Drawing.Point _mouseDownPoint;
        private void OnImgMouseDown(object sender, MouseEventArgs e)
        {
            _imgMouseDown = true;
            _mouseDownPoint = e.Location;
        }

        private void OnImgMouseUp(object sender, MouseEventArgs e)
        {
            _imgMouseDown = false;
        }

        private void OnImgMouseMove(object sender, MouseEventArgs e)
        {
            if (!_imgMouseDown) return;

            var hor = _mouseDownPoint.X - e.Location.X;
            var ver = _mouseDownPoint.Y - e.Location.Y;
            if (hor != 0)
                panel1.HorizontalScroll.Value = System.Math.Min(panel1.HorizontalScroll.Maximum, System.Math.Max(panel1.HorizontalScroll.Value + hor, 0));

            if (ver != 0)
                panel1.VerticalScroll.Value = System.Math.Min(panel1.VerticalScroll.Maximum, System.Math.Max(panel1.VerticalScroll.Value + ver, 0));
        }
    }
}