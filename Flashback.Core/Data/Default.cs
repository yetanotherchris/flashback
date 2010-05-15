using System;

namespace Flashback.Core.Data
{
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
