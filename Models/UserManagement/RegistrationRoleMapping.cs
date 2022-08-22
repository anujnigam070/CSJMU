﻿using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.UserManagement
{
    public class RegistrationRoleMapping: BaseEntity
    {
        [Key]

        public int UserRoleId { get; set; }
        public int RoleId { get; set; }
        public int RoleUserId { get; set; }
        public int IsRoleActive { get; set; }
        public int IsMainRole { get; set; }
        public int UserId { get; set; }

        public string IPAddress { get; set; }
        public List<int> RoleList { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }
    }
}
