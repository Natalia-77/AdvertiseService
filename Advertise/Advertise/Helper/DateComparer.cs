using AdvertisePudlish.Models;

namespace AdvertisePudlish.Helper
{
    public class DateComparer : IComparer<AdvertiseViewModel>
    {
        public int Compare(AdvertiseViewModel x, AdvertiseViewModel y)
        {
            if (x.DateCreate < y.DateCreate)
            {
                return 1;
            }
            else if (x.DateCreate > y.DateCreate)
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
