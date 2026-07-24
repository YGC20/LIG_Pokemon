using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using Pokemon.Models;

namespace Pokemon.Core
{
    public class TypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not PokemonType type)
            {
                return new SolidColorBrush(Colors.Gray);
            }

            return type switch
            {
                PokemonType.Fire => new SolidColorBrush(Colors.Red),
                PokemonType.Water => new SolidColorBrush(Colors.Blue),
                PokemonType.Grass => new SolidColorBrush(Colors.Green),
                PokemonType.Electric => new SolidColorBrush(Colors.Gold),
                PokemonType.Ground => new SolidColorBrush(Colors.SaddleBrown),
                PokemonType.Flying => new SolidColorBrush(Colors.SkyBlue),
                PokemonType.Ice => new SolidColorBrush(Colors.PaleTurquoise),
                PokemonType.Fighting => new SolidColorBrush(Colors.Firebrick),
                PokemonType.Normal => new SolidColorBrush(Colors.Tan),
                _ => new SolidColorBrush(Colors.Gray)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
