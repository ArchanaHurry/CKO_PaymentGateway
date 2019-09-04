using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Entities
{
    public class MerchantUsername
    {
        [Key]
        public int Id { get; set; }
        public string Merchant { get; set; }
        public string Username { get; set; }
    }
}
