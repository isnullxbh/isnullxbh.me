namespace xBh.Core.Services

open System
open xBh.Core

[<AbstractClass>]
type IPostService() =
    abstract member GetPosts: unit -> List<Post>
    abstract member GetTags: unit -> Set<string>
    abstract member GetTopics: unit -> Topic
    member this.GetPostsByTopic (t: Topic) : List<Post> =
        this.GetPosts() |> List.filter (fun p -> p.Topic = t)

    member this.GetPostsPerTopic () : PostsPerTopic list =
        let folder (s: PostsPerTopic list) (t: Topic) (path: string) =
            let path' =
                match path with
                | "" -> (t |> Topic.getName)
                | _  -> path + "." + (t |> Topic.getName)

            let level =
                path'
                |> Seq.filter (fun c -> c = '.')
                |> Seq.length

            printf $"Path: {String.Join('.', path')}\n"
            // Subtract 1 because "root" does not participate in menu generation
            s @ [{ Topic = t; Posts = this.GetPostsByTopic(path' |> Topic.ofString); Level = level - 1 }]

        Topic.fold2 folder [] (this.GetTopics()) ""
