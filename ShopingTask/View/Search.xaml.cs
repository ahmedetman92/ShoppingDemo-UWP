using ShopingTask.Model;
using ShopingTask.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopingTask.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Search : Page
    {
         List<string> suggestionsList = new List<string>();
        ShoppingItemService obj = new ShoppingItemService();

        public Search()
        {
            this.InitializeComponent();
        }

        private async void txtAutoComplete_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var search_term = args.QueryText;
            var results = suggestionsList.Where(i => i.StartsWith(search_term)).ToList();
            txtAutoComplete.ItemsSource = results;
            txtAutoComplete.IsSuggestionListOpen = true;
            var mylist = await obj.Filter(txtAutoComplete.Text,"" );
            itemList.ItemsSource = mylist;

        }

        private  void btn_Male_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.FilterShoppingItem(txtAutoComplete.Text, "male");
            itemList.ItemsSource = App.ViewModel.ShoppingContentList; ;
        }

       

        private void btn_Female_Click(object sender, RoutedEventArgs e)
        {
             App.ViewModel.FilterShoppingItem(txtAutoComplete.Text, "female");
            itemList.ItemsSource = App.ViewModel.ShoppingContentList; ;
        }

        private  void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            Progress.IsActive = true;
            ScrollViewer scrollControl = sender as ScrollViewer;
            if (scrollControl.VerticalOffset == (scrollControl.ExtentHeight - scrollControl.ViewportHeight))
            {
                App.ViewModel.LoadMoreItemWithScrolling();
                itemList.ItemsSource = App.ViewModel.ShoppingContentList;
            }
            Progress.IsActive = false;
        }
    }
}