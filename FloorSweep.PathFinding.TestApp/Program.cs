using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace FloorSweep.PathFinding.TestApp
{
    static class Program
    {


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            var provider = new ServiceCollection()

                .AddTransient<MainForm>()
                .UseFocussedDStar()
                .BuildServiceProvider();

            Application.Run(provider.GetService<MainForm>());






        }
    }
}
