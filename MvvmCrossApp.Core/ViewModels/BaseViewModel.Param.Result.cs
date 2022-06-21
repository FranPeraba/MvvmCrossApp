using System;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MvvmCrossApp.Core.ViewModels
{
	public abstract class BaseViewModel<TParameter, TResult> : BaseViewModel, IMvxViewModel<TParameter, TResult>
		where TParameter : class
		where TResult : class
	{

		protected BaseViewModel()
		{
		}

		public abstract TaskCompletionSource<object> CloseCompletionSource { get; set; }

		public abstract void Prepare(TParameter parameter);
    }
}

