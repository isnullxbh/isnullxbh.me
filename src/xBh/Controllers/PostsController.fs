namespace xBh.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open xBh.Core.Services

type PostsController(logger: ILogger<PostsController>, postService: IPostService) =
    inherit Controller()

    member this.Index () =
        let postsGroupedByTopics = postService.GetPostsPerTopic()
        this.View(postsGroupedByTopics)
