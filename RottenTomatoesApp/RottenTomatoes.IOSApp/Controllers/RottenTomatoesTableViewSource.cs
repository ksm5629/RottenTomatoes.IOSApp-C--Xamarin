using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using RottenTomatoesBusinessLogic;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace RottenTomatoes.IOSApp
{
	public class RottenTomatoesTableViewSource:UITableViewSource
	{
		public  IList<TableGroupItems> Custom_Table_Items {get;set;}
		public IList<MovieEntry> Movies  {get;set;}
		private const string ValueOfCell = "Id";
		private NSString Cell_ID = (NSString)"ID";
		public Action<int,int> OnRowSelect;


		/*
		public RottenTomatoesTableViewSource ( IEnumerable<MovieEntry> ArgMovies)
		{
			Movies = new List<MovieEntry> (ArgMovies);
		}
*/

		public RottenTomatoesTableViewSource ( IEnumerable<TableGroupItems> items)
		{
			Custom_Table_Items = new List<TableGroupItems>(items);
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);

			if (OnRowSelect != null)
			{
				OnRowSelect(indexPath.Section,indexPath.Row);
			}
		}
		/*
		public override int RowsInSection(UITableView tableview, int section)
		{
			return Movies.Count;
		}
		*/
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return Custom_Table_Items[section].Name;
		}

		public override int NumberOfSections (UITableView tableView)
		{
			return Custom_Table_Items.Count;
		}
		public override int RowsInSection (UITableView tableview, int section)
		{
			return Custom_Table_Items[section].Items.Count;
		}
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{


			var cell = tableView.DequeueReusableCell (ValueOfCell) as CustomCell;
			if (cell == null) {
				cell = new CustomCell (Cell_ID);
			}

			var entry = Custom_Table_Items [indexPath.Section].Items [indexPath.Row];
			cell.UpdateCell (entry.OverallCriticScore,entry.AbridgedCast[0].Name+", "+entry.AbridgedCast[1].Name," ,"+entry.Runtime,entry.TheatreReleaseDate,entry.MovieTitle,entry.MovieThumbnail,entry.FreshOrRotten,entry.MPAA_Rating);
			return cell;


			/*


			var cell = tableView.DequeueReusableCell(ValueOfCell) ??
				new UITableViewCell(UITableViewCellStyle.Subtitle, ValueOfCell);

			var entry = Movies[indexPath.Row];
			cell.TextLabel.Text = entry.MovieTitle;
			NSUrl nsUrl = new NSUrl(entry.MovieThumbnail);
			NSData data = NSData.FromUrl(nsUrl);
			cell.ImageView.Image = new UIImage (data);
			Boolean? RotOrFresh = entry.FreshOrRotten;
			UIImageView Rotten_Or_Fresh_Image = new UIImageView();

			if (RotOrFresh == true) {
				Rotten_Or_Fresh_Image.Image = UIImage.FromBundle ("fresh.png");
			} else if (RotOrFresh == false) {
				Rotten_Or_Fresh_Image.Image = UIImage.FromBundle ("rotten.png");
			} else {
				Rotten_Or_Fresh_Image.Image = UIImage.FromBundle ("QuestionMark.png");
			}

			cell.DetailTextLabel.Text = entry.OverallCriticScore + Environment.NewLine+ entry.AbridgedCast [0].Name + ", " + entry.AbridgedCast [1].Name + Environment.NewLine + entry.MPAA_Rating + ", " + entry.Runtime + Environment.NewLine + entry.TheatreReleaseDate;
			cell.DetailTextLabel.Font = UIFont.FromName ("AmericanTypeWriter", 8f);

			cell.DetailTextLabel.LineBreakMode = UILineBreakMode.WordWrap;
			cell.DetailTextLabel.Lines = 0;

			RectangleF Cell_Props = cell.Frame;
			Cell_Props.Height = 150f;
			cell.Frame = Cell_Props;
			return cell;

			*/
		}

		public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 99f;
		}
	}
}

