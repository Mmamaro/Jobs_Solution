using AssessmentAPI.Models;
using AssessmentAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace AssessmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly JobRepository _repo;

        public JobsController(JobRepository repo)
        {
            this._repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetAllJobs()
        {
            IEnumerable jobs = await _repo.GetJobsAsync();

            return Ok(jobs);
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<Job>> GetJobById(Guid Id)
        {
            Job? job = await _repo.GetJobByIdAsync(Id);



            if(job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }

        [HttpPost]
        public async Task<ActionResult<Job>> CreateJob(Job jobObj)
        {
            if(jobObj == null || jobObj.Name == "string" || jobObj.Description == "string" 
                || jobObj.Status == "string" || jobObj.TimeIntervals <= 0 || jobObj.City == "string")
            {
                return BadRequest();
            }

             await _repo.CreateJobAsync(jobObj);

            return StatusCode(201);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<Job>> UpdateJob(Guid Id,[FromBody] Job jobObj)
        {
            if(jobObj == null || jobObj.Name == "string" || jobObj.Description == "string" || jobObj.Status == "string" || jobObj.City == "string")
            {
                return BadRequest();
            }

            await _repo.UpdateJobAsync(jobObj);

            return StatusCode(201);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Job>> DeleteJob(Guid Id)
        {
            if(Id == null)
            {
                return BadRequest();
            }

            await _repo.DeleteJobAsync(Id);

            return StatusCode(200);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Job>>> SearchStatus(string? status, string? City)
        {

            var jobs = await _repo.SearchStatusAsync(status, City);

            return Ok(jobs);
        }
    }
}
