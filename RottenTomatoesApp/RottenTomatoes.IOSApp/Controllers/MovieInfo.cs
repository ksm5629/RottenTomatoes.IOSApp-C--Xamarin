
using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using RottenTomatoesBusinessLogic;
namespace RottenTomatoes.IOSApp
{
	public partial class MovieInfo : DialogViewController
	{
		public MovieInfo () : base (UITableViewStyle.Grouped, null)
		{
		
		}

		public RootElement getUI(MovieEntry entry){
		
			NSUrl url = new NSUrl(entry.MovieThumbnail);
			NSData data = NSData.FromUrl(url);
			UIImage MovieThumb = new UIImage (data);

			UIImage Rotten = UIImage.FromBundle ("rotten.png");
			UIImage Fresh = UIImage.FromBundle ("fresh.png");
			UIImage DontKnow = UIImage.FromBundle ("QuestionMark.png");

			System.Drawing.SizeF size = new System.Drawing.SizeF ();
			size.Height = 3;
			size.Width = 3;
			Rotten.Scale (size);
			Fresh.Scale (size);
			DontKnow.Scale (size);

			String Director_String=""; 
			String Genre_String = "";
			foreach(var directors in entry.Directors){

				Director_String = Director_String +  +directors+" " ;
			}

			foreach (var genre in entry.Genres) {
			
				Genre_String = Genre_String +genre+" ";
			}

			var Actor_Section = new Section ("Actors");

			Console.WriteLine ("CompleteCast count"+entry.CompleteCast.Count);
			Console.WriteLine ("Abridged Cast Count" + entry.AbridgedCast.Count);

			foreach (var actor in entry.AbridgedCast) {

				var elem = new StyledStringElement (actor.Name , actor.Charachter, UITableViewCellStyle.Subtitle);
				Actor_Section.Add (elem);
				
			}

			var Reviews_Section = new Section ("Reviews");

			foreach (var review in entry.ListOfCritics.Critic_Info) {
				BadgeElement elem = null;
				if(review.FreshOrRotten==true){
					elem = new BadgeElement (Fresh, review.NameOfCritic+", "+review.MediaSourceOfCritc+Environment.NewLine+Environment.NewLine+review.BriefCriticReview);
					elem.LineBreakMode = UILineBreakMode.WordWrap;
					elem.Lines = 0;
					elem.Font = UIFont.FromName ("AmericanTypeWriter", 8f);
				}else if(review.FreshOrRotten==false){
					elem = new BadgeElement (Rotten, review.NameOfCritic+", "+review.MediaSourceOfCritc+Environment.NewLine+Environment.NewLine+review.BriefCriticReview);
					elem.LineBreakMode = UILineBreakMode.WordWrap;
					elem.Lines = 0;
					elem.Font = UIFont.FromName ("AmericanTypeWriter", 8f);
				}else{
					elem = new BadgeElement (DontKnow, review.NameOfCritic+", "+review.MediaSourceOfCritc+Environment.NewLine+Environment.NewLine+review.BriefCriticReview);
					elem.LineBreakMode = UILineBreakMode.WordWrap;
					elem.Lines = 0;
					elem.Font = UIFont.FromName ("AmericanTypeWriter", 8f);
				}

				Reviews_Section.Add (elem);
			}

			var Movie_Title = new Section (entry.MovieTitle);

			var Movie_Title_Element = new BadgeElement(MovieThumb,entry.OverallCriticScore +"% Of Critics liked it." + Environment.NewLine + entry.AbridgedCast [0].Name + ", " + entry.AbridgedCast [1].Name + Environment.NewLine + entry.MPAA_Rating + ", " + entry.Runtime + Environment.NewLine + entry.TheatreReleaseDate);

			Movie_Title_Element.Font = UIFont.FromName ("AmericanTypeWriter", 10f);
			Movie_Title_Element.LineBreakMode = UILineBreakMode.WordWrap;
			Movie_Title_Element.Lines = 0;

			Movie_Title.Add (Movie_Title_Element);

			var Movie_Info_Section = new Section ("Movie Details");
			var DirectorElem = new StyledStringElement("Director Names: ",Director_String,UITableViewCellStyle.Subtitle);
			var RatingElem = new StyledStringElement ("Rated: ", entry.MPAA_Rating, UITableViewCellStyle.Subtitle);
			var RunningElem = new StyledStringElement ("Running Time: ", entry.Runtime, UITableViewCellStyle.Subtitle);
			var GenreElem = new StyledStringElement ("Genre: ", Genre_String, UITableViewCellStyle.Subtitle);
			var TheatreReleaseElem = new StyledStringElement ("Theatre Release Date: ", entry.TheatreReleaseDate, UITableViewCellStyle.Subtitle);


			Movie_Info_Section.Add (DirectorElem);
			Movie_Info_Section.Add (RatingElem);
			Movie_Info_Section.Add (RunningElem);
			Movie_Info_Section.Add (GenreElem);
			Movie_Info_Section.Add (TheatreReleaseElem);

			var Synopsis_Sec = new Section ("Synopsis");

			var SysnopsisElem = new MultilineElement ("Synopsis", entry.Synopsis);

			Synopsis_Sec.Add (SysnopsisElem);

			var Links_Section = new Section ("Links");
			var Root_Element = new RootElement ("MOVIE DETAILS");

			Root_Element.Add (Movie_Title);
			Root_Element.Add (Movie_Info_Section);
	
			Root_Element.Add (Actor_Section);

			Root_Element.Add (Reviews_Section);

			return Root_Element;
		}
	}
}
