using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Abilities.Aspects;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public abstract partial class AspectModelBase : ObservableObject 
    {
        public string Name => AspectsDictionary.Collection[GetType()].Name;

        public abstract string Description { get; }
    }
}
