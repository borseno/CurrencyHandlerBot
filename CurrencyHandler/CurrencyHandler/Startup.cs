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

            services.AddScoped<IInlineKeyboardHandler, ValueCurrencyKeyboardHandler>();
            services.AddScoped<IInlineKeyboardHandler, DisplayCurrenciesKeyboardHandler>();

            services.AddScoped<IKeyboards, Keyboards>();

            services.AddScoped<ICommand, CalcCommand>();
            services.AddScoped<ICommand, DisplayCurrenciesCommand>();
            services.AddScoped<ICommand, InfoCommand>();
            services.AddScoped<ICommand, StartCommand>();
            services.AddScoped<ICommand, ValueCurrencyCommand>();
            services.AddScoped<ICommand, SettingsCommand>();
            services.AddScoped<ICommand, PercentsCommand>();
            services.AddScoped<ICommand, PercentsWithSynonymsCommand>();

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
