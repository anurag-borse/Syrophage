using System.ComponentModel.DataAnnotations;

namespace Syrophage.Models
{
    public class UserCoupon
    {

        [Key]
        public int Id { get; set; }


        public int UserId { get; set; }
        public Users User { get; set; }

        public int CouponId { get; set; }
        public Coupon Coupon { get; set; }
    }

}
