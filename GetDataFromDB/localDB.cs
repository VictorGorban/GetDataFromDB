using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Timers;

namespace GetDataFromDB
{
    public class LocalDB
    {
        private List<News> news;
        NewsPresenter newsPresenter;

        static LocalDB db;

        string pathToDB = "NewsDB.sqlite";
        //SQLiteConnection connection;
        SQLiteCommand command;
        private LocalDB(NewsPresenter presenter = null)
        {
            newsPresenter = presenter;
            if (!File.Exists(pathToDB))
            {
                SQLiteConnection.CreateFile(pathToDB);
                try
                {
                    using (var connection = new SQLiteConnection("Data Source=" + pathToDB + ";Version=3"))
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "CREATE TABLE IF NOT EXISTS News (id INTEGER PRIMARY KEY AUTOINCREMENT, title TEXT, description TEXT)";
                        command.ExecuteNonQueryAsync();

                        news.Add(new News("В «банках ДНР» решили выдавать кредиты", "Донецкие новости"));
                        news.Add(new News("В Мариуполе судья попался на взятке", "Новостной портал ZI"));
                        news.Add(new News("В Украине задержали продавца пластида: фото", "Реал"));

                        command.CommandText = "INSERT INTO News(title, description) VALUES(?,?)";

                        foreach (News n in news)
                        {
                            DbParameter Field1 = command.CreateParameter();
                            DbParameter Field2 = command.CreateParameter();
                            command.Parameters.Add(Field1);
                            command.Parameters.Add(Field2);
                            Field1.Value = n.Title;
                            Field2.Value = n.Description;
                        }
                        StartAutoUpdate();
                    }
                }
                catch (SQLiteException)
                {
                    System.Windows.Forms.MessageBox.Show("We have a problem with DB");
                }
            }
        }


        public static LocalDB GetInstance()
        {
            return db!=null?db:new LocalDB();
        }

        public LocalDB SubscribeOn(NewsPresenter presenter)
        {
            newsPresenter = presenter;
            return this;
        }

        public async void UpdateSubscribers()
        {
            news = await getNewsAsync();
            newsPresenter.updateMe(news);
        }

        public void StartAutoUpdate()
        {
            var Timer = new System.Timers.Timer(5000)
            {
                AutoReset = true,
                Enabled = true,
            };
            Timer.Elapsed += OnTimedEvent;
            Timer.Start();


        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            UpdateSubscribers();
        }

        private async Task<List<News>> getNewsAsync()
        {
            var query = "SELECT * FROM News";
            command.CommandText = query;

            Task<List<News>> task = new Task<List<News>>(getNews);
            task.Start();
            return await task;
        }

        private List<News> getNews()
        {
            var query = "SELECT * FROM News";
            command.CommandText = query;
            using (var connection = new SQLiteConnection("Data Source=" + pathToDB + ";Version=3"))
            {
                var adapter = new SQLiteDataAdapter(query, connection);
                var table = new DataTable();

                adapter.Fill(table);

                var news = new List<News>();

                foreach (DataRow row in table.Rows)
                {
                    news.Add(new News(
                        row.Field<string>(0),
                        row.Field<string>(1)
                        ));
                }
            }
            return news;
        }
    }
}
