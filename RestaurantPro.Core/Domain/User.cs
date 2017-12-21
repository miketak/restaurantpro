using System.Collections.Generic;

namespace RestaurantPro.Core.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        //Remove this now future Michael. You know this is evil and bad practice. Regards. Michael 12/17/17:21:21CDT
        public string Password { get; set; }

        #region Navigation Properties

        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        #endregion

    }
}