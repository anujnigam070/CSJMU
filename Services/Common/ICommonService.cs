using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Common
{
    public interface ICommonService
    {
        public Task<List<DashboardModel>> GetDashboardByRoleAndUser(int roleid, int userid);

        public Task<List<ButtonPermissionModel>> GetButtonByRoleAndUser(int roleid, int userid);

    }
}
