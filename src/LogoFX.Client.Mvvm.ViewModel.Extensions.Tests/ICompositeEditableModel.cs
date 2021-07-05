using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Tests
{
    interface ICompositeEditableModel : IEditableModel
    {
        IEnumerable<int> Phones { get; }

        ISimpleEditableModel Person { get; set; }

        IEnumerable<ISimpleEditableModel> SimpleCollection { get; }
    }
}