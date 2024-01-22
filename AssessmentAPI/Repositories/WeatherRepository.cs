using AssessmentAPI.DataContext;
using AssessmentAPI.Models;
using Dapper;
using MySqlConnector;
using System.Data;

namespace AssessmentAPI.Repositories
{
    public class WeatherRepository
    {
        private readonly DapperContext _Context;

        public WeatherRepository(DapperContext context)
        {
            _Context = context;
        }

        public async Task<IEnumerable<Weather>> GetAllWeatherAsync()
        {
            const string sql = "SELECT * FROM weather";

            using IDbConnection connection = _Context.CreateConnection();

            var weather = await connection.QueryAsync<Weather>(sql);

            return weather;
        }

        public async Task AddWeatherAsync(string Temperature, string WindSpeed, string Description, Guid JobId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("Temperature", Temperature);
            parameters.Add("WindSpeed", WindSpeed);
            parameters.Add("Description", Description);
            parameters.Add("JobId", JobId);

            const string sql = "INSERT INTO weather(Temperature, WindSpeed, Description,JobId) " +
                "VALUES(@Temperature, @WindSpeed, @Description, @JobId)";

            IDbConnection connection = _Context.CreateConnection();

            await connection.ExecuteAsync(sql, parameters);
        }

        public async Task UpdateWeatherAsync(string Temperature, string WindSpeed, string Description, Guid JobId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("Temperature", Temperature);
            parameters.Add("WindSpeed", WindSpeed);
            parameters.Add("Description", Description);
            parameters.Add("JobId", JobId);

            const string sql = @"Update weather set Temperature = @Temperature, WindSpeed = @WindSpeed, Description = @Description
                                   where JobId = @JobId";

            IDbConnection connection = _Context.CreateConnection();

            await connection.ExecuteAsync(sql, parameters);
        }

    }
}
