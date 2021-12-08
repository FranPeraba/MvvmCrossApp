using System;
using System.Threading;
using Android.Views;
using Android.Widget;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.ViewModels;

namespace MvvmCrossApp.Droid.Views
{
    public class MedicinesViewHolder : MvxRecyclerViewHolder
    {
        TextView _itemName;
        Medicines _item;
        CancellationTokenSource _cancellationTokenSource;

        public MedicinesViewHolder(View itemView, IMvxAndroidBindingContext context, SearchViewModel vm) : base(itemView, context)
        {
            VM = vm;

            _itemName = itemView.FindViewById<TextView>(Resource.Id.item_name);

            itemView.Click -= Cell_Click;
            itemView.Click += Cell_Click;
        }

        public SearchViewModel VM { get; set; }

        public Medicines Item
        {
            get => _item;
            set => Refresh(value);
        }

        public void Refresh(Medicines item)
        {
            if (item is null)
                return;

            _item = item;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            _itemName.Text = _item.Nombre;
        }

        void Cell_Click(object sender, EventArgs e)
        {

        }
    }
}
