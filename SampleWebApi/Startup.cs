
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Splunk.Configurations;
using Splunk;

namespace splunkWebApi
{
    public class Startup
    {
        public class MyOptions
        {
            public bool EnableSwagger = false;

            // snip...
        }
        private MyOptions myOpts { get; }
        public IConfigurationRoot Configuration1 { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                 .WriteTo.EventCollector("http://splunk:8088/services/collector", "08559b54-8b15-4adc-8980-cf57d90039b6").CreateLogger();
            myOpts = new MyOptions();
            var appSettings = Configuration.GetSection("AppSettings");
            myOpts.EnableSwagger = false;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SplunkLoggerConfiguration>(Configuration.GetSection("Splunk"));
            services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TEST API", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            /******************************** Define Your Logger *********************************/
            /*                                                                                   */
            //                    First get configuration to be used at Logger                   //
            var splunkLoggerConfiguration = GetSplunkLoggerConfiguration(app);                   //
            //                                                                                   //
            //                       Choose one of those loggers                                 //
            //                                                                                   //                                                                                  
            loggerFactory.AddHECRawSplunkLogger(splunkLoggerConfiguration);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               // app.UseSwagger();
             //   app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TEST API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        SplunkLoggerConfiguration GetSplunkLoggerConfiguration(IApplicationBuilder app)
        {
            SplunkLoggerConfiguration result = null;

            //Retrieving Splunk configuration from appsettings json configuration file
            var splunkLoggerConfigurationOption = app.ApplicationServices.GetService<IOptions<SplunkLoggerConfiguration>>();
            if (splunkLoggerConfigurationOption != null && splunkLoggerConfigurationOption.Value != null)
                result = app.ApplicationServices.GetService<IOptions<SplunkLoggerConfiguration>>().Value;

            //You can also provide a hard code configuration
            //result = new SplunkLoggerConfiguration()
            //{
            //    HecConfiguration = new HECConfiguration()
            //    {
            //        SplunkCollectorUrl = "https://localhost:8088/services/collector",
            //        BatchIntervalInMilliseconds = 5000,
            //        BatchSizeCount = 100,
            //        ChannelIdType = HECConfiguration.ChannelIdOption.None,

            //        Token = "753c5a9c-fb59-4da0-9064-947f99dc20ba"
            //    },
            //    SocketConfiguration = new SocketConfiguration()
            //    {
            //        HostName = "localhost",
            //        Port = 8111
            //    }
            //};
            return result;
        }
    }
}
