using Sample.Background.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Sample.Background
{
    public class SampleService
    {
        private readonly IJobFactory _jobFactory;

        public SampleService(IJobFactory jobFactory)
        {
            _jobFactory = jobFactory;
        }

        public void OnStart()
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;
            scheduler.JobFactory = _jobFactory;
            scheduler.Start().Wait();

            var jobDetails = JobBuilder.Create<SampleJob>()
                .WithIdentity(JobKey.Create("Name of the service", "Group of the service"))
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(new TriggerKey("Name of the service", "Group of the service"))
                .StartNow()
                .WithSimpleSchedule(builder =>
                {
                    builder.WithIntervalInSeconds(120).RepeatForever();
                })
                .Build();

            scheduler.ScheduleJob(jobDetails, trigger).Wait();
        }

        public void OnStop()
        {
           
        }
    }
}
