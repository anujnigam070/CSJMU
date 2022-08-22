using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.Registration
{
    public class RegistrationRepository : BaseRepository, IRegistrationRepository
    {
        public RegistrationRepository(IConfiguration configuration)
   : base(configuration)
        { }
        public async Task<int> CreateAsync(RegistrationModel registrationModel)
        {
            try
            {
                var query = "SP_CRUD_USERLOGIN";
                using (var connection = CreateConnection())
                {
                    //var UserId = connection.ExecuteScalar("SELECT Max(UserId)+1 AS ID FROM UserLogin");
                    //if (UserId == null)
                    //{
                    //    UserId = 1;
                    //}
                    //StringBuilder stringBuilder = new StringBuilder();
                    //foreach (int roleid in registrationModel.roleid)
                    //{
                    //    stringBuilder.Append("insert into UserRole(RoleId,RoleUserId,IsRoleActive,IsMainRole,IPAddress,UserId,ModTs,CreateTs) values('" + roleid + "','" + UserId + "','1', '1',':11','"+ registrationModel.CreatedBy + "',GETDATE(),GETDATE())");
                    //    stringBuilder.Append("; ");

                    //}
                    registrationModel.IsRoleActive = 1;
                    registrationModel.IsUserActive = 1;
                    registrationModel.IsMainRole = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UserName", registrationModel.UserName, DbType.String);
                    parameters.Add("LoginID", registrationModel.LoginID, DbType.String);
                    parameters.Add("MobileNo", registrationModel.MobileNo, DbType.String);
                    parameters.Add("EmailID", registrationModel.EmailID, DbType.String);
                    parameters.Add("Salt", registrationModel.Salt, DbType.String);
                    parameters.Add("SaltedHash", registrationModel.SaltedHash, DbType.String);
                    parameters.Add("IsUserActive", registrationModel.IsUserActive, DbType.String);
                    parameters.Add("RefType", registrationModel.RefType, DbType.String);
                    parameters.Add("IPAddress", ":11");
                    parameters.Add("@Query", 1, DbType.Int32);
                    var res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    //InsertChildRecord(registrationModel);
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //public void InsertChildRecord(RegistrationModel registrationModel)
        //{
        //    try
        //    {
        //        var res = 0;
        //        var query = "SP_CRUD_USERLOGIN";
        //        using (var connection = CreateConnection())
        //        {
        //            var UserId = connection.ExecuteScalar("SELECT Max(UserId)+1 AS ID FROM UserLogin");
        //            if (UserId == null)
        //            {
        //                UserId = 1;
        //            }
        //            DynamicParameters parameters = new DynamicParameters();
        //            //parameters.Add("roleid", registrationModel.roleid, DbType.Int32);
        //            parameters.Add("UserRoleId", UserId, DbType.Int32);
        //            parameters.Add("RoleUserId", 111, DbType.Int32);
        //            parameters.Add("IsRoleActive", registrationModel.IsRoleActive, DbType.Int32);
        //            parameters.Add("IsMainRole", registrationModel.IsMainRole, DbType.Int32);
        //            parameters.Add("IPAddress", ":11");
        //            parameters.Add("UserId", registrationModel.CreatedBy, DbType.String);
        //            parameters.Add("@Query", 7, DbType.Int32);

        //            if (registrationModel.RoleName == "Administrator")
        //            {
        //                StringBuilder stringBuilder = new StringBuilder();
        //                foreach (int roleid in registrationModel.roleid)
        //                {
        //                    parameters.Add("RoleId", roleid);
        //                    res = SqlMapper.Execute(connection, query, parameters, commandType: CommandType.StoredProcedure);
        //                }
        //            }
        //            else
        //            {
        //                parameters.Add("RoleId", 16);
        //                res = SqlMapper.Execute(connection, query, parameters, commandType: CommandType.StoredProcedure);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        public async Task<int> DeleteAsync(RegistrationModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Role";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("IsRecordDeleted", 1);
                    parameters.Add("UserID", entity.UserID, DbType.Int32);
                    parameters.Add("@Query", 3, DbType.Int32);
                    var res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<RegistrationModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_CRUD_USERLOGIN";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<RegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<RegistrationModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<RegistrationModel> GetByIdAsync(int id)
        {
            try
            {
                var query = "SP_CRUD_USERLOGIN";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UserID", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<RegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(RegistrationModel registrationModel)
        {
            try
            {
                var query = "SP_CRUD_USERLOGIN";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UserID", registrationModel.UserID, DbType.Int32);
                    parameters.Add("UserName", registrationModel.UserName, DbType.String);
                    parameters.Add("LoginID", registrationModel.LoginID, DbType.String);
                    parameters.Add("MobileNo", registrationModel.MobileNo, DbType.String);
                    parameters.Add("EmailID", registrationModel.EmailID, DbType.String);
                    parameters.Add("IsUserActive", 1);
                    parameters.Add("RefType", registrationModel.RefType, DbType.String);
                    parameters.Add("IPAddress", registrationModel.IPAddress, DbType.String);
                    parameters.Add("@Query", 2, DbType.Int32);
                    var res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
