using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.ViewModels;

namespace MvvmCrossApp.Droid.Views
{
    public class MedicinesAdapter : MvxRecyclerAdapter
    {
        List<Medicines> _itemsSource;

        public MedicinesAdapter(IMvxAndroidBindingContext bindingContext) : base(bindingContext)
        {
        }

        public SearchViewModel VM { get; set; }

        protected override void SetItemsSource(IEnumerable value)
        {
            base.SetItemsSource(value);

            if (value == null)
                return;

            FillSourceList();
        }

        void FillSourceList()
        {
            _itemsSource = new List<Medicines>();
            foreach (var item in ItemsSource)
            {
                _itemsSource.Add((Medicines)item);
            }
            NotifyDataSetChanged();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
            var view = InflateViewForHolder(parent, viewType, itemBindingContext);

            return new MedicinesViewHolder(view, itemBindingContext, VM);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var medicinesHolder = (MedicinesViewHolder)holder;
            var item = (Medicines)GetItem(position);
            medicinesHolder.Refresh(item);
        }

        public override object GetItem(int viewPosition)
        {
            return _itemsSource?.ElementAt(viewPosition) ?? base.GetItem(viewPosition);
        }

        public override int ItemCount => _itemsSource?.Count ?? base.ItemCount;

    }
}
