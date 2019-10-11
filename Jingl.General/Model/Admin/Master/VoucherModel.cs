using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Master
{
    public class VoucherModel
    {
        public int Id { get; set; }

        public string VoucherCd { get; set; }

        public string VoucherNm { get; set; }

        public string VoucherDesc { get; set; }

        public int? RemainingCount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsUsed { get; set; }

        public bool? IsClaimed { get; set; }

        public string SentTo { get; set; }

        public decimal Amount { get; set; }

        public decimal Percentage { get; set; }
    }
}
