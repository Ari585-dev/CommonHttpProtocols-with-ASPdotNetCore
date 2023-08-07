namespace WebApplication1.Services
{
    public class FileWritter : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string fileName = "File.txt";
        private Timer timer;

        public FileWritter(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Write("Process Started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            Write("The Process is Ending Now");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Write("Executing Process: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        }

        private void Write(string mssg)
        {
            var route = $@"{env.ContentRootPath}\wwwroot\{this.fileName}";
            using(StreamWriter writer= new StreamWriter(route, append: true))
            {
                writer.WriteLine(mssg);
            }
        }
    }
}
