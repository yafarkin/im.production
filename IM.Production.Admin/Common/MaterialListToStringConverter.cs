using Epam.ImitationGames.Production.Common.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Admin.Desktop.Common
{
    [ValueConversion(typeof(List<string>), typeof(string))]
    public class MaterialListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var materials = (IEnumerable<Material>)value;
            if(materials != null && materials.Count() != 0)
            {
                var result = string.Join(",", materials.Select(x => x.DisplayName));
                return result;
            }
            else
            {
                return "Господом данное";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
