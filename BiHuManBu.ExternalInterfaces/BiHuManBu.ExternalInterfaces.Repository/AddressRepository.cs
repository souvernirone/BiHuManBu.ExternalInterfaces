using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;


namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        /// 20170228修改bx_address表userid为代理人id.
        /// 无需刷数据，该模块就预约单在用，目前预约单几乎未使用
        

        public int Add(bx_address bxAddress)
        {
            int addressId = 0;
            try
            {
                var t = DataContextFactory.GetDataContext().bx_address.Add(bxAddress);
                DataContextFactory.GetDataContext().SaveChanges();
                addressId = t.id;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                addressId = 0;
            }
            return addressId;
        }

        public bx_address Find(int addressId, int userid)
        {
            bx_address bxAddress=new bx_address();
            try
            {
                bxAddress = DataContextFactory.GetDataContext().bx_address.FirstOrDefault(x => x.id == addressId && x.userid == userid);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);

            }
            return bxAddress;
        }

        public int Delete(int addressId, int userid)
        {
            int count = 0;
            try
            {
                var address =
                    DataContextFactory.GetDataContext().bx_address.FirstOrDefault(x => x.id == addressId && x.userid == userid);
                if (address != null)
                {
                    address.Status = 0;

                    DataContextFactory.GetDataContext().bx_address.AddOrUpdate(address);
                    count = DataContextFactory.GetDataContext().SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }

        public int Update(bx_address bxAddress)
        {
            int count = 0;
            try
            {
                var address =
                    DataContextFactory.GetDataContext().bx_address.FirstOrDefault(x => x.id == bxAddress.id && x.userid == bxAddress.userid);
                if (address != null)
                {
                    address.Name = bxAddress.Name;
                    address.address = bxAddress.address;
                    address.agentId = bxAddress.agentId;
                    address.areaId = bxAddress.areaId;
                    address.cityId = bxAddress.cityId;
                    address.phone = bxAddress.phone;
                    address.provinceId = bxAddress.provinceId;
                    address.updatetime = bxAddress.updatetime;
                    DataContextFactory.GetDataContext().bx_address.AddOrUpdate(address);
                    count = DataContextFactory.GetDataContext().SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }

        public List<bx_address> FindByBuidAndAgentId(int userId)//, int agentId
        {
            List<bx_address> bxAddresses=new List<bx_address>();
            try
            {
                bxAddresses =
                    DataContextFactory.GetDataContext().bx_address.Where(x => x.userid == userId && x.Status == 1).ToList();//&& x.agentId == agentId
            }
            catch (Exception ex )
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                
            }
            return bxAddresses;
        }
    }
}
