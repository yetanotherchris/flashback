using System;
using System.Collections.Generic;
using System.IO;
using Flashback.Core;
using Microsoft.Data.Sqlite;

namespace Flashback.Core.Net.Repositories
{
    // Minimal, single-file implementation of IRepository using Microsoft.Data.Sqlite.
    public class SqliteRepository : IRepository
    {
        private readonly string _dbPath;
        private readonly string _connectionString;

        public SqliteRepository(string dbPath)
        {
            _dbPath = dbPath ?? throw new ArgumentNullException(nameof(dbPath));
            _connectionString = new SqliteConnectionStringBuilder { DataSource = _dbPath }.ToString();
        }

        private SqliteConnection GetConnection() => new SqliteConnection(_connectionString);

        public void CreateDatabase()
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS categories (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL, inbuilt INTEGER DEFAULT 0, active INTEGER DEFAULT 0);";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS questions (id INTEGER PRIMARY KEY AUTOINCREMENT, [order] INTEGER DEFAULT 0, categoryid INTEGER, title TEXT, answer TEXT, askcount INTEGER DEFAULT 0, lastasked INTEGER DEFAULT 0, nextaskon INTEGER DEFAULT 0, previousinterval INTEGER DEFAULT 0, [interval] INTEGER DEFAULT 0, easinessfactor REAL DEFAULT 0, responsequality INTEGER DEFAULT 0);";
            cmd.ExecuteNonQuery();
        }

        public void DeleteDatabase()
        {
            if (File.Exists(_dbPath)) File.Delete(_dbPath);
        }

