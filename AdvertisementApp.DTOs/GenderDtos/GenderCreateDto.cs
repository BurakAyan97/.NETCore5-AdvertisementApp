using AdvertisementApp.DTOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisementApp.DTOs.GenderDtos
{
    public class GenderCreateDto:IDto
    {
        public string Definition { get; set; }
    }
}
