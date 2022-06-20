using System;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCrossApp.iOS.Views;
using UIKit;

namespace MvvmCrossApp.iOS.Sources
{
	public class MedicinesTableViewSource: MvxTableViewSource
	{
		public MedicinesTableViewSource(UITableView tableView): base(tableView)
		{
			DeselectAutomatically = true;
		}

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
			var cell = tableView.DequeueReusableCell("medicineCell", indexPath) as MedicineCell;

			return cell;
        }
    }
}

