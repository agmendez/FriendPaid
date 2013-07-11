﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Results;

namespace friendpaid_web.ViewModels
{
    public abstract class BaseViewModel
    {

        public abstract string username { get; set; }

        public abstract IList<ValidationFailure> errors { get; set; }

    }
}