using System;
using System.Collections.Generic;

namespace RottenTomatoesBusinessLogic
{
	public class CriticInfo
	{
		public CriticInfo ()
		{
		}

		public String NameOfCritic { get ; set ;}
		public String MediaSourceOfCritc { get ; set ;}
		public Boolean? FreshOrRotten { get ; set ;}
		public String BriefCriticReview { get ; set ;}
		public List<Link> Links = new List<Link>();
	}
}

