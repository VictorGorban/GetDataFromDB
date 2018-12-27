using System.Collections.Generic;
using System.ComponentModel;

namespace GetDataFromDB
{
    public class NewsPresenter
    {
        NewsView view;
        public BindingList<News> news;
        private LocalDB db;

        public void updateMe(IList<News> news)
        {
            this.news = new BindingList<News>(news);
            updateView();
        }

        private void updateView()
        {
            view.Update();
        }

        public NewsPresenter(NewsView view)
        {
            this.view = view;
            db = LocalDB.GetInstance().SubscribeOn(this);
        }
    }
}
