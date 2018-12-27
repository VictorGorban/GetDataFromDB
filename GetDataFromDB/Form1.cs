using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetDataFromDB
{
    public partial class NewsView : Form
    {
        NewsPresenter presenter;
        public NewsView()
        {
            InitializeComponent();
            presenter = new NewsPresenter(this);
        }

        public void Update(IList<News> news)
        {
            dataView.Rows.Clear();
            foreach(var n in news)
            {
                dataView.Rows.Add(n.Title, n.Description, n.Source);
            }
            dataView.Update();
            if(!dataView.Visible)
                dataView.Visible = true;
        }

        private void dataView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var b = sender as Button;
            b.Enabled = false;
            await presenter.db.UpdateNewsRandom();
            await presenter.db.UpdateSubscribers();
            b.Enabled = true;
        }
    }
}
