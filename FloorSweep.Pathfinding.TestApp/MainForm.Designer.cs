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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.mapGroupBox = new System.Windows.Forms.GroupBox();
            this.mapBox = new System.Windows.Forms.FlowLayoutPanel();
            this.pathBoxgroup = new System.Windows.Forms.GroupBox();
            this.pathbox = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.mapGroupBox.SuspendLayout();
            this.pathBoxgroup.SuspendLayout();
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
            this.toolStrip1.Size = new System.Drawing.Size(2052, 34);
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
            this.loadMapButton.Size = new System.Drawing.Size(96, 29);
            this.loadMapButton.Text = "Load Map";
            // 
            // runButton
            // 
            this.runButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.runButton.Enabled = false;
            this.runButton.Image = ((System.Drawing.Image)(resources.GetObject("runButton.Image")));
            this.runButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(47, 29);
            this.runButton.Text = "Run";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 34);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(2052, 719);
            this.splitContainer2.SplitterDistance = 684;
            this.splitContainer2.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.mapGroupBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pathBoxgroup);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(684, 719);
            this.splitContainer1.SplitterDistance = 205;
            this.splitContainer1.TabIndex = 6;
            // 
            // mapGroupBox
            // 
            this.mapGroupBox.Controls.Add(this.mapBox);
            this.mapGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapGroupBox.Location = new System.Drawing.Point(0, 0);
            this.mapGroupBox.Name = "mapGroupBox";
            this.mapGroupBox.Size = new System.Drawing.Size(684, 205);
            this.mapGroupBox.TabIndex = 4;
            this.mapGroupBox.TabStop = false;
            this.mapGroupBox.Text = "Map";
            // 
            // mapBox
            // 
            this.mapBox.AutoScroll = true;
            this.mapBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapBox.Location = new System.Drawing.Point(3, 27);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(678, 175);
            this.mapBox.TabIndex = 2;
            // 
            // pathBoxgroup
            // 
            this.pathBoxgroup.Controls.Add(this.pathbox);
            this.pathBoxgroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pathBoxgroup.Location = new System.Drawing.Point(0, 25);
            this.pathBoxgroup.Name = "pathBoxgroup";
            this.pathBoxgroup.Size = new System.Drawing.Size(684, 485);
            this.pathBoxgroup.TabIndex = 4;
            this.pathBoxgroup.TabStop = false;
            this.pathBoxgroup.Text = "Path";
            // 
            // pathbox
            // 
            this.pathbox.AutoScroll = true;
            this.pathbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pathbox.Location = new System.Drawing.Point(3, 27);
            this.pathbox.Name = "pathbox";
            this.pathbox.Size = new System.Drawing.Size(678, 455);
            this.pathbox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Elapsed miliseconds:";
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
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.mapGroupBox.ResumeLayout(false);
            this.pathBoxgroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton loadMapButton;
        private System.Windows.Forms.ToolStripButton runButton;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox mapGroupBox;
        private System.Windows.Forms.FlowLayoutPanel mapBox;
        private System.Windows.Forms.GroupBox pathBoxgroup;
        private System.Windows.Forms.FlowLayoutPanel pathbox;
        private System.Windows.Forms.Label label1;
    }
}

