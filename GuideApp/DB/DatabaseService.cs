using System.IO;
using GuideApp.Model;
using Microsoft.Data.Sqlite;

namespace GuideApp.DB
{
    public class DatabaseService
    {
        private string dbPath = Path.Combine(FileSystem.AppDataDirectory, "Articles.db");

        public List<ArticleModel> GetArticlesBySection(int section)
        {
            var list = new List<ArticleModel>();
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
            SELECT id_Article, Title, Section, QR_Code
            FROM Article
            WHERE Section = @section
                                    ";
            command.Parameters.AddWithValue("@section", section);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new ArticleModel(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3)
                ));
            }
            return list;
        }

        public List<ArticleContent> GetArticleContent(int articleId)
        {
            var result = new List<ArticleContent>();
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
            SELECT Text AS Content, Text_Order AS [Order], 0 AS Type
            FROM Text
            WHERE id_Article = @id

            UNION ALL

            SELECT Image_Path AS Content, Image_Order AS [Order], 1 AS Type
            FROM Images
            WHERE id_Article = @id

            ORDER BY [Order]
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
        
        public List<ArticleModel> SearchArticles(string query)
        {
            var list = new List<ArticleModel>();

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
            SELECT id_Article, Title, Section, QR_Code
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
                    reader.GetInt32(3)
                ));
            }

            return list;
        }
        
    }
   
}
