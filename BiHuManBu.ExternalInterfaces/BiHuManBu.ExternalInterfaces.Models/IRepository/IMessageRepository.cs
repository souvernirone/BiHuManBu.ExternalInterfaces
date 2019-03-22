
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IMessageRepository
    {
        #region bx_message
        int Add(bx_message bxMessage);
        int Update(bx_message bxMessage);
        bx_message Find(int msgId);
        bx_message FindById(int msgId);
        List<bx_message> FindMsgList(int agentId, int pageSize, int curPage, out int total);
        List<bx_message> FindNoReadList(int agentId, out int total);
        List<TableMessage> FindNoReadAllList(int agentId, out int total);
        #endregion

        #region bx_system_message
        int AddSysMessage(bx_system_message bxSystemMessage);
        int UpdateSysMessage(bx_system_message bxSystemMessage);
        List<bx_system_message> FindSysMessage();

        #endregion
    }
}
