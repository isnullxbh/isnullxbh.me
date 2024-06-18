namespace xBh.Core.Services

open System
open System.IO
open xBh.Core

type PostService() =
    inherit IPostService()

    do printf "PostService has been created\n"

    let mutable Posts : Post list = []
    let mutable Tags : Set<string> = Set.empty
    let mutable Topics : Topic = CompositeTopic ("root", [])

    override this.Load (path: string) =
        let mutable options = EnumerationOptions()
        options.RecurseSubdirectories <- true

        let postsPaths =
            Directory.GetFiles(path, "*.md", options)
            |> Array.toList

        let posts =
            postsPaths
            |> List.map File.ReadAllText
            |> List.map Post.ofMarkdownString

        Posts <- posts
        printf $"{Posts.Length} posts loaded\n"

        let tags =
            Posts
            |> List.map (_.GetTags())
            // |> List.map Set.toList
            |> List.concat
            |> Set.ofList

        Tags <- tags
        printf $"{Tags.Count} tags retrieved\n"

        let topics =
            Posts
            |> List.fold (fun t p -> Topic.merge t (Topic.ofString p.Topic)) Topics

        Topics <- topics

    override this.GetPosts() = Posts
    override this.GetTags() = Tags
    override this.GetTopics() = Topics
