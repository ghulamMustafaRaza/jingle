﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.ViewModel
{
    public class DashboardViewModel
    {
        public List<Itemdata> items { get; set; }

    }

    public class Itemdata
    {
        public string label { get; set; }
        public int value { get; set; }
        public int previous_rank { get; set; }
    }
}
