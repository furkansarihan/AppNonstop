using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nonstop.Forms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage(string loadingMessage)
        {
            InitializeComponent();

            // Set the message text
            LoadingPageMessage.Text = @"""" + loadingMessage + @"""";
        }
    }
}