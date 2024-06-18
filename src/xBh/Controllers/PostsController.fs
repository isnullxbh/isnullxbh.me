namespace xBh.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open xBh.Core.Services

type PostsController(logger: ILogger<PostsController>, postService: IPostService) =
    inherit Controller()
        do printf "PostController created\n"

    member this.Index () =
        let postsGroupedByTopics = postService.GetPostsPerTopic()
        printf $"Post groups: {postsGroupedByTopics.Length}\n"
        this.View(postsGroupedByTopics)
