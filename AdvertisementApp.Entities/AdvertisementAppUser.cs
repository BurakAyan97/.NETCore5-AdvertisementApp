using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisementApp.Entities
{
    public class AdvertisementAppUser : BaseEntity
    {
        public DateTime? EndDate { get; set; }
        public int WorkExperience { get; set; }
        public string CvPath { get; set; }

        //Nav Props
        public int AdvertisementId { get; set; }
        public Advertisement Advertisement { get; set; }
        
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        
        public int AdvertisementAppUserStatusId { get; set; }
        public AdvertisementAppUserStatus AdvertisementAppUserStatus { get; set; }
        
        public int MilitaryStatusId { get; set; }
        public MilitaryStatus MilitaryStatus { get; set; }
        

    }
}
