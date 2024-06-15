namespace xBh.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open xBh.Core.Services

type PostsController(logger: ILogger<PostsController>, postService: IPostService) =
    inherit Controller()

    member this.Index () = this.View(postService.GetPostsPerTopic())
