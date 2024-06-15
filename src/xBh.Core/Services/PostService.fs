namespace xBh.Core.Services

open System
open xBh.Core

type PostService() =
    inherit IPostService()
    override this.GetPosts() = [
        { Title = "Post1"; Topic = Topic.ofString "root.C++"; Path = "/posts/2024/post-1"; Tags = Set.ofList ["cpp"; "clang"]; CreatedAt = DateTime.Now }
        { Title = "Post1"; Topic = Topic.ofString "root.C++"; Path = "/posts/2024/post-1"; Tags = Set.ofList ["cpp"; "clang"]; CreatedAt = DateTime.Now }
        { Title = "Post1"; Topic = Topic.ofString "root.C++.Standard Library"; Path = "/posts/2024/post-1"; Tags = Set.ofList ["cpp"; "clang"]; CreatedAt = DateTime.Now }
        { Title = "Post1"; Topic = Topic.ofString "root.C++.Standard Library"; Path = "/posts/2024/post-1"; Tags = Set.ofList ["cpp"; "clang"]; CreatedAt = DateTime.Now }
        { Title = "Post1"; Topic = Topic.ofString "root.C++.Standard Library.Metaprogramming"; Path = "/posts/2024/post-1"; Tags = Set.ofList ["cpp"; "clang"]; CreatedAt = DateTime.Now }
        { Title = "Post1"; Topic = Topic.ofString "root.C++.Standard Library.Metaprogramming"; Path = "/posts/2024/post-1"; Tags = Set.ofList ["cpp"; "clang"]; CreatedAt = DateTime.Now }
        { Title = "Post11"; Topic = Topic.ofString "root.C++.Standard Library.Algorithms"; Path = "/posts/2024/post-1"; Tags = Set.ofList ["cpp"; "clang"]; CreatedAt = DateTime.Now }
        { Title = "Post11"; Topic = Topic.ofString "root.C++.Standard Library.Algorithms"; Path = "/posts/2024/post-1"; Tags = Set.ofList ["cpp"; "clang"]; CreatedAt = DateTime.Now }
        { Title = "Post2"; Topic = Topic.ofString "root.DevOps.GitLab"; Path = "/posts/2024/post-2"; Tags = Set.ofList ["gitlab"; "docker"; "devops"]; CreatedAt = DateTime.Now }
        { Title = "Post2"; Topic = Topic.ofString "root.DevOps.GitLab"; Path = "/posts/2024/post-2"; Tags = Set.ofList ["gitlab"; "docker"; "devops"]; CreatedAt = DateTime.Now }
        { Title = "Post21"; Topic = Topic.ofString "root.DevOps.GitHub"; Path = "/posts/2024/post-2"; Tags = Set.ofList ["gitlab"; "docker"; "devops"]; CreatedAt = DateTime.Now }
        { Title = "Post21"; Topic = Topic.ofString "root.DevOps.GitHub"; Path = "/posts/2024/post-2"; Tags = Set.ofList ["gitlab"; "docker"; "devops"]; CreatedAt = DateTime.Now }
    ]
    override this.GetTags() = Set.ofList ["c++"; "clang"; "llvm"]
    override this.GetTopics() =
        let folder (t: Topic) (p: Post) = Topic.merge t p.Topic
        List.fold folder (CompositeTopic ("root", [])) (this.GetPosts())
