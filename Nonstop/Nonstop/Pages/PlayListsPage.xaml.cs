using Nonstop.Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nonstop.Forms.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlayListsPage : ContentPage
	{
        ObservableCollection<PlaylistCell> playlists { get; set; }
        public PlayListsPage ()
		{
            
            InitializeComponent ();

            playlists = new ObservableCollection<PlaylistCell>();
            playlists.Add(new PlaylistCell { Title = "Title1", TotalCount = "12", Image = "https://www.iconsdb.com/icons/preview/light-gray/square-xxl.png" });
            playlists.Add(new PlaylistCell { Title = "Title2", TotalCount = "23", Image = "https://www.iconsdb.com/icons/preview/light-gray/square-xxl.png" });
            playlists.Add(new PlaylistCell { Title = "Title3", TotalCount = "12", Image = "https://www.iconsdb.com/icons/preview/light-gray/square-xxl.png" });
            playlists.Add(new PlaylistCell { Title = "Title4", TotalCount = "45", Image = "https://www.iconsdb.com/icons/preview/light-gray/square-xxl.png" });
            listView.ItemsSource = playlists;
        }
	}
    
}