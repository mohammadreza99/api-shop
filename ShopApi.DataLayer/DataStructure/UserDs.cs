using System;

namespace ShopApi.DataLayer.DataStructure
{
    public class UserDs
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public int RoleId { get; set; }
    }
}
