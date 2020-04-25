using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Domain.User
{
    public class TokenValue
    {
        [Key]
        public long Id { get; set; }
        public string ValueToken { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime DateLogin { get; set; }
        public bool IsActive { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
