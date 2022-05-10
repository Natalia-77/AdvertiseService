using AdvertisePudlish.Models;

namespace AdvertisePudlish.Helper
{
    public  class PriceComparer : IComparer<AdvertiseViewModel>
    {
       

        public int Compare(AdvertiseViewModel x, AdvertiseViewModel y)
        {
            if (x.Price < y.Price)
            {
                return 1;
            }
            else if (x.Price > y.Price)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
