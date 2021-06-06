namespace xBh.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

type PostsController(logger: ILogger<PostsController>) =
    inherit Controller()

    member this.Index () = this.View()
