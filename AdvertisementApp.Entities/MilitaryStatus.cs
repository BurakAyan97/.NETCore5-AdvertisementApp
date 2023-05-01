using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisementApp.Entities
{
    public class MilitaryStatus : BaseEntity
    {
        public string Definition { get; set; }

        //Nav Props
        public List<AdvertisementAppUser> AdvertisementAppUsers { get; set; }
    }
}
