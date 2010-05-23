using System;

namespace Flashback.Core.Data
{
	/// <summary>
	/// The default SQL script all versions of the app ship with.
	/// </summary>
	public class Default
	{
		public static string Sql()
		{
			return @"
insert into categories (name,inbuilt,active) values ('Default category',0,1);
insert into questions(categoryid,title,answer) values (1,'Example question','Example answer');";
		}
	}
}
