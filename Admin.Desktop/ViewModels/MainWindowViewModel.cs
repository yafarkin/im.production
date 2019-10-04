using Admin.Desktop.Common;
using Epam.ImitationGames.Production.Common.Production;
using SantaHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Desktop.ViewModels
{
    class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private IEnumerable<Material> material;

        public IEnumerable<Material> Materials
        {
            get => material;
            set
            {
                material = value;
                OnPropertyChanged("Materials");
            }
        }

        public MainWindowViewModel()
        {
            var fileName = @"c:\tmp\materials.json";
            Materials = FileHelper.Load<List<Material>>(fileName);
        }
    }
}
