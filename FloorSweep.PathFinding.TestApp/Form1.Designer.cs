namespace FloorSweep.PathFinding.TestApp
{
    partial class Form1
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
            this.graphGroupBox = new System.Windows.Forms.GroupBox();
            this.graphPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.mapGroupBox = new System.Windows.Forms.GroupBox();
            this.mapBox = new System.Windows.Forms.FlowLayoutPanel();
            this.pathBoxgroup = new System.Windows.Forms.GroupBox();
            this.pathbox = new System.Windows.Forms.FlowLayoutPanel();
            this.graphGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.mapGroupBox.SuspendLayout();
            this.pathBoxgroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // graphGroupBox
            // 
            this.graphGroupBox.Controls.Add(this.graphPanel);
            this.graphGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphGroupBox.Location = new System.Drawing.Point(0, 0);
            this.graphGroupBox.Name = "graphGroupBox";
            this.graphGroupBox.Size = new System.Drawing.Size(2052, 199);
            this.graphGroupBox.TabIndex = 1;
            this.graphGroupBox.TabStop = false;
            this.graphGroupBox.Text = "Graph";
            // 
            // graphPanel
            // 
            this.graphPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphPanel.Location = new System.Drawing.Point(3, 27);
            this.graphPanel.Name = "graphPanel";
            this.graphPanel.Size = new System.Drawing.Size(2046, 169);
            this.graphPanel.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(2052, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "Run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Elapsed miliseconds:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 59);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.graphGroupBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(2052, 694);
            this.splitContainer1.SplitterDistance = 199;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.mapGroupBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pathBoxgroup);
            this.splitContainer2.Size = new System.Drawing.Size(2052, 491);
            this.splitContainer2.SplitterDistance = 212;
            this.splitContainer2.TabIndex = 3;
            // 
            // mapGroupBox
            // 
            this.mapGroupBox.Controls.Add(this.mapBox);
            this.mapGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapGroupBox.Location = new System.Drawing.Point(0, 0);
            this.mapGroupBox.Name = "mapGroupBox";
            this.mapGroupBox.Size = new System.Drawing.Size(2052, 212);
            this.mapGroupBox.TabIndex = 3;
            this.mapGroupBox.TabStop = false;
            this.mapGroupBox.Text = "Map";
            // 
            // mapBox
            // 
            this.mapBox.AutoScroll = true;
            this.mapBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapBox.Location = new System.Drawing.Point(3, 27);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(2046, 182);
            this.mapBox.TabIndex = 2;
            // 
            // pathBoxgroup
            // 
            this.pathBoxgroup.Controls.Add(this.pathbox);
            this.pathBoxgroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pathBoxgroup.Location = new System.Drawing.Point(0, 0);
            this.pathBoxgroup.Name = "pathBoxgroup";
            this.pathBoxgroup.Size = new System.Drawing.Size(2052, 275);
            this.pathBoxgroup.TabIndex = 3;
            this.pathBoxgroup.TabStop = false;
            this.pathBoxgroup.Text = "Path";
            // 
            // pathbox
            // 
            this.pathbox.AutoScroll = true;
            this.pathbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pathbox.Location = new System.Drawing.Point(3, 27);
            this.pathbox.Name = "pathbox";
            this.pathbox.Size = new System.Drawing.Size(2046, 245);
            this.pathbox.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(2052, 753);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.graphGroupBox.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.mapGroupBox.ResumeLayout(false);
            this.pathBoxgroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox graphGroupBox;
        private System.Windows.Forms.FlowLayoutPanel graphPanel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox mapGroupBox;
        private System.Windows.Forms.FlowLayoutPanel mapBox;
        private System.Windows.Forms.GroupBox pathBoxgroup;
        private System.Windows.Forms.FlowLayoutPanel pathbox;
    }
}

