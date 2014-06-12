using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;

namespace RottenTomatoesBusinessLogic
{
	public class FillMoviesList : IFillMoviesList
	{

		public String API_KEY = "2npuvzd64n8v6cucsgcjd98z";
		public FillMoviesList ()
		{

		}

		private String getDateFormHHMM (String time)
		{
			// Converting time into X hr: YY m format 
			int amount = Convert.ToInt32 (time);
			int tempHr = 0;
			while (amount > 60) {

				amount = amount - 60;
				tempHr++;

			}
			String answer = Convert.ToString (tempHr) + "Hr. : " + Convert.ToString (amount)+" Min.";

			return answer;
		}

		private async Task<List<MovieEntry>> BoxOfficeMoviesHelper (String Url)
		{
			MovieList Movie_List = new MovieList ();

			MovieEntry Movie_Entry = null;

			var Client = new HttpClient ();

			var Result = Client.GetStringAsync (@Url);

			var JsonDataStructure = JObject.Parse (Result.Result);

			int Count_Movies = JsonDataStructure.Count;

			int Temp_Count = 0;

			while (Temp_Count < Count_Movies) {


				Movie_Entry = new MovieEntry ();

				Movie_Entry.MovieTitle = (String)JsonDataStructure ["movies"] [Temp_Count] ["title"];
				Movie_Entry.MPAA_Rating = (String)JsonDataStructure ["movies"] [Temp_Count] ["mpaa_rating"];
				Movie_Entry.Synopsis = (String)JsonDataStructure ["movies"] [Temp_Count] ["synopsis"];
				Movie_Entry.Runtime = getDateFormHHMM ((String)JsonDataStructure ["movies"] [Temp_Count] ["runtime"]);
				Movie_Entry.MovieThumbnail = (String)JsonDataStructure ["movies"] [Temp_Count] ["posters"] ["thumbnail"];

				if (((String)JsonDataStructure ["movies"] [Temp_Count] ["ratings"] ["critics_rating"]).Equals ("Certified Fresh")) {
					Movie_Entry.FreshOrRotten = true;
				} else if (((String)JsonDataStructure ["movies"] [Temp_Count] ["ratings"] ["critics_rating"]).Equals ("Rotten")) {
					Movie_Entry.FreshOrRotten = false;
				} else {
					Movie_Entry.FreshOrRotten = null;
				}

				Movie_Entry.OverallCriticScore = (String)JsonDataStructure ["movies"] [Temp_Count] ["ratings"] ["critics_score"];
				Movie_Entry.TheatreReleaseDate = (String)JsonDataStructure ["movies"] [Temp_Count] ["release_dates"] ["theater"];

				var JsonTempObject = (JArray)JsonDataStructure ["movies"] [Temp_Count] ["abridged_cast"];
				int Temp_Count1 = 0;

				while (Temp_Count1 < JsonTempObject.Count) {

					Cast Cast_Obj = new Cast ();
					IDictionary<string,JToken> dictionary =(JObject) JsonTempObject [Temp_Count1];
					if (dictionary.ContainsKey("characters")) {
						int CharachterCount = (((JArray)JsonTempObject [Temp_Count1] ["characters"]).Count) - 1;
						while (CharachterCount >= 0) {
							Cast_Obj.Charachter = Cast_Obj.Charachter + JsonTempObject [Temp_Count1] ["characters"] [CharachterCount] + " ";
							CharachterCount--;
						}
					} else {
						Cast_Obj.Charachter = "";
					}

					Cast_Obj.Name = (String)JsonTempObject [Temp_Count1] ["name"];

					Movie_Entry.AbridgedCast.Add (Cast_Obj);

					Temp_Count1++;

				}

				//Would need a new HTTP call for filling the other data.

				// HTTP call for CriticReview


				String WebUrl = ((String)(JsonDataStructure ["movies"] [Temp_Count] ["links"] ["reviews"] + "?apikey=" + API_KEY));

				HttpClient tempClient = new HttpClient ();
				var TempResults = tempClient.GetStringAsync (@WebUrl);
				//	tempClient.Dispose ();
				var TempJsonDataStructure = JObject.Parse (TempResults.Result);

				Temp_Count1 = 0;
				Critics Critics_Obj = new Critics ();
				while (Temp_Count1 < ((JArray)TempJsonDataStructure ["reviews"]).Count) {

					CriticInfo Critic_Info = new CriticInfo ();

					Critic_Info.BriefCriticReview = (String)TempJsonDataStructure ["reviews"] [Temp_Count1] ["quote"];
					Critic_Info.MediaSourceOfCritc = (String)TempJsonDataStructure ["reviews"] [Temp_Count1] ["publication"];
					Critic_Info.NameOfCritic = (String)TempJsonDataStructure ["reviews"] [Temp_Count1] ["critic"];

					if (((String)TempJsonDataStructure ["reviews"] [Temp_Count1] ["freshness"]).Equals ("fresh")) {
						Critic_Info.FreshOrRotten = true;
					} else if (((String)TempJsonDataStructure ["reviews"] [Temp_Count1] ["freshness"]).Equals ("rotten")) {
						Critic_Info.FreshOrRotten = false;
					} else {
						Critic_Info.FreshOrRotten = null;
					}

					Critics_Obj.Critic_Info.Add (Critic_Info);
					Temp_Count1++;
				}

				Link Link_Obj2 = new Link ();
				Link_Obj2.NameOfLink = "self";
				Link_Obj2.WebUrl = (String)TempJsonDataStructure ["links"] ["self"]+ "?apikey=" + API_KEY;
				Critics_Obj.Links.Add (Link_Obj2);

				Link_Obj2 = new Link ();
				Link_Obj2.NameOfLink = "next";
				Link_Obj2.WebUrl = (String)TempJsonDataStructure ["links"] ["next"]+ "?apikey=" + API_KEY;
				Critics_Obj.Links.Add (Link_Obj2);

				Link_Obj2 = new Link ();
				Link_Obj2.NameOfLink = "alternate";
				Link_Obj2.WebUrl = (String)TempJsonDataStructure ["links"] ["alternate"]+ "?apikey=" + API_KEY;
				Critics_Obj.Links.Add (Link_Obj2);

				Link_Obj2 = new Link ();
				Link_Obj2.NameOfLink = "rel";
				Link_Obj2.WebUrl = (String)TempJsonDataStructure ["links"] ["rel"]+ "?apikey=" + API_KEY;
				Critics_Obj.Links.Add (Link_Obj2);

				Movie_Entry.ListOfCritics = Critics_Obj;


				// HTTP call for Cast So call to main Cast page

				WebUrl = "";
				TempJsonDataStructure = null;
				TempResults = null;
				@WebUrl = (String)JsonDataStructure ["movies"] [Temp_Count] ["links"] ["cast"] + "?apikey=" + API_KEY;
				tempClient = new HttpClient ();

				TempResults = tempClient.GetStringAsync (@WebUrl);
				//tempClient.Dispose ();
				TempJsonDataStructure = JObject.Parse (TempResults.Result);

				Temp_Count1 = 0;

				while (Temp_Count1 < TempJsonDataStructure.Count) {

					Cast Cast_Obj = new Cast ();
					IDictionary<string,JToken> dictionary =(JObject) TempJsonDataStructure ["cast"][Temp_Count1];
					if (dictionary.ContainsKey ("characters")) {
						int CharachterCount = (((JArray)TempJsonDataStructure ["cast"] [Temp_Count1] ["characters"]).Count) - 1;
						while (CharachterCount >= 0) {
							Cast_Obj.Charachter = Cast_Obj.Charachter + TempJsonDataStructure ["cast"] [Temp_Count1] ["characters"] [CharachterCount] + " ";
							CharachterCount--;
						}
					} else {
						Cast_Obj.Charachter = "";
					}
					Cast_Obj.Name = (String)TempJsonDataStructure["cast"] [Temp_Count1] ["name"];

					Movie_Entry.CompleteCast.Add (Cast_Obj);

					Temp_Count1++;

				}

				// HTTP call for Directors and Genre's

				WebUrl = "";
				TempJsonDataStructure = null;
				TempResults = null;
				@WebUrl = (String)JsonDataStructure ["movies"] [Temp_Count] ["links"] ["self"] + "?apikey=" + API_KEY;
				tempClient = new HttpClient ();


				TempResults = tempClient.GetStringAsync (@WebUrl);
				//tempClient.Dispose ();
				TempJsonDataStructure = JObject.Parse (TempResults.Result);

				int DirectorCount = ((JArray)TempJsonDataStructure ["abridged_directors"]).Count-1;

				while (DirectorCount >= 0) {

					Movie_Entry.Directors.Add ((String)TempJsonDataStructure ["abridged_directors"] [DirectorCount] ["name"]);

					DirectorCount--;

				}

				int GenreCount = ((JArray)TempJsonDataStructure ["genres"]).Count-1;

				while (GenreCount >= 0) {

					Movie_Entry.Genres.Add ((String)TempJsonDataStructure ["genres"] [GenreCount]);
					GenreCount--;
				}
				Link Link_Obj = new Link ();
				Link_Obj.NameOfLink = "self";
				Link_Obj.WebUrl = (String)JsonDataStructure ["movies"] [Temp_Count] ["links"] ["self"] + "?apikey=" + API_KEY;
				Movie_Entry.Links.Add (Link_Obj);

				Link_Obj = new Link ();
				Link_Obj.NameOfLink = "alternate";
				Link_Obj.WebUrl = (String)JsonDataStructure ["movies"] [Temp_Count] ["links"] ["alternate"] + "?apikey=" + API_KEY;
				Movie_Entry.Links.Add (Link_Obj);

				Link_Obj = new Link ();
				Link_Obj.NameOfLink = "cast";
				Link_Obj.WebUrl = (String)JsonDataStructure ["movies"] [Temp_Count] ["links"] ["cast"] + "?apikey=" + API_KEY;
				Movie_Entry.Links.Add (Link_Obj);

				Link_Obj = new Link ();
				Link_Obj.NameOfLink = "clips";
				Link_Obj.WebUrl = (String)JsonDataStructure ["movies"] [Temp_Count] ["links"] ["clips"] + "?apikey=" + API_KEY;
				Movie_Entry.Links.Add (Link_Obj);

				Link_Obj = new Link ();
				Link_Obj.NameOfLink = "reviews";
				Link_Obj.WebUrl = (String)JsonDataStructure ["movies"] [Temp_Count] ["links"] ["reviews"] + "?apikey=" + API_KEY;
				Movie_Entry.Links.Add (Link_Obj);

				Link_Obj = new Link ();
				Link_Obj.NameOfLink = "similar";
				Link_Obj.WebUrl = (String)JsonDataStructure ["movies"] [Temp_Count] ["links"] ["similar"] + "?apikey=" + API_KEY;
				Movie_Entry.Links.Add (Link_Obj);


				Movie_List.ListOfMovies.Add (Movie_Entry);
				Temp_Count++;

			}

			return Movie_List.ListOfMovies;

		}


		#region IFillMoviesList implementation

		public async Task<ListOfMoviesFromBoxOffice> BoxOfficeMovies(){
		
			ListOfMoviesFromBoxOffice ListOfMovies = new ListOfMoviesFromBoxOffice ();

			int Count = 0;

			while (Count < 3) {
			
				if (Count == 0) {
				
					ListOfMovies.In_Theatres = await BoxOfficeMoviesHelper (@"http://api.rottentomatoes.com/api/public/v1.0/lists/movies/in_theaters.json?apikey=" + API_KEY);
				}

				if (Count == 1) {
					ListOfMovies.Opening_Movies = await BoxOfficeMoviesHelper (@"http://api.rottentomatoes.com/api/public/v1.0/lists/movies/opening.json?apikey=" + API_KEY);
				
				}

				if (Count == 2) {
				
					ListOfMovies.Top_BoxOffice = await BoxOfficeMoviesHelper (@"http://api.rottentomatoes.com/api/public/v1.0/lists/movies/box_office.json?apikey=" + API_KEY);
				}

				Count++;
			}

			return ListOfMovies;
		}

		#endregion
	}

	public interface IFillMoviesList
	{

		Task<ListOfMoviesFromBoxOffice> BoxOfficeMovies ();


	}
}

