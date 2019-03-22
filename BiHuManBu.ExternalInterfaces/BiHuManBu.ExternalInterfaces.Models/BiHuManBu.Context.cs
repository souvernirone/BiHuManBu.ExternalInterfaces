﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BiHuManBu.ExternalInterfaces.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EntityContext : DbContext
    {
        public EntityContext()
            : base("name=EntityContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<bx_agent_special_rate> bx_agent_special_rate { get; set; }
        public DbSet<bx_lastinfo> bx_lastinfo { get; set; }
        public DbSet<bx_agent_rate> bx_agent_rate { get; set; }
        public DbSet<bx_lastinfo_userinput> bx_lastinfo_userinput { get; set; }
        public DbSet<user_token> user_token { get; set; }
        public DbSet<bx_renewalquote> bx_renewalquote { get; set; }
        public DbSet<bx_gsc_userinfo> bx_gsc_userinfo { get; set; }
        public DbSet<bx_charge> bx_charge { get; set; }
        public DbSet<bx_charge_history> bx_charge_history { get; set; }
        public DbSet<bx_city> bx_city { get; set; }
        public DbSet<bx_images> bx_images { get; set; }
        public DbSet<bx_area> bx_area { get; set; }
        public DbSet<bx_address> bx_address { get; set; }
        public DbSet<bx_bj_union> bx_bj_union { get; set; }
        public DbSet<bx_hebaodianwei> bx_hebaodianwei { get; set; }
        public DbSet<bx_devicedetail> bx_devicedetail { get; set; }
        public DbSet<bx_order> bx_order { get; set; }
        public DbSet<bx_order_userinfo> bx_order_userinfo { get; set; }
        public DbSet<bx_sms_account> bx_sms_account { get; set; }
        public DbSet<bx_carinfo> bx_carinfo { get; set; }
        public DbSet<user> user { get; set; }
        public DbSet<bx_sms_account_content> bx_sms_account_content { get; set; }
        public DbSet<bx_sms_order> bx_sms_order { get; set; }
        public DbSet<bx_agentpoint> bx_agentpoint { get; set; }
        public DbSet<bx_system_message> bx_system_message { get; set; }
        public DbSet<bx_order_quoteresult> bx_order_quoteresult { get; set; }
        public DbSet<bx_order_savequote> bx_order_savequote { get; set; }
        public DbSet<bx_transferrecord> bx_transferrecord { get; set; }
        public DbSet<bx_notice_xb> bx_notice_xb { get; set; }
        public DbSet<bx_history_contract> bx_history_contract { get; set; }
        public DbSet<bx_report_claim> bx_report_claim { get; set; }
        public DbSet<bx_consumer_review> bx_consumer_review { get; set; }
        public DbSet<bx_userinfo_renewal_info> bx_userinfo_renewal_info { get; set; }
        public DbSet<bx_agent_distributed> bx_agent_distributed { get; set; }
        public DbSet<bx_para_bhtype> bx_para_bhtype { get; set; }
        public DbSet<bx_appsetting> bx_appsetting { get; set; }
        public DbSet<bx_buid_activity> bx_buid_activity { get; set; }
        public DbSet<bx_preferential_activity> bx_preferential_activity { get; set; }
        public DbSet<bx_userinfo_renewal_index> bx_userinfo_renewal_index { get; set; }
        public DbSet<bx_gsc_renewal> bx_gsc_renewal { get; set; }
        public DbSet<bx_order_claimdetail> bx_order_claimdetail { get; set; }
        public DbSet<bx_cityquoteday> bx_cityquoteday { get; set; }
        public DbSet<bx_car_renewal> bx_car_renewal { get; set; }
        public DbSet<bx_savequote> bx_savequote { get; set; }
        public DbSet<bx_vehicleinfo> bx_vehicleinfo { get; set; }
        public DbSet<bx_violationlog> bx_violationlog { get; set; }
        public DbSet<bx_config> bx_config { get; set; }
        public DbSet<bx_quoteresult_carinfo> bx_quoteresult_carinfo { get; set; }
        public DbSet<bx_drivelicense_cartype> bx_drivelicense_cartype { get; set; }
        public DbSet<bx_car_claims> bx_car_claims { get; set; }
        public DbSet<bx_carmodel> bx_carmodel { get; set; }
        public DbSet<bx_claim_detail> bx_claim_detail { get; set; }
        public DbSet<bx_message> bx_message { get; set; }
        public DbSet<bj_baodanxinxi> bj_baodanxinxi { get; set; }
        public DbSet<bx_agent_select> bx_agent_select { get; set; }
        public DbSet<bx_picture> bx_picture { get; set; }
        public DbSet<bx_order_submit_info> bx_order_submit_info { get; set; }
        public DbSet<bx_quote_many_source> bx_quote_many_source { get; set; }
        public DbSet<bx_anxin_delivery> bx_anxin_delivery { get; set; }
        public DbSet<bx_userinfo> bx_userinfo { get; set; }
        public DbSet<bx_submit_info> bx_submit_info { get; set; }
        public DbSet<bx_specialoption> bx_specialoption { get; set; }
        public DbSet<bx_agent_config> bx_agent_config { get; set; }
        public DbSet<bx_agent_xgaccount_relationship> bx_agent_xgaccount_relationship { get; set; }
        public DbSet<bx_batchrenewal_item> bx_batchrenewal_item { get; set; }
        public DbSet<bx_camera_config> bx_camera_config { get; set; }
        public DbSet<bx_crm_steps> bx_crm_steps { get; set; }
        public DbSet<bx_msgindex> bx_msgindex { get; set; }
        public DbSet<bx_quoteresult> bx_quoteresult { get; set; }
        public DbSet<bx_userinfo_expand> bx_userinfo_expand { get; set; }
        public DbSet<manager_role_db> manager_role_db { get; set; }
        public DbSet<bx_distributed_history> bx_distributed_history { get; set; }
        public DbSet<bx_camera_blacklist> bx_camera_blacklist { get; set; }
        public DbSet<bx_agent_ukey> bx_agent_ukey { get; set; }
        public DbSet<bx_car_record> bx_car_record { get; set; }
        public DbSet<bx_car_renewal_premium> bx_car_renewal_premium { get; set; }
        public DbSet<bx_renewalstatus> bx_renewalstatus { get; set; }
        public DbSet<bx_agent> bx_agent { get; set; }
        public DbSet<bx_quotehistory_related> bx_quotehistory_related { get; set; }
        public DbSet<bx_car_order> bx_car_order { get; set; }
        public DbSet<bx_ywxdetail> bx_ywxdetail { get; set; }
        public DbSet<bx_quotereq_carinfo> bx_quotereq_carinfo { get; set; }
        public DbSet<bj_baodanxianzhong> bj_baodanxianzhong { get; set; }
        public DbSet<bx_customercategories> bx_customercategories { get; set; }
    }
}
