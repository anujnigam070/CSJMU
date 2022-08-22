using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories.UserManagement.ButtonPermission;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.ButtonPermission
{
    public class ButtonPermissionService : IButtonPermissionService
    {
        private readonly IButtonPermissionRepository _buttonPermissionRepository;

        public ButtonPermissionService(IButtonPermissionRepository buttonPermissionRepository)
        {
            _buttonPermissionRepository = buttonPermissionRepository;
        }

        public async Task<List<ButtonPermissionModel>> GetAllButtonPermissionAsync()
        {
            return await _buttonPermissionRepository.GetAllAsync();
        }

        public async Task<ButtonPermissionModel> GetButtonPermissionByIdAsync(int id)
        {
            return await _buttonPermissionRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateButtonPermissionAsync(ButtonPermissionModel buttonPermissionModel)
        {
            return await _buttonPermissionRepository.CreateAsync(buttonPermissionModel);
        }

        public async Task<int> UpdateButtonPermissionAsync(ButtonPermissionModel buttonPermissionModel)
        {
            return await _buttonPermissionRepository.UpdateAsync(buttonPermissionModel);
        }

        public async Task<int> DeleteButtonPermissionAsync(ButtonPermissionModel buttonPermissionModel)
        {
            return await _buttonPermissionRepository.DeleteAsync(buttonPermissionModel);
        }
        public async Task<List<RegistrationModel>> GetAllUsersAsync(int roleid)
        {
            return await _buttonPermissionRepository.GetAllUserAsync(roleid);
        }
    }
}
