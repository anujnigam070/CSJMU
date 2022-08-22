using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Common
{
    public interface ICommonRepository
    {
        Task<List<DashboardModel>> GetByRoleAndUserAsync(int roleid, int userid);

        Task<List<ButtonPermissionModel>> GetButtonByRoleAndUserAsync(int roleid, int userid);
    }
}
