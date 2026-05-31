using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilmReviewApp
{
    /// <summary>
    /// Entry point for the Film Review application.
    /// Initializes and runs the main form.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