        public void DeleteCategory(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM categories WHERE id=@id; DELETE FROM questions WHERE categoryid=@id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public void DeleteQuestion(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM questions WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public IList<Category> ListCategories()
        {
            var list = new List<Category>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id,name,inbuilt,active FROM categories";
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(new Category { Id = reader.GetInt32(0), Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1), InBuilt = reader.GetInt32(2) != 0, Active = reader.GetInt32(3) != 0 });
            return list;
        }

        public IList<Question> ListQuestions()
        {
            var list = new List<Question>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT q.id,q.[order],q.categoryid,q.title,q.answer,q.askcount,q.lastasked,q.nextaskon,q.[interval],q.previousinterval,q.easinessfactor,q.responsequality,c.name,c.inbuilt,c.active FROM questions q JOIN categories c ON c.id = q.categoryid";
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(new Question { Id = reader.GetInt32(0), Order = reader.GetInt32(1), Category = new Category { Id = reader.GetInt32(2), Name = reader.IsDBNull(12) ? string.Empty : reader.GetString(12), InBuilt = reader.GetInt32(13) != 0, Active = reader.GetInt32(14) != 0 }, Title = reader.IsDBNull(3) ? string.Empty : reader.GetString(3), Answer = reader.IsDBNull(4) ? string.Empty : reader.GetString(4), AskCount = reader.GetInt32(5), LastAsked = new DateTime(reader.GetInt64(6)), NextAskOn = new DateTime(reader.GetInt64(7)), Interval = reader.GetInt32(8), PreviousInterval = reader.GetInt32(9), EasinessFactor = reader.GetDouble(10), ResponseQuality = reader.GetInt32(11) });
            return list;
        }

        public IList<Question> QuestionsForCategory(Category category)
        {
            var list = new List<Question>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id,[order],categoryid,title,answer,askcount,lastasked,nextaskon,[interval],previousinterval,easinessfactor,responsequality FROM questions WHERE categoryid = @categoryid";
            cmd.Parameters.AddWithValue("@categoryid", category.Id);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(new Question { Id = reader.GetInt32(0), Order = reader.GetInt32(1), Category = category, Title = reader.IsDBNull(3) ? string.Empty : reader.GetString(3), Answer = reader.IsDBNull(4) ? string.Empty : reader.GetString(4), AskCount = reader.GetInt32(5), LastAsked = new DateTime(reader.GetInt64(6)), NextAskOn = new DateTime(reader.GetInt64(7)), Interval = reader.GetInt32(8), PreviousInterval = reader.GetInt32(9), EasinessFactor = reader.GetDouble(10), ResponseQuality = reader.GetInt32(11) });
            return list;
        }

        public Category ReadCategory(int id)
        {
            var category = new Category();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id,name,inbuilt,active FROM categories WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read()) { category.Id = reader.GetInt32(0); category.Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1); category.InBuilt = reader.GetInt32(2) != 0; category.Active = reader.GetInt32(3) != 0; }
            return category;
        }

        public Question ReadQuestion(int id)
        {
            var q = new Question();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT q.id,q.[order],q.categoryid,q.title,q.answer,q.askcount,q.lastasked,q.nextaskon,q.[interval],q.previousinterval,q.easinessfactor,q.responsequality,c.name,c.inbuilt,c.active FROM questions q JOIN categories c ON c.id = q.categoryid WHERE q.id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read()) { q.Id = reader.GetInt32(0); q.Order = reader.GetInt32(1); q.Category = new Category { Id = reader.GetInt32(2), Name = reader.IsDBNull(12) ? string.Empty : reader.GetString(12), InBuilt = reader.GetInt32(13) != 0, Active = reader.GetInt32(14) != 0 }; q.Title = reader.IsDBNull(3) ? string.Empty : reader.GetString(3); q.Answer = reader.IsDBNull(4) ? string.Empty : reader.GetString(4); q.AskCount = reader.GetInt32(5); q.LastAsked = new DateTime(reader.GetInt64(6)); q.NextAskOn = new DateTime(reader.GetInt64(7)); q.Interval = reader.GetInt32(8); q.PreviousInterval = reader.GetInt32(9); q.EasinessFactor = reader.GetDouble(10); q.ResponseQuality = reader.GetInt32(11); }
            return q;
        }

        public int SaveCategory(Category category)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            if (category.Id > 0)
            {
                cmd.CommandText = "UPDATE categories SET name=@name, inbuilt=@inbuilt, active=@active WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", category.Id);
                cmd.Parameters.AddWithValue("@name", category.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@inbuilt", category.InBuilt ? 1 : 0);
                cmd.Parameters.AddWithValue("@active", category.Active ? 1 : 0);
                cmd.ExecuteNonQuery();
                return category.Id;
            }
            else
            {
                cmd.CommandText = "INSERT INTO categories (name,inbuilt,active) VALUES (@name,@inbuilt,@active); SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("@name", category.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@inbuilt", category.InBuilt ? 1 : 0);
                cmd.Parameters.AddWithValue("@active", category.Active ? 1 : 0);
                var id = (long)cmd.ExecuteScalar();
                category.Id = (int)id;
                return category.Id;
            }
        }

        public int SaveQuestion(Question question)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            if (question.Id > 0)
            {
                cmd.CommandText = "UPDATE questions SET answer=@answer, askcount=@askcount, categoryid=@categoryid, easinessfactor=@easinessfactor, [interval]=@interval, lastasked=@lastasked, nextaskon=@nextaskon, [order]=@order, previousinterval=@previousinterval, responsequality=@responsequality, title=@title WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", question.Id);
                cmd.Parameters.AddWithValue("@title", question.Title ?? string.Empty);
                cmd.Parameters.AddWithValue("@answer", question.Answer ?? string.Empty);
                cmd.Parameters.AddWithValue("@askcount", question.AskCount);
                cmd.Parameters.AddWithValue("@categoryid", question.Category?.Id ?? 0);
                cmd.Parameters.AddWithValue("@easinessfactor", question.EasinessFactor);
                cmd.Parameters.AddWithValue("@interval", question.Interval);
                cmd.Parameters.AddWithValue("@lastasked", question.LastAsked.Ticks);
                cmd.Parameters.AddWithValue("@nextaskon", question.NextAskOn.Ticks);
                cmd.Parameters.AddWithValue("@order", question.Order);
                cmd.Parameters.AddWithValue("@previousinterval", question.PreviousInterval);
                cmd.Parameters.AddWithValue("@responsequality", question.ResponseQuality);
                cmd.ExecuteNonQuery();
                return question.Id;
            }
            else
            {
                cmd.CommandText = "INSERT INTO questions ([order],categoryid,title,answer,askcount,lastasked,nextaskon,previousinterval,[interval],easinessfactor,responsequality) VALUES (@order,@categoryid,@title,@answer,@askcount,@lastasked,@nextaskon,@previousinterval,@interval,@easinessfactor,@responsequality); SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("@title", question.Title ?? string.Empty);
                cmd.Parameters.AddWithValue("@answer", question.Answer ?? string.Empty);
                cmd.Parameters.AddWithValue("@askcount", question.AskCount);
                cmd.Parameters.AddWithValue("@categoryid", question.Category?.Id ?? 0);
                cmd.Parameters.AddWithValue("@easinessfactor", question.EasinessFactor);
                cmd.Parameters.AddWithValue("@interval", question.Interval);
                cmd.Parameters.AddWithValue("@lastasked", question.LastAsked.Ticks);
                cmd.Parameters.AddWithValue("@nextaskon", question.NextAskOn.Ticks);
                cmd.Parameters.AddWithValue("@order", question.Order);
                cmd.Parameters.AddWithValue("@previousinterval", question.PreviousInterval);
                cmd.Parameters.AddWithValue("@responsequality", question.ResponseQuality);
                var id = (long)cmd.ExecuteScalar();
                return (int)id;
            }
        }

        public void MoveQuestion(Question question, int newIndex)
        {
            var questions = new List<Question>(ListQuestions());
            questions.Sort((a, b) => a.Order.CompareTo(b.Order));
            int currentIndex = questions.FindIndex(q => q.Id == question.Id);
            if (currentIndex == -1) return;
            questions.RemoveAt(currentIndex);
            questions.Insert(newIndex, question);
            for (int i = 0; i < questions.Count; i++) questions[i].Order = i;
            using var conn = GetConnection();
            conn.Open();
            using var tx = conn.BeginTransaction();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE questions SET [order]=@order WHERE id=@id";
            foreach (var q in questions)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@order", q.Order);
                cmd.Parameters.AddWithValue("@id", q.Id);
                cmd.ExecuteNonQuery();
            }
            tx.Commit();
        }
    }
}
