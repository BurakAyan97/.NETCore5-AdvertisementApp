﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisementApp.Entities
{
    public class AppRole:BaseEntity
    {
        public string Definition { get; set; }

        //Nav Props
        public List<AppUserRole> AppUserRoles { get; set; }
    }
}
