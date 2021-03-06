﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Results;

namespace Web.ViewModels
{
    public class PurchaseViewModel : BaseViewModel
    {
        public override string username { get; set; }

        public override IList<string> errors { get; set; }

        public override string message { get; set; }

        public string description { get; set; }

        public float totalAmount { get; set; }

        public override string groupName { get; set; }

        public string groups { get; set; }

    }
}