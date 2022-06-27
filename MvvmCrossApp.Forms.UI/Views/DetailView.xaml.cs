using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using MvvmCrossApp.Core.ViewModels;
using Xamarin.Forms;

namespace MvvmCrossApp.Forms.UI.Views
{
    public partial class DetailView : MvxContentPage<DetailMedicineViewModel>
    {
        public DetailView()
        {
            InitializeComponent();
        }
    }
}

