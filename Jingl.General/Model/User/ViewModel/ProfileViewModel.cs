using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.User.Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.User.ViewModel
{
    public class ProfileViewModel
    {
        public UserModel UserModel { get; set; }
        public TalentModel TalentModel { get; set; }
        public List<BookModel> TalentBookList { get; set; }
        public IList<TalentVideoModel> TalentVideoList { get; set; }
        public IList<TalentVideoModel> UserVideoList { get; set; }
        public IList<TalentVideoModel> AllVideoList { get; set; }
        public IList<DeviceModel> DeviceModelList { get; set; }
        public string AppVersion { get; set; }
        public string Period { get; set; }
        public List<TalentCategoryViewModel> ListTalentWishlist { get; set; }
        public List<BookModel> ListBooking { get; set; }
        public string ActBookCount { get; set; }
        public string CompBookCount { get; set; }
        public List<BookModel> BookPaidList { get; set; }
        public SaldoModel TalentSaldo { get; set; }
        public decimal? LimitSaldo { get; set; }


    }
}
