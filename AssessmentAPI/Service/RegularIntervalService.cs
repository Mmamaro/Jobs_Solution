using AssessmentAPI.Models;
using AssessmentAPI.Repositories;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace AssessmentAPI.Service
{

    public class RegularIntervalService : IHostedService
    {
        bool Run = true;
        private readonly JobRepository? _jobRepository;
        private readonly WeatherRepository? _weatherRepository;
        private readonly WeatherDatafetcher _weatherDatafetcher;
        //private readonly CancellationTokenSource? _cancellationTokenSource;

        public RegularIntervalService(JobRepository jobRepository, WeatherRepository? weatherRepository, WeatherDatafetcher weatherDatafetcher)
        {
            _jobRepository = jobRepository;
            //_cancellationTokenSource = new CancellationTokenSource();
            _weatherRepository = weatherRepository;
            _weatherDatafetcher = weatherDatafetcher;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => RunTask(cancellationToken));

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(Timeout.Infinite, cancellationToken);
        }

        //The background task 
        private async Task RunTask(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                
                var jobs = await _jobRepository.GetJobsAsync();
                var weather = await _weatherRepository.GetAllWeatherAsync();

                foreach (var job in jobs)
                {
                    try
                    {
                        job.LastRunTime = DateTime.Now;

                        DateTime nextRunTime = DateTime.Now.AddSeconds(job.TimeIntervals);

                        if (nextRunTime > job.LastRunTime)
                        {
                            _jobRepository.UpdateLastRunTime(job.Id, job.LastRunTime);

                            //Getting the weather info and storing it in a dictionary
                            Dictionary<string, string> weatherData = await _weatherDatafetcher.FetchWeatherDataAsync(job.City);

                            //check if a job already exists in the weather table
                            var w = weather.Where(x => x.JobId == job.Id).FirstOrDefault();

                            //if the job does not exist then I insert it with its weather information if it does then I update
                            if (w != null)
                            {
                                //Updating the information
                                await _weatherRepository.UpdateWeatherAsync(weatherData["Temperature"], weatherData["WindSpeed"], weatherData["Description"], job.Id);
                            }
                            else
                            {
                                //Inserting Information
                                await _weatherRepository.AddWeatherAsync(weatherData["Temperature"], weatherData["WindSpeed"], weatherData["Description"], job.Id);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await Console.Out.WriteLineAsync($"Error processing job: {job?.Id}: {ex.StackTrace}");
                        continue;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(job.TimeIntervals), cancellationToken);
                }

                }

            }


        }
    }




