// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MvvmCrossApp.iOS.Views
{
	[Register ("MedicineCell")]
	partial class MedicineCell
	{
		[Outlet]
		UIKit.UILabel MedicineLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (MedicineLabel != null) {
				MedicineLabel.Dispose ();
				MedicineLabel = null;
			}

		}
	}
}
