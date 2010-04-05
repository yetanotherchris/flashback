using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using System.Data;

namespace Flashback.Core
{
	public partial class Category : BaseDataObject<Category>
	{
		protected override string TableName
		{
			get
			{
				return "categories";
			}
		}

		protected override Category GetRow(SqliteDataReader reader)
		{
			Category category = new Category();
			category.Id = reader.GetInt32(0);
			category.Name = reader.GetString(1);

			return category;
		}

		protected override int Save(SqliteCommand command, bool updating)
		{
			SqliteParameter parameter;
			string sql = @"INSERT INTO categories VALUES (@name);SELECT last_insert_rowid();";

			if (updating)
			{
				sql = @"UPDATE categories SET Name=@name WHERE id=@id";
				parameter = new SqliteParameter("@id", DbType.Int32);
				parameter.Value = Id;
				command.Parameters.Add(parameter);
			}

			parameter = new SqliteParameter("@name", DbType.String);
			parameter.Value = Name;
			command.Parameters.Add(parameter);

			command.CommandText = sql;
			int newId = (int) command.ExecuteScalar();

			if (updating)
				return Id;
			else
				return newId;
		}
	}
}
