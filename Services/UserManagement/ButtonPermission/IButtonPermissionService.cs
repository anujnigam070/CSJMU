using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.ButtonPermission
{
    public interface IButtonPermissionService
    {
        public Task<List<ButtonPermissionModel>> GetAllButtonPermissionAsync();
        public Task<ButtonPermissionModel> GetButtonPermissionByIdAsync(int id);
        public Task<int> CreateButtonPermissionAsync(ButtonPermissionModel buttonPermissionModel);
        public Task<int> UpdateButtonPermissionAsync(ButtonPermissionModel buttonPermissionModel);
        public Task<int> DeleteButtonPermissionAsync(ButtonPermissionModel buttonPermissionModel);
        public Task<List<RegistrationModel>> GetAllUsersAsync(int roleid);
    }
}
