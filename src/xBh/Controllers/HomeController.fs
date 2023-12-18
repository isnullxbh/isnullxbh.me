namespace xBh.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Diagnostics

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

open xBh.Models

type HomeController (logger : ILogger<HomeController>) =
    inherit Controller()

    member this.Index () =
        this.View()

    member this.Posts () = this.RedirectToAction("Index", "Posts")

    member this.Talks () = this.View()

    member this.Projects () = this.View()

    member this.Contact () = this.View()

    member this.About () = this.View()

    member this.CV() = this.View()

    member this.Privacy () =
        this.View()

    [<ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)>]
    member this.Error () =
        let reqId = 
            if isNull Activity.Current then
                this.HttpContext.TraceIdentifier
            else
                Activity.Current.Id

        this.View({ RequestId = reqId })