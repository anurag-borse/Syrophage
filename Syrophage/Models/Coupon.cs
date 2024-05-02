﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Syrophage.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }

        [Required]
        public double Discount { get; set; }

        [Required]
        public double MinimumAmount { get; set; }

        [Required]
        public string? CouponDescription { get; set; }

        [Required]
        [MaxLength(17)]
        public string Code { get; set; }


        [Required]
        public DateTime ExpiryDate { get; set; }


        public DateTime ? CreatedDate { get; set; }

        [NotMapped]

        [Required]
        public IFormFile? CouponPicture { get; set; }

        public string? CouponPictureUrl { get; set; }


        public bool? IsActivated { get; set; }



        public ICollection<UserCoupon>? UserCoupons { get; set; }





    }
}
