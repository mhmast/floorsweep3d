﻿using FloorSweep.PathFinding;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FloorSweep.Pathfinding.TestApp
{
    public partial class LoadMapForm : Form
    {
        private string Path { get; set; }
        private Image Img { get;  set; }
        private Math.Point? Start { get; set; }
        private Math.Point? End { get; set; }

        private int Scaling => scaleBox.SelectedIndex +1;

        public LoadedMap LoadedMap { get; private set; }
        public LoadMapForm()
        {
            InitializeComponent();

            scaleBox.SelectedIndex = 0;
            scaleBox.Enabled = false;
            scaleBox.SelectedIndexChanged += ScalingChanged;
            SetOk();
            LoadImage(Scaling);
        }

        private void ScalingChanged(object sender, EventArgs e)
        {
            GeneratePictureBox(Img,Scaling);
        }

        private void OnItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == loadImageButton)
            {
                LoadImage(Scaling);
            }
            if (e.ClickedItem == setEndButton)
            {
                setStartButton.Checked = false;
            }
            if (e.ClickedItem == setStartButton)
            {
                setEndButton.Checked = false;
            }
        }


        private void LoadImage(int scaling)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Path = openFileDialog1.FileName;
                Img = Image.FromFile(Path);
                GeneratePictureBox(Img,scaling);
                scaleBox.Enabled = true;
                setStartButton.Checked = true;
                setEndButton.Checked = false;
            }
            SetOk();
        }

        private void GeneratePictureBox(Image img, int scaling)
        {
            var pictureBox = new PictureBox
            {
                BackgroundImage = img,
                Width = img.Width / scaling,
                Height = img.Height / scaling,
                Left = ((ClientSize.Width - img.Width) / scaling / 2),
                Top = (ClientSize.Height - img.Height) / scaling / 2,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            panel1.Controls.Clear();
            panel1.Controls.Add(pictureBox);
            pictureBox.MouseMove += PictureBoxMouseMove;
            pictureBox.MouseClick += PictureBoxMouseClick;
        }

        private void PictureBoxMouseClick(object sender, MouseEventArgs e)
        {
            if (setStartButton.Checked)
            {
                Start = new Math.Point(e.X, e.Y);
                startLabel.Text = $"Start (x:{e.X},y:{e.Y})";
                setStartButton.Checked = false;
                setEndButton.Checked = true;
            }
            else if (setEndButton.Checked)
            {
                End = new Math.Point(e.X, e.Y);
                endLabel.Text = $"End (x:{e.X},y:{e.Y})";
            }
            SetOk();
        }

        private void SetOk()
        {
            okButton.Enabled = Start != null && End != null && Path != null;
        }

        private void PictureBoxMouseMove(object sender, EventArgs e)
        {
            if (sender is Control c)
            {
                locationLabel.Text = $"Location: x:{c.Cursor.HotSpot.X},y:{c.Cursor.HotSpot.X}";
            }
        }

        private void OnBottomItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == okButton)
            {
                LoadedMap = new LoadedMap(MapData.FromImage(Path, Start.Value, End.Value, Scaling), Path,Img);
                DialogResult = DialogResult.OK;
            }
            if (e.ClickedItem == cancelButton)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}