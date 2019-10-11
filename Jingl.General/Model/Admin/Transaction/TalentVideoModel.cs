using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class TalentVideoModel
    {
        public int Id { get; set; }
        public int? TalentId { get; set; }
        public int? UserId { get; set; }
        public string CustomerName { get; set; }
        public int? FileId { get; set; }
        public string TalentNm { get; set; }
        public string Link { get; set; }
        public string ProfImg { get; set; }
        public string Thumbnails { get; set; }
        public string ProjectNm { get; set; }
        public int FileCategory { get; set; }
        public int Rate { get; set; }
        public int ViewsCount { get; set; }
        public bool? IsActive { get; set; }

        public string VideoNm { get; set; }
        public int Sequence { get; set; }
        public int? BookCategory { get; set; }
        public string CategoryName { get; set; }

    }
}
