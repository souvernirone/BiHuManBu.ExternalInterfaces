﻿using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class WorkOrderMapper
    {
        public static WorkOrderViewModel ConverToViewModel(this bx_userinfo_renewal_info bxWorkOrder)
        {
            WorkOrderViewModel model = new WorkOrderViewModel();
            if (bxWorkOrder != null)
            {
                model.Id = bxWorkOrder.id;
                model.Intention_View = bxWorkOrder.client_intention;
                model.Customer_Name = bxWorkOrder.client_name;
                model.Phone = bxWorkOrder.client_mobile;
                model.Sa_Agent_Id = bxWorkOrder.sa_id;
                model.SaAgentName = bxWorkOrder.sa_name;
                model.Status = bxWorkOrder.result_status;
                model.Create_Time = bxWorkOrder.create_time.ToString("yyyy-MM-dd HH:mm");
                model.Adv_Agent_Id = bxWorkOrder.xubao_id;
                model.AdvAgentName = bxWorkOrder.xubao_name;
                model.Buid = bxWorkOrder.b_uid.HasValue?bxWorkOrder.b_uid.Value:0;
                model.IntentionCompany = bxWorkOrder.intentioncompany;
                model.Remark = bxWorkOrder.remark;
            }
            return model;
        }

        public static List<WorkOrderDetail> ConverToViewModel(this List<bx_consumer_review> workOrderDetail)
        {
            List<WorkOrderDetail> list = new List<WorkOrderDetail>();
            WorkOrderDetail detail;
            if (workOrderDetail != null)
            {
                foreach (var item in workOrderDetail)
                {
                    detail = new WorkOrderDetail();
                    detail.Id = item.id;
                    detail.Result_Status = item.result_status;
                    detail.Recall_Time = item.next_review_date.HasValue ? item.next_review_date.Value.ToString("yyyy-MM-dd HH:mm") : null;
                    detail.Agent_Id = item.operatorId;
                    detail.AgentName = item.operatorName;
                    detail.Create_Time = item.create_time.HasValue ? item.create_time.Value.ToString("yyyy-MM-dd HH:mm") : null;
                    detail.IntentionCompany = item.intentioncompany;
                    detail.Remark = item.content;
                    list.Add(detail);
                }
            }
            return list;
        }
    }
}