using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GetDataFromDB
{
    public class NewsPresenter
    {
        NewsView view;
        public List<News> news;
        public LocalDB db;

        private void updateView()
        {
            view.Update();
        }

        public async Task getNews()
        {
            await db.UpdateNewsRandom();
        }

        public NewsPresenter(NewsView view)
        {
            this.view = view;
            db = LocalDB.GetInstance().SubscribeOn(this);
        }

        public void notify()
        {
            news = LocalDB.GetInstance().news;
            view.Update(news);
        }
    }
}
