using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class ImagesRepository : IImagesRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public int Add(List<bx_images> images)
        {
            try
            {
                DataContextFactory.GetDataContext().Database.ExecuteSqlCommand("UPDATE bx_images SET TYPE=-1 WHERE buid=" + images[0].buid + "");
                foreach (var item in images)
                {
                    DataContextFactory.GetDataContext().bx_images.Add(item);
                }
                DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                return -1;
            }
            return 0;

        }

        //类型,1=验车照片,2=在京使用证明,-1=已删除
        public List<bx_images> FindByBuid(long buid)
        {
            try
            {
                return DataContextFactory.GetDataContext().bx_images.Where(x => x.buid == buid && x.type>=0).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                
            }
            return null;
        }
    }
}
