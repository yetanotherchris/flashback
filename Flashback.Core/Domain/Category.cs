using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;

namespace Flashback.Core
{
	/// <summary>
	/// The category for a question.
	/// </summary>
	public partial class Category : BaseDataObject<Category>
	{
		/// <summary>
		/// The name of the category
		/// </summary>
		public string Name { get; set; }
	}
}
