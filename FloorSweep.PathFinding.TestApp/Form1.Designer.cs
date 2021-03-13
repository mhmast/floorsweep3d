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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.graphPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.gb1 = new System.Windows.Forms.GroupBox();
            this.mapBox = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.gb1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.graphPanel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(2052, 404);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Graph";
            // 
            // graphPanel
            // 
            this.graphPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphPanel.Location = new System.Drawing.Point(3, 27);
            this.graphPanel.Name = "graphPanel";
            this.graphPanel.Size = new System.Drawing.Size(2046, 374);
            this.graphPanel.TabIndex = 0;
            // 
            // gb1
            // 
            this.gb1.Controls.Add(this.mapBox);
            this.gb1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gb1.Location = new System.Drawing.Point(0, 404);
            this.gb1.Name = "gb1";
            this.gb1.Size = new System.Drawing.Size(2052, 753);
            this.gb1.TabIndex = 2;
            this.gb1.TabStop = false;
            this.gb1.Text = "Map";
            // 
            // mapBox
            // 
            this.mapBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapBox.Location = new System.Drawing.Point(3, 27);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(2046, 723);
            this.mapBox.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 1157);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(2052, 34);
            this.button1.TabIndex = 3;
            this.button1.Text = "Run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 1191);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Elapsed miliseconds:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(2078, 753);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gb1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.gb1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel graphPanel;
        private System.Windows.Forms.GroupBox gb1;
        private System.Windows.Forms.FlowLayoutPanel mapBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
    }
}

