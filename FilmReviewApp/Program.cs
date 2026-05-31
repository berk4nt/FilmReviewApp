using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FilmReviewApp.Forms;

namespace FilmReviewApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool keepRunning = true;
            while (keepRunning)
            {
                LoginForm loginForm = new LoginForm();
                DialogResult result = loginForm.ShowDialog();

                if (result == DialogResult.OK && loginForm.LoginSuccessful)
                {
                    Form1 mainForm = new Form1(loginForm.IsAdmin);
                    Application.Run(mainForm);
                }
                else
                {
                    keepRunning = false;
                }
            }
        }
    }
}
