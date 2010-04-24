using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

#if MONOTOUCH
using Mono.Data.Sqlite;
#endif

#if WINDOWS
using SqliteDataReader = System.Data.SQLite.SQLiteDataReader;
using SqliteCommand = System.Data.SQLite.SQLiteCommand;
using SqliteConnection = System.Data.SQLite.SQLiteConnection;
using SqliteParameter = System.Data.SQLite.SQLiteParameter;
using SqliteException = System.Data.SQLite.SQLiteException;
#endif

namespace Flashback.Core
{
	public partial class Question : BaseDataObject<Question>
	{
		protected override string TableName
		{
			get
			{
				return "questions";
			}
		}

		protected override Question GetRow(SqliteDataReader reader)
		{
			Question question = new Question();
			question.Id = Convert.ToInt32(reader["id"]);
			question.Answer = (string)reader["answer"];
			question.AskCount = Convert.ToInt32(reader["askcount"]);
			
			int id = Convert.ToInt32(reader["categoryid"]);
			question.Category = Category.Read(id);

			question.EasinessFactor = Convert.ToDouble(reader["easinessfactor"]);
			question.Interval = Convert.ToInt32(reader["interval"]);
			question.LastAsked = new DateTime(Convert.ToInt64(reader["lastasked"]));
			question.NextAskOn = new DateTime(Convert.ToInt64(reader["nextaskon"]));
			question.Order = Convert.ToInt32(reader["order"]);
			question.PreviousInterval = Convert.ToInt32(reader["previousinterval"]);
			question.ResponseQuality = Convert.ToInt32(reader["responsequality"]);
			question.Title = (string) reader["title"];

			return question;
		}

		protected override int Save(SqliteCommand command, bool updating)
		{
			SqliteParameter parameter;
			string sql = @"INSERT INTO questions (answer,askcount,categoryid,easinessfactor,interval,lastasked,nextaskon,[order],previousinterval,responsequality,title) ";
			sql += "VALUES (@answer,@askcount,@categoryid,@easinessfactor,@interval,@lastasked,@nextaskon,@order,@previousinterval,@responsequality,@title);SELECT last_insert_rowid();";

			if (updating)
			{
				sql = @"UPDATE questions SET answer=@answer,askcount=@askcount,categoryid=@categoryid,easinessfactor=@easinessfactor,interval=@interval,";
				sql += "lastasked=@lastasked,nextaskon=@nextaskon,[order]=@order,previousinterval=@previousinterval,responsequality=@responsequality,title=@title ";
				sql += "WHERE id=@id";

				parameter = new SqliteParameter("@id", DbType.Int32);
				parameter.Value = Id;
				command.Parameters.Add(parameter);
			}

			parameter = new SqliteParameter("@answer", DbType.String);
			parameter.Value = Answer;
			command.Parameters.Add(parameter);

			parameter = new SqliteParameter("@askcount", DbType.Int32);
			parameter.Value = AskCount;
			command.Parameters.Add(parameter);

			parameter = new SqliteParameter("@categoryid", DbType.Int32);
			parameter.Value = Category.Id;
			command.Parameters.Add(parameter);

			parameter = new SqliteParameter("@easinessfactor", DbType.Double);
			parameter.Value = EasinessFactor;
			command.Parameters.Add(parameter);

			parameter = new SqliteParameter("@interval", DbType.Int32);
			parameter.Value = Interval;
			command.Parameters.Add(parameter);

			parameter = new SqliteParameter("@lastasked", DbType.Int64);
			parameter.Value = LastAsked.Ticks;
			command.Parameters.Add(parameter);

			parameter = new SqliteParameter("@nextaskon", DbType.Int64);
			parameter.Value = NextAskOn.Ticks;
			command.Parameters.Add(parameter);

			parameter = new SqliteParameter("@order", DbType.Int32);
			parameter.Value = Order;
			command.Parameters.Add(parameter);

			parameter = new SqliteParameter("@previousinterval", DbType.Int32);
			parameter.Value = PreviousInterval;
			command.Parameters.Add(parameter);

			parameter = new SqliteParameter("@responsequality", DbType.Int32);
			parameter.Value = ResponseQuality;
			command.Parameters.Add(parameter);

			parameter = new SqliteParameter("@title", DbType.String);
			parameter.Value = Title;
			command.Parameters.Add(parameter);

			command.CommandText = sql;

			if (updating)
			{
				command.ExecuteNonQuery();
				return Id;
			}
			else
			{
				Int64 newId = (Int64)command.ExecuteScalar();
				return Convert.ToInt32(newId);
			}
		}
	}
}
