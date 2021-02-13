using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloorSweep.PathFinding.TestApp
{
    public partial class Form1 : Form
    {
        private PlottedPath _resp;

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(PlottedPath resp)
        {
            BackgroundImage = resp.Image;
            BackgroundImageLayout = ImageLayout.Stretch;
        }
    }
}
