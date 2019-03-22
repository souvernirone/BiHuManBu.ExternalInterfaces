using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class UserCalimMapper
    {
        public static UserClaimViewModel ConverToViewModel(this bx_claim_detail detail)
        {
            UserClaimViewModel vm = new UserClaimViewModel();

            if (detail.endcase_time.HasValue)
            {
                vm.EndcaseTime = detail.endcase_time.Value.ToString("yyyy-MM-dd");
            }
            if (detail.loss_time.HasValue)
            {
                vm.LossTime = detail.loss_time.Value.ToString("yyyy-MM-dd");
            }
            vm.PayAmount = detail.pay_amount ?? 0;
            vm.PayCompanyName = detail.pay_company_name;
            return vm;
        }

        public static List<UserClaimViewModel> ConvertToViewModelList(this List<bx_claim_detail> details)
        {
            List<UserClaimViewModel> viewModels = new List<UserClaimViewModel>();
            foreach (bx_claim_detail detail in details)
            {
                UserClaimViewModel vm = detail.ConverToViewModel();
                viewModels.Add(vm);
            }

            return viewModels;

        }
        public static UserClaimDetailViewModel ConverToDetailViewModel(this bx_claim_detail detail)
        {
            UserClaimDetailViewModel vm = new UserClaimDetailViewModel();

            if (detail.endcase_time.HasValue)
            {
                vm.EndcaseTime = detail.endcase_time.Value.ToString("yyyy-MM-dd");
            }
            if (detail.loss_time.HasValue)
            {
                vm.LossTime = detail.loss_time.Value.ToString("yyyy-MM-dd");
            }
            vm.PayAmount = detail.pay_amount ?? 0;
            vm.PayCompanyName = detail.pay_company_name;
            vm.PayType = detail.pay_type ?? -1;
            return vm;
        }

        public static List<UserClaimDetailViewModel> ConvertToDetailViewModelList(this List<bx_claim_detail> details)
        {
            List<UserClaimDetailViewModel> viewModels = new List<UserClaimDetailViewModel>();
            foreach (bx_claim_detail detail in details)
            {
                UserClaimDetailViewModel vm = detail.ConverToDetailViewModel();
                viewModels.Add(vm);
            }

            return viewModels;

        }

        public static List<ClaimDetailViewModel> ConvertToNewList(this List<bx_claim_detail> details)
        {
            var list = new List<ClaimDetailViewModel>();
            if (!details.Any()) return list;
            ClaimDetailViewModel detail;
            foreach (var i in details)
            {
                detail = new ClaimDetailViewModel
                {
                    Buid = i.b_uid,
                    CreateTime = i.create_time,
                    StrCreateTime = i.create_time.HasValue ? i.create_time.Value.ToString("yyyy-MM-dd") : "",
                    EndCaseTime = i.endcase_time,
                    StrEndCaseTime = i.endcase_time.HasValue ? i.endcase_time.Value.ToString("yyyy-MM-dd") : "",
                    Id = i.id,
                    Liid = i.li_id,
                    LossTime = i.loss_time,
                    StrLossTime = i.loss_time.HasValue ? i.loss_time.Value.ToString("yyyy-MM-dd") : "",
                    PayAmount = i.pay_amount,
                    PayCompanyName = i.pay_company_name,
                    PayCompanyNo = i.pay_company_no
                };
                list.Add(detail);
            }
            return list;
        }

        public static List<UserClaimDetailViewModel> ConvertToNewDetailList(this List<bx_claim_detail> details)
        {
            var list = new List<UserClaimDetailViewModel>();
            if (!details.Any()) return list;
            UserClaimDetailViewModel detail;
            foreach (var i in details)
            {
                detail = new UserClaimDetailViewModel
                {
                    EndcaseTime = i.endcase_time.HasValue ? i.endcase_time.Value.ToString("yyyy-MM-dd") : "",
                    LossTime = i.loss_time.HasValue ? i.loss_time.Value.ToString("yyyy-MM-dd") : "",
                    PayAmount = i.pay_amount.HasValue ? i.pay_amount.Value : 0,
                    PayCompanyName = i.pay_company_name ?? "",
                    PayType = i.pay_type ?? -1
                };
                list.Add(detail);
            }
            return list;
        }
    }
}