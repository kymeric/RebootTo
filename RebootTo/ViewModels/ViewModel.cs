using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace RebootTo.ViewModels
{
    public abstract class ViewModel : PropertyChangedBase
    {
    }

    public abstract class ViewModel<TModel> : ViewModel
    {
        protected ViewModel(TModel model)
        {
            Model = model;
        }

        protected TModel Model { get; set; }
    }
}
