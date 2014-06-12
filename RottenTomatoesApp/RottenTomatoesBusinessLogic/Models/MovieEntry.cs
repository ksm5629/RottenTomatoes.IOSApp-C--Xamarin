using System;
using System.Collections.Generic;
namespace RottenTomatoesBusinessLogic
{
	public class MovieEntry
	{
		public MovieEntry ()
		{
		}

		public String MovieTitle { get ; set ;}
		public String MovieThumbnail { get ; set ;}
		public Boolean? FreshOrRotten { get ; set ;}
		public String OverallCriticScore { get ; set ;}
		public List<Cast> AbridgedCast = new List<Cast> ();
		public List<Cast> CompleteCast = new List<Cast> ();
		public String MPAA_Rating { get ; set ;}
		public String Runtime { get ; set ;}
		public Critics ListOfCritics { get ; set ;}
		public String Synopsis { get ; set ;}
		public List<String> Genres = new List<String> ();
		public List<String> Directors = new List<String> ();
		public String TheatreReleaseDate { get ; set ;}
		public List<Link> Links = new List<Link>();
	}
}

