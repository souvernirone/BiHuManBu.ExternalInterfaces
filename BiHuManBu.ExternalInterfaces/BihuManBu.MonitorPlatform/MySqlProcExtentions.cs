using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BihuManBu.MonitorPlatform
{
    public static class MySqlProcExtentions
    {
        public static IList<TEntity> ExecuteStoredProcedureList<TEntity>(this DbContext context, string commandText, params object[] parameters) where TEntity : class
        {
            //if (parameters != null && parameters.Length > 0)
            //{
            //    for (int i = 0; i <= parameters.Length - 1; i++)
            //    {
            //        var p = parameters[i] as DbParameter;
            //        if (p == null)
            //            throw new Exception("Not support parameter type");

            //        commandText += i == 0 ? " " : ", ";

            //        commandText += "@" + p.ParameterName;
            //        if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
            //        {

            //            commandText += " output";
            //        }
            //    }
            //}

            var result = context.Database.SqlQuery<TEntity>(commandText, parameters).ToList();


            bool acd = context.Configuration.AutoDetectChangesEnabled;

            try
            {
                context.Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                    result[i] = context.Set<TEntity>().Attach(result[i]);
            }
            finally
            {
                context.Configuration.AutoDetectChangesEnabled = acd;
            }

            return result;
        }
    }
}