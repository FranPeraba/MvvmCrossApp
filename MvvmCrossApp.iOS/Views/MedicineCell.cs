﻿// This file has been autogenerated from a class added in the UI designer.

using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MvvmCrossApp.iOS.Views
{
    public partial class MedicineCell : MvxTableViewCell
	{
		public MedicineCell(IntPtr handle) : base(handle)
		{
			this.DelayBind(() => { this.AddBindings(MedicineLabel, "Text Nombre"); });
		}
	}
}
