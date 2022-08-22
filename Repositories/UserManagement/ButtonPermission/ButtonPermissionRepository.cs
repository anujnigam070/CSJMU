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

namespace CoreLayout.Repositories.UserManagement.ButtonPermission
{
    public class ButtonPermissionRepository : BaseRepository, IButtonPermissionRepository
    {
        public ButtonPermissionRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(ButtonPermissionModel entity)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                  
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("FormName", entity.FormName, DbType.String);
                    parameters.Add("AddButton", entity.AddButton, DbType.String);
                    parameters.Add("EditButton", entity.EditButton, DbType.String);
                    parameters.Add("DetailButton", entity.DetailButton, DbType.String);
                    parameters.Add("DeleteButton", entity.DeleteButton, DbType.String);
                    parameters.Add("DownloadButton", entity.DownloadButton, DbType.String);
                    parameters.Add("SearchButton", entity.SearchButton, DbType.String);
                    parameters.Add("UploadButton", entity.UploadButton, DbType.String);
                    parameters.Add("ExtraButton", entity.ExtraButton, DbType.String);
                    parameters.Add("UserId", entity.UserId, DbType.Int32);
                    parameters.Add("RoleId", entity.RoleId, DbType.Int32);
                    parameters.Add("Status", entity.Status, DbType.String);
                    parameters.Add("CreatedBy", entity.CreatedBy, DbType.String);
                    parameters.Add("@Query", 1, DbType.Int32);
                    var res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> DeleteAsync(ButtonPermissionModel entity)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", entity.Id, DbType.Int32);
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

        public async Task<List<ButtonPermissionModel>> GetAllAsync()
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<ButtonPermissionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<ButtonPermissionModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<ButtonPermissionModel> GetByIdAsync(int Id)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", Id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<ButtonPermissionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(ButtonPermissionModel entity)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", entity.Id, DbType.String);
                    parameters.Add("FormName", entity.FormName, DbType.String);
                    parameters.Add("AddButton", entity.AddButton, DbType.String);
                    parameters.Add("EditButton", entity.EditButton, DbType.String);
                    parameters.Add("DetailButton", entity.DetailButton, DbType.String);
                    parameters.Add("DeleteButton", entity.DeleteButton, DbType.String);
                    parameters.Add("DownloadButton", entity.DownloadButton, DbType.String);
                    parameters.Add("SearchButton", entity.SearchButton, DbType.String);
                    parameters.Add("UploadButton", entity.UploadButton, DbType.String);
                    parameters.Add("ExtraButton", entity.ExtraButton, DbType.String);
                    parameters.Add("UserId", entity.UserId, DbType.Int32);
                    parameters.Add("RoleId", entity.RoleId, DbType.Int32);
                    parameters.Add("Status", entity.Status, DbType.Int32);
                    parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.String);
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
        public async Task<List<RegistrationModel>> GetAllUserAsync(int roleid)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleId", roleid, DbType.String);
                    parameters.Add("@Query", 6, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<RegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<RegistrationModel>)list;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

