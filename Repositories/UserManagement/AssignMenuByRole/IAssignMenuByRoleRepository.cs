﻿using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.AssignMenuByRole
{
    public interface IAssignMenuByRoleRepository : IRepository<AssignMenuByRoleModel>
    {
        Task<List<AssignMenuByRoleModel>> CheckAlreadyAsync(int menuid,int roleid);
    }
}
