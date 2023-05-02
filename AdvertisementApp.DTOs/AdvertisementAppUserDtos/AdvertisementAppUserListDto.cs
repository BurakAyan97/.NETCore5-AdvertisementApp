using AdvertisementApp.DTOs.AdvertisementAppUserStatusDtos;
using AdvertisementApp.DTOs.AdvertisementDtos;
using AdvertisementApp.DTOs.AppUserDtos;
using AdvertisementApp.DTOs.MilitaryStatusDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisementApp.DTOs.AdvertisementAppUserDtos
{
    public class AdvertisementAppUserListDto
    {
        public int Id { get; set; }
        public DateTime? EndDate { get; set; }
        public int WorkExperience { get; set; }
        public string CvPath { get; set; }
        public int AdvertisementId { get; set; }
        public AdvertisementListDto Advertisement { get; set; }
        public int AppUserId { get; set; }
        public AppUserListDto AppUser { get; set; }
        public int AdvertisementAppUserStatusId { get; set; }
        public AdvertisementAppUserStatusListDto AdvertisementAppUserStatus { get; set; }
        public int MilitaryStatusId { get; set; }
        public MilitaryStatusListDto MilitaryStatus { get; set; }
    }
}
