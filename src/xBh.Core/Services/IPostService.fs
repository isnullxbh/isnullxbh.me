namespace xBh.Core.Services

open System
open xBh.Core

[<AbstractClass>]
type IPostService() =
    abstract member GetPosts: unit -> List<Post>
    abstract member GetTags: unit -> Set<string>
    abstract member GetTopics: unit -> Topic
    member this.GetPostsByTopic (t: string) : List<Post> =
        this.GetPosts() |> List.filter (fun p -> p.Topic = t)

    abstract member Load: string -> unit

    member this.GetPostsPerTopic () : PostsPerTopic list =
        let folder (s: PostsPerTopic list) (t: Topic) (path: string) =
            let path' =
                match path with
                | "" -> t |> Topic.getName
                | _  -> path + "." + (t |> Topic.getName)

            let level =
                path'
                |> Seq.filter (fun c -> c = '.')
                |> Seq.length

            let unrootedPath =
                if path'.StartsWith("root.")
                    then path'.Substring(5)
                    else path'

            // Subtract 1 because "root" does not participate in menu generation
            s @ [{ Topic = t; Posts = this.GetPostsByTopic(unrootedPath); Level = level - 1 }]

        Topic.fold2 folder [] (this.GetTopics()) ""
