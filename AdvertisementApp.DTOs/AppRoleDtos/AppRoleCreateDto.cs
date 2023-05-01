using AdvertisementApp.DTOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisementApp.DTOs.AppRoleDtos
{
    public class AppRoleCreateDto:IDto
    {
        public string Definition { get; set; }
    }
}
