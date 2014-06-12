using System;
using System.Collections.Generic;
using RottenTomatoesBusinessLogic;
namespace RottenTomatoes.IOSApp
{
	public class TableGroupItems
	{
		public TableGroupItems (String Name)
		{
			this.Name = Name;
		}

		public string Name { get; set;}

		public List<MovieEntry> Items
		{
			get { return items; }
			set { items = value; }
		}

		protected List<MovieEntry> items = new List<MovieEntry> ();
	}
}

