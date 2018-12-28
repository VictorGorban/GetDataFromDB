using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Timers;
using System;

namespace GetDataFromDB
{
    public class LocalDB
    {
        public List<News> news { get; private set; } = new List<News>();
        List<NewsPresenter> newsPresenters = new List<NewsPresenter>();

        static LocalDB db = null;

        string pathToDB = "NewsDB.sqlite";
        //SQLiteConnection connection;
        private LocalDB()
        {
        }

        public static LocalDB GetInstance()
        {
            return db != null ? db : db = new LocalDB();
        }

        public LocalDB Subscribe(NewsPresenter presenter)
        {
            if (!newsPresenters.Contains(presenter))
            {
                newsPresenters.Add(presenter);
            }
            return this;
        }

        public LocalDB UnSubscribe(NewsPresenter presenter)
        {
            newsPresenters.Remove(presenter);
            return this;
        }

        public async Task UpdateNewsRandom()
        {
            news.Clear();

            while (File.Exists(pathToDB))
            {
                pathToDB += "1";
            }
            SQLiteConnection.CreateFile(pathToDB);

            try
            {
                using (var connection = new SQLiteConnection("Data Source=" + pathToDB + ";Version=3"))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection))
                    {
                        command.Connection = connection;
                        command.CommandText = "CREATE TABLE IF NOT EXISTS News (id INTEGER PRIMARY KEY AUTOINCREMENT, title TEXT, description TEXT, source TEXT)";
                        await command.ExecuteNonQueryAsync();
                    }
                    var numNews = new Random().Next(1, 10);
                    for (var i = 0; i < numNews; i++)
                    {
                        news.Add(await News.GetRandom());
                    }
                }

                foreach (News n in news)
                {
                    using (var connection = new SQLiteConnection("Data Source=" + pathToDB + ";Version=3"))
                    {
                        connection.Open();

                        using (var command = new SQLiteCommand(connection))
                        {
                            command.CommandText = "INSERT INTO News(title, description, source) VALUES(?,?,?)";
                            DbParameter Field1 = command.CreateParameter();
                            DbParameter Field2 = command.CreateParameter();
                            DbParameter Field3 = command.CreateParameter();
                            command.Parameters.Add(Field1);
                            command.Parameters.Add(Field2);
                            command.Parameters.Add(Field3);
                            Field1.Value = n.Title;
                            Field2.Value = n.Description;
                            Field3.Value = n.Source;
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (SQLiteException)
            {
                System.Windows.Forms.MessageBox.Show("We have a problem with DB");
            }
        }

        public async Task UpdateSubscribers()
        {
            await getNewsAsync();

            foreach (var p in newsPresenters)
            {
                p.notify();
            }
        }

        public async Task getNewsAsync()
        {
            Task task = new Task(getNews);
            task.Start();
            await task;
        }

        public void getNews()
        {
            var query = "SELECT * FROM News";
            using (var connection = new SQLiteConnection("Data Source=" + pathToDB + ";Version=3"))
            {
                connection.Open();

                var adapter = new SQLiteDataAdapter(query, connection);
                var table = new DataTable();

                adapter.Fill(table);

                var news = new List<News>();

                foreach (DataRow row in table.Rows)
                {
                    news.Add(new News(
                        row.Field<string>(1),
                        row.Field<string>(2),
                        row.Field<string>(3)
                        ));
                }
            }
        }
    }
}
