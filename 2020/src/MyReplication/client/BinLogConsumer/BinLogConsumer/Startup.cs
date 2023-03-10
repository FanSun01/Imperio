using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinLogConsumer.EventBus;
using BinLogConsumer.Events;
using BinLogConsumer.Handles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace BinLogConsumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>();
            services.AddSingleton<IEventBusSubscriptionManager, EventBusSubscriptionManager>(sp => new EventBusSubscriptionManager());
            services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp => new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" });
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddControllers().AddNewtonsoftJson();
            services.AddDistributedMemoryCache(options =>
            {
                options.ExpirationScanFrequency = TimeSpan.FromMinutes(5);
                options.SizeLimit = 10;
            });


            services.AddEventBus();
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<ObjectPool<IModel>>(serviceProvider =>
            {
                var objectPoolProvider = serviceProvider.GetRequiredService<ObjectPoolProvider>();
                var connectionFactory = serviceProvider.GetRequiredService<ConnectionFactory>();
                return objectPoolProvider.Create(new ChannelObjectPoolPolicy(connectionFactory));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
