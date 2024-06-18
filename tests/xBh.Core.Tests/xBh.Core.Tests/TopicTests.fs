module xBh.Core.Tests.TopicTests

open NUnit.Framework
open xBh.Core

[<Test>]
let CreateTopicFromString () =
    let t1 = Topic.ofString "cpp.std.meta"
    let t2 = Topic.CompositeTopic ("cpp", [CompositeTopic ("std", [CompositeTopic ("meta", [])])])
    Assert.AreEqual(t1, t2)

[<Test>]
let MergeTopics () =
    let t1 = Topic.merge (Topic.ofString "cpp.std.meta") (Topic.ofString "cpp.dev.ides")
    let t2 = Topic.CompositeTopic ("cpp", [
        CompositeTopic("std", [CompositeTopic ("meta", [])])
        CompositeTopic("dev", [CompositeTopic ("ides", [])])
    ])
    Assert.AreEqual(t1, t2)
    let t3 = Topic.merge t2 (Topic.ofString "cpp.dev.ides.idea")
    let t4 = Topic.CompositeTopic ("cpp", [
        CompositeTopic("std", [CompositeTopic ("meta", [])])
        CompositeTopic("dev", [CompositeTopic ("ides", [CompositeTopic ("idea", [])])])
    ])
    Assert.AreEqual(t3, t4)
    let t5 = Topic.merge t3 (Topic.ofString "java.frameworks")
    let t6 = Topic.CompositeTopic ("root", [
        CompositeTopic ("cpp", [
            CompositeTopic("std", [CompositeTopic ("meta", [])])
            CompositeTopic("dev", [CompositeTopic ("ides", [CompositeTopic ("idea", [])])])
        ])
        CompositeTopic ("java", [
            CompositeTopic ("frameworks", [])
        ])
    ])
    Assert.AreEqual(t5, t6)

[<Test>]
let FoldTopic () =
    let t1 = Topic.CompositeTopic ("root", [
        CompositeTopic ("cpp", [
            CompositeTopic("std", [CompositeTopic ("meta", [])])
            CompositeTopic("dev", [CompositeTopic ("ides", [CompositeTopic ("idea", [])])])
        ])
        CompositeTopic ("java", [
            CompositeTopic ("frameworks", [])
        ])
    ])

    let folder (l: string list) (t: Topic) = l @ [(Topic.getName t)]
    let r: string list = Topic.fold folder [] t1
    Assert.AreEqual(r, ["root"; "cpp"; "std"; "meta"; "dev"; "ides"; "idea"; "java"; "frameworks"])
