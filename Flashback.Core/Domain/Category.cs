using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vici.CoolStorage;

namespace Flashback.Core
{
	[MapTo("categories")]
	public class Category : CSObject<Category,Guid>
	{
		public Guid Id
		{
			get { return (Guid)GetField("Id"); }
			set { SetField("Id", value); }
		}

		public string Name
		{
			get { return (string)GetField("Name"); }
			set { SetField("Name", value); }
		}

		[OneToMany]
		[Lazy]
		public CSList<Question> Questions
		{
			get { return (CSList<Question>)GetField("Questions"); }
			set { SetField("Questions", value); }
		}
	}
}
