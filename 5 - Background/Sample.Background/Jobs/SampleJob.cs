using Quartz;
using System.Threading.Tasks;

namespace Sample.Background.Jobs
{
    public class SampleJob : IJob
    {
        public SampleJob()
        {
        }

        public Task Execute(IJobExecutionContext context)
        {
            // Here you can call an application method to do some process, or anything you want

            return Task.CompletedTask;
        }
    }
}
