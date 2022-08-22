using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.ButtonPermission
{
    public interface IButtonPermissionRepository : IRepository<ButtonPermissionModel>
    {
        Task<List<RegistrationModel>> GetAllUserAsync(int roleid);
    }
}
