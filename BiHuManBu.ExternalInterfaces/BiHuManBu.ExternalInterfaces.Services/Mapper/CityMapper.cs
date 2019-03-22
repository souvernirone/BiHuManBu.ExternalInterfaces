using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static  class CityMapper
    {
         public static CityViewModel ConvertViewModel(this List<bx_city> cities)
         {
             CityViewModel view = new CityViewModel();
             view.Cities = new List<City> {};
             if (cities != null)
             {
                 foreach (bx_city city in cities)
                 {
                     City c = ConvertToCity(city);
                     view.Cities.Add(c);
                 }
             }

             return view;

         }

         public static City ConvertToCity(this bx_city city)
         {
             return new City
             {
                 CityId = city.id,
                 CityName = city.city_name
             };
         }
    }
}
