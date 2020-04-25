using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApi.Domain.User
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
