using ShopingTask.Model;
using ShopingTask.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ShopingTask.ViewModel
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        #region onproperty

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string Property = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
            }
        }

        #endregion;


        /// <summary>
        /// constructor that contains initialization 
        /// </summary>
        public SearchViewModel()
        {
            shoppingServiceObjcet = new ShoppingItemService();
            ShoppingContentList = new ObservableCollection<Content>();
            SuggestionsList = new List<string>();
            GetShoppingList();
        }

        //shooping service object that will help the viewmodel to make REST calls
        ShoppingItemService shoppingServiceObjcet;

        /// <summary>
        /// getting shopping list items
        /// </summary>
        public async void GetShoppingList()
        {
            ProgressRingVisibility = true;
            var temp = await shoppingServiceObjcet.GetShoppItems();
            foreach (var item in temp)
                ShoppingContentList.Add(item);
            ProgressRingVisibility = false;

        }

        /// <summary>
        /// loading more shopping items when we try to scroll
        /// </summary>
        public async void LoadMoreItemWithScrolling()
        {
           var temp = await shoppingServiceObjcet.GetShoppItems();
            foreach (var item in temp)
                ShoppingContentList.Add(item);

        }

        /// <summary>
        /// 
        ///filtering shops items by article or gender or both togther
        /// </summary>
        /// <param name="article"></param>
        /// <param name="gender"></param>
        public async void FilterShoppingItem(string article, string gender)
        {
            if (ShoppingContentList != null)
                ShoppingContentList.Clear();
            var temp = await shoppingServiceObjcet.Filter(article, gender);
            foreach (var item in temp)
                ShoppingContentList.Add(item);

        }

        /// <summary>
        /// loading list of suggestion for user, from the original list
        /// </summary>
        private void LoadSuggestionsList()
        {
            if (SuggestionsList != null)
                SuggestionsList.Clear();
            foreach (var item in ShoppingContentList)
                suggestionsList.Add(item.name.ToLower());
            var search_term = suggestedKeyword.ToLower();
            var results = suggestionsList.Where(i => i.Contains(search_term)).ToList();
            SuggestionsList = results;
        }

        #region properties

        private List<string> suggestionsList;

        public List<string> SuggestionsList
        {
            get { return suggestionsList; }
            set
            {
                suggestionsList = value;
                NotifyPropertyChanged();
            }
        }

        private string suggestedKeyword;

        public string SuggestedKeyword
        {
            get { return suggestedKeyword; }
            set
            {
                suggestedKeyword = value;
                if (suggestedKeyword != null)
                {
                    LoadSuggestionsList();
                }
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Content> shoppingContentList;

        public ObservableCollection<Content> ShoppingContentList
        {
            get { return shoppingContentList; }
            set
            {
                shoppingContentList = value;
                NotifyPropertyChanged();
            }
        }

        private bool progressRingVisibility;
        public bool ProgressRingVisibility
        {
            get { return progressRingVisibility; }
            set
            {
                progressRingVisibility = value;
                NotifyPropertyChanged();
            }
        }
        #endregion



    }

}

