using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShopApi.DataLayer.DataStructure;
using ShopApi.Domain.User;
using System;
using System.Linq;
using ShopApi.DataLayer.Data;

namespace ShopApi.DataLayer.Method.Token
{
    public class Token
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DatabaseContext _context;

        public Token(DatabaseContext context)
        {
            _context = context;
            _httpContextAccessor = new HttpContextAccessor();
        }

        public CrudOperationDs CheckToken(CrudOperationDs crud)
        {
            var tokenHeader = _httpContextAccessor.HttpContext.Request.Headers["authorization"].FirstOrDefault();
            TokenValue token;
            try
            {
                token = _context.TokenValue.Include(p => p.User)
                    .FirstOrDefault(p => p.ValueToken == tokenHeader && p.IsActive);
            }
            catch (Exception)
            {
                crud.SetError("اتصال به سرور صورت نگرفته است");
                return crud;
            }

            if (token == null)
            {
                crud.TokenExist = false;
                crud.SetError("چنین کاربری ثبت نشده است یا اعتبار شما به پایان رسیده است دوباره وارد شوید");
                return crud;
            }
            if (token.ExpireDate < DateTime.Now)
            {
                crud.TokenExist = false;
                crud.SetError("زمان شما به پایان رسیده است. دوباره وارد شوید");
                return crud;
            }
            crud.UserInfo = new UserDs
            {
                Username = token.User.Username,
                Id = token.User.Id,
                Password = token.User.Password,
                EmailAddress = token.User.EmailAddress,
                RoleId = token.User.RoleId
            };
            return crud;
        }
    }
}

