using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using RottenTomatoesBusinessLogic;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
namespace RottenTomatoes.IOSApp
{
	public class RottenTomatoesTableViewController : UITableViewController
	{
		private readonly UINavigationController RTNavigationController;
		private readonly FillMoviesList MovieListRepository = new FillMoviesList();

		public List<TableGroupItems> List_Items = new List<TableGroupItems> ();
		RottenTomatoesTableViewSource C_Source;


		public RottenTomatoesTableViewController (UINavigationController RTnavigationController)
		{
			RTNavigationController = RTnavigationController;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			C_Source = new RottenTomatoesTableViewSource(Enumerable.Empty<TableGroupItems>());

			TableView = new UITableView(Rectangle.Empty) {Source = C_Source};

			RefreshControl = new UIRefreshControl();

			C_Source.OnRowSelect = OnRowSelect;
			RefreshControl.ValueChanged += RefreshControlOnValueChanged;
		}

		private async void RefreshControlOnValueChanged(object sender, EventArgs eventArgs)
		{
			await LoadMoviesAsync();
			RefreshControl.EndRefreshing();
		}
		private void OnRowSelect(int CSection, int RItemNo )
		{
			var entry = C_Source.Custom_Table_Items [CSection].Items [RItemNo];
			//	RTNavigationController.PushViewController(new CompleteMovieInfoController(entry), true);
			MovieInfo Movie_Info = new MovieInfo ();

			RootElement Root_Elem = Movie_Info.getUI (entry);

			var DialogViewController_Obj = new DialogViewController (Root_Elem, true);

			RTNavigationController.PushViewController(DialogViewController_Obj, true);


		}
		public async override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			await LoadMoviesAsync();
		}
		private async Task LoadMoviesAsync()
		{
			var data = await MovieListRepository.BoxOfficeMovies();

			/*
	C_Source.Movies = new List<MovieEntry>(data.In_Theatres);

			foreach (MovieEntry me in data.Opening_Movies) {
				C_Source.Movies.Add (me);
			}
			foreach (MovieEntry me in data.Top_BoxOffice) {
				C_Source.Movies.Add (me);
			}
			*/
			C_Source.Movies = new List<MovieEntry>(data.In_Theatres);
			C_Source.Custom_Table_Items = new List<TableGroupItems> ();

			TableGroupItems CGroup = new TableGroupItems ("In Theatres");
			C_Source.Custom_Table_Items.Add (CGroup);

			foreach (MovieEntry me in data.In_Theatres) {
				CGroup.Items.Add (me);
			}
			CGroup = new TableGroupItems ("Opening In Theatres");
			C_Source.Custom_Table_Items.Add (CGroup);

			foreach (MovieEntry me in data.Opening_Movies) {
				CGroup.Items.Add (me);
			}

			CGroup = new TableGroupItems ("Top Box Office");
			C_Source.Custom_Table_Items.Add (CGroup);

			foreach (MovieEntry me in data.Top_BoxOffice) {
				CGroup.Items.Add (me);
			}
			TableView.ReloadData();
		}

	}
}

