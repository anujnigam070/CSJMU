using CoreLayout.Models;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Country
{
    public class CountryRepository : BaseRepository, ICountryRepository
    {
        public CountryRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(CountryModel countryModel)
        {
            try
            {
                var query = "InsertCountry";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CountryName", countryModel.CountryName, DbType.String);
                    parameters.Add("CountryStatus", countryModel.CountryStatus, DbType.String);
                    parameters.Add("CreatedBy", countryModel.CreatedBy, DbType.String);
                    //parameters.Add("CreatedDate", countryModel.CreatedDate, DbType.DateTime);
                    var res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> DeleteAsync(CountryModel entity)
        {
            try
            {
                var query = "DeleteCountry_ByID";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CountryId", entity.CountryId, DbType.Int32);
                    var res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<CountryModel>> GetAllAsync()
        {
            try
            {
                var query = "GellAllCountry";
                using (var connection = CreateConnection())
                {
                    var list = await SqlMapper.QueryAsync<CountryModel>(connection, query, commandType: CommandType.StoredProcedure);

                    return (List<CountryModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CountryModel> GetByIdAsync(int CountryId)
        {
            try
            {
                var query = "GellAllCountry_ByID";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CountryId", CountryId, DbType.Int32);

                    var lst =  await SqlMapper.QueryAsync<CountryModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(CountryModel entity)
        {
            try
            {
                var query = "UpdateCountry_ByID";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CountryId", entity.CountryId, DbType.Int32);
                    parameters.Add("CountryName", entity.CountryName, DbType.String);
                    parameters.Add("CountryStatus", entity.CountryStatus, DbType.String);
                    parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.String);
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
