using BRIX.Mobile.View.IconFonts;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class AspectPanelViewModel : ViewModelBase
    {
        public AspectPanelViewModel()
        {
            AspectsCollection = new()
            {
                new AspectPanelPageVM { Icon = AwesomeRPG.AlienFire },
                new AspectPanelPageVM { Icon = AwesomeRPG.FlamingTrident },
                new AspectPanelPageVM { Icon = AwesomeRPG.Repair },
                new AspectPanelPageVM { Icon = AwesomeRPG.Rabbit },
                new AspectPanelPageVM { Icon = AwesomeRPG.RoundShield },
                new AspectPanelPageVM { Icon = AwesomeRPG.ShotgunShell },
                new AspectPanelPageVM { Icon = AwesomeRPG.BoltShield },
            };
        }

        [ObservableProperty]
        private ObservableCollection<AspectPanelPageVM> _aspectsCollection = new();

        [ObservableProperty]
        private AspectPanelPageVM _selectedAspect = new();
    }

    public class AspectPanelPageVM
    {
        public string Icon { get; set; }
        public string AspectPagePath { get; set; }
    }
}
