namespace xBh

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy;
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Westwind.AspNetCore.Markdown
open xBh.Core.Services

type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =

        services.AddControllersWithViews().AddRazorRuntimeCompilation() |> ignore
        services.AddRazorPages() |> ignore

        services.AddMvc()
            .AddApplicationPart(typeof<MarkdownPageProcessorMiddleware>.Assembly) |> ignore

        services.AddMarkdown(fun configuration ->
            configuration.AddMarkdownProcessingFolder("/posts/") |> ignore
        ) |> ignore

        services.AddSingleton<IPostService>(PostService()) |> ignore
        // TODO: Can this be done the following way? Not sure.
        let serviceProvider = services.BuildServiceProvider()
        serviceProvider.GetService<IPostService>().Load(
            $"{serviceProvider.GetService<IWebHostEnvironment>().WebRootPath}/posts")

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =

        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore
        else
            app.UseExceptionHandler("/Home/Error") |> ignore
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts() |> ignore

        app.UseMarkdown() |> ignore
        app.UseStaticFiles() |> ignore
        app.UseRouting() |> ignore
        app.UseAuthorization() |> ignore

        app.UseEndpoints(fun endpoints ->
            endpoints.MapDefaultControllerRoute() |> ignore
            endpoints.MapRazorPages() |> ignore) |> ignore

    member val Configuration : IConfiguration = null with get, set
