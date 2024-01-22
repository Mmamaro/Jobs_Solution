using AssessmentAPI.DataContext;
using AssessmentAPI.Models;
using Dapper;
using System.Collections;
using System.Data;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace AssessmentAPI.Repositories
{
    public class JobRepository
    {
        private readonly DapperContext _Context;

        public JobRepository(DapperContext context)
        {
            _Context = context;
        }

        public async Task<IEnumerable<Job>> GetJobsAsync()
        {
            const string sql = "SELECT * FROM jobs";

            using IDbConnection connection = _Context.CreateConnection();

            var jobs = await connection.QueryAsync<Job>(sql);

            return jobs;
        }

        public async Task<Job> GetJobByIdAsync(Guid Id)
        {
            var parameters = new { JobId = Id };

            const string sql = "SELECT * from jobs where Id = @JobId";

            IDbConnection connection = _Context.CreateConnection();

            var result = await connection.QueryFirstOrDefaultAsync<Job>(sql, parameters);

            return result;
        }



        public async Task CreateJobAsync(Job jobObj)
        {

            var parameters = new DynamicParameters();
            parameters.Add("Name", jobObj.Name, DbType.String);
            parameters.Add("Description", jobObj.Description, DbType.String);
            parameters.Add("LastRunTime", jobObj.LastRunTime);
            parameters.Add("TimeIntervals", jobObj.TimeIntervals);
            parameters.Add("Status", jobObj.Status);
            parameters.Add("City", jobObj.City);

            const string sql = "INSERT INTO jobs(Name, Description, LastRunTime, TimeIntervals, Status, City) " +
                "VALUES(@Name, @Description, @LastRunTime, @TimeIntervals, @Status, @City)";

            IDbConnection connection = _Context.CreateConnection();

            await connection.ExecuteAsync(sql, parameters);
        }

        public async Task UpdateJobAsync(Job jobObj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", jobObj.Id);
            parameters.Add("Name", jobObj.Name, DbType.String);
            parameters.Add("Description", jobObj.Description, DbType.String);
            parameters.Add("LastRunTime", jobObj.LastRunTime);
            parameters.Add("TimeIntervals", jobObj.TimeIntervals);
            parameters.Add("Status", jobObj.Status);
            parameters.Add("City", jobObj.City);

            const string sql = "UPDATE jobs SET Name = @Name, Description = @Description, LastRunTime = @LastRunTime, " +
                "TimeIntervals = @TimeIntervals, Status = @Status, City = @City WHERE Id = @Id";

            using IDbConnection connection = _Context.CreateConnection();

            await connection.ExecuteAsync(sql, parameters); 
        }

        public async Task DeleteJobAsync(Guid id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            const string sql = "DELETE FROM jobs WHERE Id = @Id";

            using IDbConnection connection = _Context.CreateConnection();

            await connection.ExecuteAsync(sql, parameters);
        } 

        public async Task<List<Job>> SearchStatusAsync(string? status, string? City)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Status", status);
            parameters.Add("City", City);

            using IDbConnection connection = _Context.CreateConnection();

            string sql = "SELECT * FROM jobs";

            if(status != null)
            {
                sql = sql + " WHERE Status = @Status";
            }

            if(City != null)
            {
                if(status != null)
                {
                    sql = sql + " and City = @City";
                }
                else
                {
                    sql = sql + " where City = @City";
                }
            }

            var jobs = await connection.QueryAsync<Job>(sql, parameters);

            return jobs.ToList();
        }

        public async void UpdateLastRunTime(Guid jobId, DateTime lastRunTime)
        {
            //Parameters
            var parameters = new DynamicParameters();
            parameters.Add("Id", jobId);
            parameters.Add("LastRunTime", lastRunTime);

            //Query
            const string sql = "update jobs set LastRunTime = @LastRunTime where Id = @Id";

            //Accessing Db Connection
            using IDbConnection connection = _Context.CreateConnection();

            //Executing Query
            await connection.ExecuteAsync(sql, parameters);

        }

    }
}
