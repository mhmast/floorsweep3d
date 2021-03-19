namespace FloorSweep.PathFinding.TestApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.loadMapButton = new System.Windows.Forms.ToolStripButton();
            this.runButton = new System.Windows.Forms.ToolStripButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.mapGroupBox = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mapPanel = new System.Windows.Forms.Panel();
            this.debugDataBox = new System.Windows.Forms.GroupBox();
            this.debugPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.loadedMapsBox = new System.Windows.Forms.ListBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.mapGroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            this.debugDataBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadMapButton,
            this.runButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(2052, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnItemClicked);
            // 
            // loadMapButton
            // 
            this.loadMapButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.loadMapButton.Image = ((System.Drawing.Image)(resources.GetObject("loadMapButton.Image")));
            this.loadMapButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadMapButton.Name = "loadMapButton";
            this.loadMapButton.Size = new System.Drawing.Size(64, 20);
            this.loadMapButton.Text = "Load Map";
            // 
            // runButton
            // 
            this.runButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.runButton.Enabled = false;
            this.runButton.Image = ((System.Drawing.Image)(resources.GetObject("runButton.Image")));
            this.runButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(34, 20);
            this.runButton.Text = "Run";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 25);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            this.splitContainer2.Panel1MinSize = 1000;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(2052, 728);
            this.splitContainer2.SplitterDistance = 1000;
            this.splitContainer2.TabIndex = 7;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.mapGroupBox);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.debugDataBox);
            this.splitContainer3.Panel2.Controls.Add(this.label1);
            this.splitContainer3.Panel2MinSize = 200;
            this.splitContainer3.Size = new System.Drawing.Size(1000, 728);
            this.splitContainer3.SplitterDistance = 430;
            this.splitContainer3.TabIndex = 6;
            // 
            // mapGroupBox
            // 
            this.mapGroupBox.Controls.Add(this.panel1);
            this.mapGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapGroupBox.Location = new System.Drawing.Point(0, 0);
            this.mapGroupBox.Name = "mapGroupBox";
            this.mapGroupBox.Size = new System.Drawing.Size(1000, 430);
            this.mapGroupBox.TabIndex = 6;
            this.mapGroupBox.TabStop = false;
            this.mapGroupBox.Text = "Map";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.mapPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(994, 400);
            this.panel1.TabIndex = 1;
            // 
            // mapPanel
            // 
            this.mapPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.mapPanel.Location = new System.Drawing.Point(0, 0);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(994, 395);
            this.mapPanel.TabIndex = 1;
            this.mapPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnImgMouseDown);
            this.mapPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnImgMouseMove);
            this.mapPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnImgMouseUp);
            // 
            // debugDataBox
            // 
            this.debugDataBox.Controls.Add(this.debugPanel);
            this.debugDataBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugDataBox.Location = new System.Drawing.Point(0, 25);
            this.debugDataBox.Name = "debugDataBox";
            this.debugDataBox.Size = new System.Drawing.Size(1000, 269);
            this.debugDataBox.TabIndex = 7;
            this.debugDataBox.TabStop = false;
            this.debugDataBox.Text = "Debug data";
            // 
            // debugPanel
            // 
            this.debugPanel.AutoScroll = true;
            this.debugPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugPanel.Location = new System.Drawing.Point(3, 27);
            this.debugPanel.Name = "debugPanel";
            this.debugPanel.Size = new System.Drawing.Size(994, 239);
            this.debugPanel.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 25);
            this.label1.TabIndex = 10;
            this.label1.Text = "Elapsed miliseconds:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.loadedMapsBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1048, 728);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Loaded Maps";
            // 
            // loadedMapsBox
            // 
            this.loadedMapsBox.DisplayMember = "File";
            this.loadedMapsBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadedMapsBox.FormattingEnabled = true;
            this.loadedMapsBox.ItemHeight = 25;
            this.loadedMapsBox.Location = new System.Drawing.Point(3, 27);
            this.loadedMapsBox.Name = "loadedMapsBox";
            this.loadedMapsBox.Size = new System.Drawing.Size(1042, 698);
            this.loadedMapsBox.TabIndex = 0;
            this.loadedMapsBox.SelectedIndexChanged += new System.EventHandler(this.OnSelectedMapChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(2052, 753);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "TestApp";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.mapGroupBox.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.debugDataBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton loadMapButton;
        private System.Windows.Forms.ToolStripButton runButton;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox loadedMapsBox;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox mapGroupBox;
        private System.Windows.Forms.GroupBox debugDataBox;
        private System.Windows.Forms.FlowLayoutPanel debugPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel mapPanel;
    }
}

