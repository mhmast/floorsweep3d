
namespace FloorSweep.Pathfinding.TestApp
{
    partial class LoadMapForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadMapForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.loadImageButton = new System.Windows.Forms.ToolStripButton();
            this.setStartButton = new System.Windows.Forms.ToolStripButton();
            this.setEndButton = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.locationLabel = new System.Windows.Forms.ToolStripLabel();
            this.startLabel = new System.Windows.Forms.ToolStripLabel();
            this.endLabel = new System.Windows.Forms.ToolStripLabel();
            this.okButton = new System.Windows.Forms.ToolStripButton();
            this.cancelButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadImageButton,
            this.setStartButton,
            this.setEndButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1009, 34);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnItemClicked);
            // 
            // loadImageButton
            // 
            this.loadImageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.loadImageButton.Image = ((System.Drawing.Image)(resources.GetObject("loadImageButton.Image")));
            this.loadImageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadImageButton.Name = "loadImageButton";
            this.loadImageButton.Size = new System.Drawing.Size(110, 29);
            this.loadImageButton.Text = "Load Image";
            // 
            // setStartButton
            // 
            this.setStartButton.CheckOnClick = true;
            this.setStartButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.setStartButton.Image = ((System.Drawing.Image)(resources.GetObject("setStartButton.Image")));
            this.setStartButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.setStartButton.Name = "setStartButton";
            this.setStartButton.Size = new System.Drawing.Size(52, 29);
            this.setStartButton.Text = "Start";
            // 
            // setEndButton
            // 
            this.setEndButton.CheckOnClick = true;
            this.setEndButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.setEndButton.Image = ((System.Drawing.Image)(resources.GetObject("setEndButton.Image")));
            this.setEndButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.setEndButton.Name = "setEndButton";
            this.setEndButton.Size = new System.Drawing.Size(46, 29);
            this.setEndButton.Text = "End";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "\"Images|*.png,*.jpg,*.gif\"";
            // 
            // panel1
            // 
            this.panel1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1009, 550);
            this.panel1.TabIndex = 2;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.locationLabel,
            this.startLabel,
            this.endLabel,
            this.okButton,
            this.cancelButton});
            this.toolStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStrip2.Location = new System.Drawing.Point(0, 584);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1009, 160);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            this.toolStrip2.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnBottomItemClicked);
            // 
            // locationLabel
            // 
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(1006, 25);
            this.locationLabel.Text = "Location:";
            // 
            // startLabel
            // 
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(1006, 25);
            this.startLabel.Text = "Start:";
            // 
            // endLabel
            // 
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(1006, 25);
            this.endLabel.Text = "Text";
            // 
            // okButton
            // 
            this.okButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.okButton.Image = ((System.Drawing.Image)(resources.GetObject("okButton.Image")));
            this.okButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(1006, 29);
            this.okButton.Text = "OK";
            // 
            // cancelButton
            // 
            this.cancelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cancelButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelButton.Image")));
            this.cancelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(1006, 29);
            this.cancelButton.Text = "Cancel";
            // 
            // LoadMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 744);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Name = "LoadMapForm";
            this.Text = "LoadMapForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton loadImageButton;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton setStartButton;
        private System.Windows.Forms.ToolStripButton setEndButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel locationLabel;
        private System.Windows.Forms.ToolStripLabel startLabel;
        private System.Windows.Forms.ToolStripLabel endLabel;
        private System.Windows.Forms.ToolStripButton okButton;
        private System.Windows.Forms.ToolStripButton cancelButton;
    }
}