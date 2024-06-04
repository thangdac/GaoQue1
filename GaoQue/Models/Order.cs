using GaoQue.DataAccess;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaoQue.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }

        [Required(ErrorMessage = "Địa chỉ giao hàng là bắt buộc.")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Ghi chú là bắt buộc.")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
        public string PaymentMethod { get; set; }

        [ForeignKey("UserId")]

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
