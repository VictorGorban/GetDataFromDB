using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetDataFromDB
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var view = new NewsView();
            var view2 = new NewsView();
            var presenter = new NewsPresenter();
            presenter.Subscribe(view);
            presenter.Subscribe(view2);
            view.presenter = presenter;
            view2.presenter = presenter;

            // view.Show();
            view2.Show();


            Application.Run(view);
        }
    }
}
