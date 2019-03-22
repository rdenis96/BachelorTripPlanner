using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Helpers
{
    public static class UserInterestsHelper
    {
        public static string ConvertCountriesEnumListToString(this List<CountriesEnum> countriesEnums)
        {
            var countries = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendJoin(',', countriesEnums);
            countries = stringBuilder.ToString();
            return countries;
        }

        public static List<CountriesEnum> ConvertCountriesStringToCountriesEnumList(this string countries)
        {
            var countriesSplit = countries.Split(',');
            List<CountriesEnum> countriesEnums = new List<CountriesEnum>();
            foreach (var country in countriesSplit)
            {
                CountriesEnum countryEnum = CountriesEnum.None;
                try
                {
                    Enum.TryParse(country, out countryEnum);
                }
                catch (Exception ex)
                {
                    countryEnum = CountriesEnum.None;
                }
                if (Convert.ToInt32(countryEnum) != 0)
                {
                    countriesEnums.Add(countryEnum);
                }
            }
            return countriesEnums;
        }
    }
}