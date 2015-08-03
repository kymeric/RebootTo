using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace RebootTo.ViewModels
{
    public abstract class ScreenViewModel : Screen
    {
    }

    public abstract class ScreenViewModel<TModel> : ScreenViewModel
    {
        protected ScreenViewModel(TModel model)
        {
            Model = model;
        }

        protected TModel Model { get; set; }
    }
}