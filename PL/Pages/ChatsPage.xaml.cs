﻿using DalFacade.DO;
using PL.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PL.Pages
{
    public partial class ChatsPage : INotifyPropertyChanged
    {
        public ChatsViewModel personalChats { get; set; }
        public User user { get; set; }

        public ChatsPage(List<Customer> customers, IEnumerable<Chat> allChats, User user)
        {
            personalChats = new ChatsViewModel(customers, ref allChats, user);
            this.user = user;
            UserList = customers.Select(c => c.name).ToList();

            InitializeComponent();
            DataContext = this;
        }

        private string _searchText = "";

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
                OnPropertyChanged("MyFilteredItems");
            }
        }

        public List<string> UserList { get; set; }

        public IEnumerable<string> MyFilteredItems => UserList.Where(x => x.ToUpper().StartsWith(SearchText.ToUpper()));

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
