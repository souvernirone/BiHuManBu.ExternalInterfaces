using System;
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class DriverLicenseTypeMapper
    {
        public static DriverLicenseType ConverToVewModel(this bx_drivelicense_cartype item)
        {
            DriverLicenseType model = new DriverLicenseType()
            {
                TypeName = item.type_name ?? string.Empty,
                TypeValue = item.type_value ?? string.Empty
            };

            return model;
        }

        public static List<DriverLicenseType> ConvertToList(this List<bx_drivelicense_cartype> lists)
        {
            List<DriverLicenseType> models = new List<DriverLicenseType>();
            foreach (bx_drivelicense_cartype drivelicenseCartype in lists)
            {
                models.Add(drivelicenseCartype.ConverToVewModel());
            }

            return models;
        } 
    }
}
