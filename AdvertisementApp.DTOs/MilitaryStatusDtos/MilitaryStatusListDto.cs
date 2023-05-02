using AdvertisementApp.DTOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisementApp.DTOs.MilitaryStatusDtos
{
    public class MilitaryStatusListDto:IDto
    {
        public int Id { get; set; }
        public string Definition { get; set; }
    }
}
