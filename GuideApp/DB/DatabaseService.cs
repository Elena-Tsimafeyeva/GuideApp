using GuideApp.Model;
using Microsoft.Data.Sqlite;

namespace GuideApp.DB
{
    public class DatabaseService
    {
        //Путь к файлу БД
        private string dbPath = Path.Combine(FileSystem.AppDataDirectory, "Articles.db");
        //Метод GetArticlesBySection берёт статьи из одного раздела (1 - антропогенные, 2 - природные)
        public List<ArticleModel> GetArticlesBySection(int section)
        {
            var list = new List<ArticleModel>();
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
            SELECT id_Article, Title, Section, QR_Code, Way, Point
            FROM Article
            WHERE Section = @section
            ORDER BY Title
                                    ";
            command.Parameters.AddWithValue("@section", section);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new ArticleModel(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4),
                    reader.GetInt32(5)
                ));
            }
            return list;
        }
        // Метод GetArticleContent собирает контент статьи, а именно текст и картики в нужном порядке
        public List<ArticleContent> GetArticleContent(int articleId)
        {
            var result = new List<ArticleContent>();
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
            SELECT *
            FROM (
            SELECT Text AS Content, Text_Order AS [Order], 0 AS Type
            FROM Text
            WHERE id_Article = @id

            UNION ALL

            SELECT Image_Path AS Content, Image_Order AS [Order], 1 AS Type
            FROM Images
            WHERE id_Article = @id
            )
            ORDER BY [Order], Type;
            ";
            command.Parameters.AddWithValue("@id", articleId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new ArticleContent
                {
                    Content = reader.GetString(0),
                    Order = reader.GetInt32(1),
                    IsImage = reader.GetInt32(2) == 1
                });
            }

            return result;
        }
        // Метод SearchArticles ищет статьи по названию
        public List<ArticleModel> SearchArticles(string query)
        {
            var list = new List<ArticleModel>();

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
            SELECT id_Article, Title, Section, QR_Code, Way, Point
            FROM Article
            WHERE Title LIKE @query
            ORDER BY Title
            ";

            command.Parameters.AddWithValue("@query", $"%{query}%");

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new ArticleModel(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4),
                    reader.GetInt32(5)
                ));
            }

            return list;
        }
        // Метод GetAllArticles возвращает все статьи (без фильтра)
        public List<ArticleModel> GetAllArticles()
        {
            var list = new List<ArticleModel>();

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
            SELECT id_Article, Title, Section, QR_Code, Way, Point
            FROM Article
            ORDER BY Title
            ";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new ArticleModel(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4),
                    reader.GetInt32(5)
                ));
            }

            return list;
        }
        //Метод GetArticleByQr ищет статью по QR_Code
        public ArticleModel GetArticleByQr(string qrCode)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
            SELECT id_Article, Title, Section, QR_Code, Way, Point
            FROM Article
            WHERE QR_Code = @qr
            LIMIT 1
            ";

            //command.Parameters.AddWithValue("@qr", qrCode);
            command.Parameters.AddWithValue("@qr", int.Parse(qrCode));


            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new ArticleModel(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4),
                    reader.GetInt32(5)
                );
            }

            return null;
        }
        public List<ArticleModel> GetArticlesByWay(int way)
        {
            var list = new List<ArticleModel>();

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
            SELECT id_Article, Title, Section, QR_Code, Way, Point
            FROM Article
            WHERE Way = @way
            ORDER BY Point
            ";

            command.Parameters.AddWithValue("@way", way);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new ArticleModel(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4),
                    reader.GetInt32(5)
                ));
            }

            return list;
        }
    }
   
    }
