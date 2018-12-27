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

        public void Update(BindingList<News> news)
        {
            dataView.DataSource = news;
        }

        private void dataView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
