using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderManagement.EntityFramework.Identity;
using static OrderManagement.Domain.Constants;

namespace OrderManagement.EntityFramework
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly OrderManagementDBContext _OrderManagementDBContext;
        public AppUserRepository(OrderManagementDBContext OrderManagementDBContext)
        {
            _OrderManagementDBContext = OrderManagementDBContext;
        }

        public async Task<List<AppUser>> GetList()
        {
            var query = from user in _OrderManagementDBContext.AppUsers
                        join userRole in _OrderManagementDBContext.UserRoles on user.Id equals userRole.UserId
                        join role in _OrderManagementDBContext.Roles on userRole.RoleId equals role.Id
                        select new AppUser
                        {
                            Id = user.Id,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            Role = role.Name,
                            Surname = user.Surname
                        };


        return await  query.ToListAsync();
        }

        public async Task<bool> UserExists(string email)
        {
            return await _OrderManagementDBContext.AppUsers.AnyAsync(x => x.Id == email.ToLower());
        }

       
    }
}
