using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GetDataFromDB
{
    public class NewsPresenter
    {
        List<NewsView> views = new List<NewsView>();
        public List<News> news;
        public LocalDB db;

        private void updateViews()
        {
            foreach (var view in views)
            {
                view.Update(news);
            }
        }

        public NewsPresenter Unsubscribe(NewsView view)
        {
            views.Remove(view);
            return this;
        }

        public NewsPresenter Subscribe(NewsView view)
        {
            if (!views.Contains(view))
                views.Add(view);
            return this;
        }

        public async Task getNews()
        {
            await db.UpdateNewsRandom();
        }

        public NewsPresenter()
        {
            db = LocalDB.GetInstance().Subscribe(this);
        }

        public void notify()
        {
            news = LocalDB.GetInstance().news;
            updateViews();
        }
    }
}
