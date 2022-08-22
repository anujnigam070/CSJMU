﻿using CoreLayout.Models;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.State
{
    public class StateRepository : BaseRepository, IStateRepository
    {
        public StateRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(StateModel entity)
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_State";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CountryId", entity.CountryId, DbType.Int32);
                    parameters.Add("StateName", entity.StateName, DbType.String);
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

        public async Task<int> DeleteAsync(StateModel entity)
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_State";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("StateId", entity.StateId, DbType.Int32);
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

        public async Task<List<StateModel>> GetAllAsync()
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_State";       
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<StateModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<StateModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<StateModel> GetByIdAsync(int StateId)
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_State";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("StateId", StateId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<StateModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(StateModel entity)
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_State";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("StateId", entity.StateId, DbType.Int32);
                    parameters.Add("CountryId", entity.CountryId, DbType.Int32);
                    parameters.Add("StateName", entity.StateName, DbType.String);
                    parameters.Add("Status", entity.Status, DbType.String);
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
    }
}
