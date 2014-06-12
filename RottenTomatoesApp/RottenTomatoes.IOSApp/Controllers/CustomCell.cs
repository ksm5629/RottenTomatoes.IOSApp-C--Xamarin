using System;
using System.Drawing;
using RottenTomatoesBusinessLogic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreFoundation;
using MonoTouch.ImageIO;
using System.Threading.Tasks;
namespace RottenTomatoes.IOSApp
{
	public class CustomCell:UITableViewCell
	{
		UILabel Movie_Title_Label , MPAA_Rating_Label , Critic_Rating_Label , Abridged_Cast_Label
		, Runtime_Label , Theatre_Release_Date_Label;
		UIImageView Movie_Thumbnail_Image , Rotten_Or_Fresh_Image;
		UIView Custom_View ;
		public CustomCell (NSString Cell_ID) : base(UITableViewCellStyle.Default,Cell_ID)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Gray;
			ContentView.BackgroundColor = UIColor.White;
			Movie_Thumbnail_Image = new UIImageView ();
			Rotten_Or_Fresh_Image = new UIImageView ();

			Movie_Title_Label = new UILabel () {
				Font = UIFont.FromName ("AmericanTypeWriter", 14f)
			};

			MPAA_Rating_Label = new UILabel () {
				Font = UIFont.FromName ("AmericanTypeWriter", 10f)
			};

			Critic_Rating_Label = new UILabel () {
				Font = UIFont.FromName ("AmericanTypeWriter", 10f)
			};

			Abridged_Cast_Label = new UILabel () {
				Font = UIFont.FromName ("AmericanTypeWriter", 10f)
			};

			Runtime_Label = new UILabel () {
				Font = UIFont.FromName ("AmericanTypeWriter", 10f)
			};

			Theatre_Release_Date_Label = new UILabel () {
				Font = UIFont.FromName ("AmericanTypeWriter", 10f)
			};

			Custom_View = new UIView ();
			ContentView.Add (Critic_Rating_Label);
			ContentView.Add (Abridged_Cast_Label);
			ContentView.Add (Runtime_Label);
			ContentView.Add (Theatre_Release_Date_Label);
			ContentView.Add (Rotten_Or_Fresh_Image);
			ContentView.Add (MPAA_Rating_Label);

			ContentView.Add (Movie_Title_Label);

			//ContentView.Add (Movie_Thumbnail_Image);

		}

		public void UpdateCell (string CriticRating, string AbridgedCast,String Runtime ,String TheatreReleaseDate , String MovieTitle, String ThumbUrl , Boolean? RotOrFresh,string MPAARating)
		{
			NSUrl nsUrl = new NSUrl(ThumbUrl);
			NSData data = NSData.FromUrl(nsUrl);
			Movie_Thumbnail_Image.Image = new UIImage (data);
			ImageView.Image = Movie_Thumbnail_Image.Image;
			Critic_Rating_Label.Text = CriticRating+"%";
			Abridged_Cast_Label.Text = AbridgedCast;
			Runtime_Label.Text = Runtime;
			Theatre_Release_Date_Label.Text = TheatreReleaseDate;
			Movie_Title_Label.Text = MovieTitle;
			MPAA_Rating_Label.Text = MPAARating;
			Critic_Rating_Label.LineBreakMode = UILineBreakMode.WordWrap;
			Critic_Rating_Label.Lines = 0;
			Abridged_Cast_Label.LineBreakMode = UILineBreakMode.WordWrap;
			Abridged_Cast_Label.Lines = 0;
			Runtime_Label.LineBreakMode = UILineBreakMode.WordWrap;
			Runtime_Label.Lines = 0;
			Theatre_Release_Date_Label.LineBreakMode = UILineBreakMode.WordWrap;
			Theatre_Release_Date_Label.Lines = 0;
			MPAA_Rating_Label.LineBreakMode = UILineBreakMode.WordWrap;
			MPAA_Rating_Label.Lines = 0;
			Movie_Title_Label.LineBreakMode = UILineBreakMode.WordWrap;
			Movie_Title_Label.Lines = 0;
			Add(Critic_Rating_Label);
			Add (Abridged_Cast_Label);
			Add (Runtime_Label);
			Add (Theatre_Release_Date_Label);
			Add (Movie_Title_Label);
			if (RotOrFresh == true) {
				Rotten_Or_Fresh_Image.Image = UIImage.FromBundle ("fresh.png");
			} else if (RotOrFresh == false) {
				Rotten_Or_Fresh_Image.Image = UIImage.FromBundle ("rotten.png");
			} else {
				Rotten_Or_Fresh_Image.Image = UIImage.FromBundle ("QuestionMark.png");
			}

		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			Critic_Rating_Label.Frame = new RectangleF (98, 25,25, 25);
			MPAA_Rating_Label.Frame = new RectangleF (80, 50,50, 25);
			Abridged_Cast_Label.Frame = new RectangleF (80, 37,300, 25);
			Runtime_Label.Frame=new RectangleF(115,50 ,100, 25);
			Theatre_Release_Date_Label.Frame=new RectangleF(80, 65,150, 25);;
			Rotten_Or_Fresh_Image.Frame = new RectangleF(80, 29, 15, 15);
		

			Movie_Title_Label.Frame=new RectangleF(80, 10, 300, 25);
			//Movie_Thumbnail_Image.Frame=new RectangleF(5, 4, ContentView.Bounds.Width-20, ContentView.Bounds.Height);

		}


	}
}

