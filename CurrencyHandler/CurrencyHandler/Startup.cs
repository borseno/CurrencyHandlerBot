using CurrencyHandler.Models;
using CurrencyHandler.Models.Commands;
using CurrencyHandler.Models.Commands.Abstractions;
using CurrencyHandler.Models.Database.Contexts;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers;
using CurrencyHandler.Models.InlineKeyboardHandlers.Abstractions;
using CurrencyHandler.Models.QueryHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyHandler
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
            services.AddMvc();

            var connectionString = Configuration.GetConnectionString("SettingsDBSqlite");

            services.AddDbContext<ChatSettingsContext>(
                options => options.UseSqlite(connectionString));

            services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();
            services.AddScoped<ICurrenciesEmojisRepository, CurrenciesEmojisRepository>();

            services.AddScoped<IValueCurrencyKeyboardHandler, ValueCurrencyKeyboardHandler>();
            services.AddScoped<IDisplayCurrenciesKeyboardHandler, DisplayCurrenciesKeyboardHandler>();

            services.AddScoped<IKeyboards, Keyboards>();

            services.AddScoped<ICalcCommand, CalcCommand>();
            services.AddScoped<IDisplayCurrenciesCommand, DisplayCurrenciesCommand>();
            services.AddScoped<IInfoCommand, InfoCommand>();
            services.AddScoped<IStartCommand, StartCommand>();
            services.AddScoped<IValueCurrencyCommand, ValueCurrencyCommand>();
            services.AddScoped<ISettingsCommand, SettingsCommand>();
            services.AddScoped<IPercentsCommand, PercentsCommand>();

            services.AddScoped<ICommands, Commands>();

            services.AddScoped<IInlineQueryHandler, InlineQueryHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            // обработка ошибок HTTP
            app.UseStatusCodePages();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Bot Configurations
            Bot.Init();
        }
    }
}
