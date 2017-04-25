﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EducationManager.Models.DataModel
{
    public class Address
    {
        [Key]
        public int AddresId { get; set; }
        public string AddresValue { get; set; }
    }
}
