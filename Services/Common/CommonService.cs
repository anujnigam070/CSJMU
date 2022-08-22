using CoreLayout.Models;
using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Common
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _commonRepository;

        public CommonService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }
      
        public async Task<List<DashboardModel>> GetDashboardByRoleAndUser(int roleid, int userid)
        {
            return await _commonRepository.GetByRoleAndUserAsync(roleid, userid);
        }
        public async Task<List<ButtonPermissionModel>> GetButtonByRoleAndUser(int roleid, int userid)
        {
            return await _commonRepository.GetButtonByRoleAndUserAsync(roleid, userid);
        }
    }
}
