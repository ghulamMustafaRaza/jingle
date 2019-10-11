﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction.API.FasPay
{
   public class TransactionPostResponse
    {
        public string response { get; set; }
        public string trx_id { get; set; }
        public string merchant_id { get; set; }
        public string merchant { get; set; }
        public string bill_no { get; set; }
        public string redirect_url { get; set; }
        public string response_code { get; set; }
        public string response_desc { get; set; }
        public List<item> bill_items { get; set; }
    }
}
