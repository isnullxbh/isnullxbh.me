namespace xBh.Core

open System
open Markdig
open Markdig.Extensions.Yaml
open YamlDotNet.Serialization
open Markdig.Syntax
open System.Linq

type Post() =
    [<YamlMember(Alias = "title")>]
    member val Title: string = "" with get, set

    [<YamlMember(Alias = "topic")>]
    member val Topic: string = "" with get, set

    [<YamlMember(Alias = "tags")>]
    member val TagsAsStr: string = "" with get, set

    [<YamlMember(Alias = "date")>]
    member val CreatedAt: string = "" with get, set

    // TODO
    [<YamlMember(Alias = "path")>]
    member val Path: string = "" with get, set

    member this.GetTags() : string list =
        this.TagsAsStr.Split([|','|])
        |> Array.toList

type PostsPerTopic =
    { Topic: Topic
      Posts: Post list
      Level: int }

module Post =
    [<AbstractClass; Sealed>]
    type MarkdownExtensions private () =
        static member YamlDeserializer: IDeserializer =
            DeserializerBuilder().IgnoreUnmatchedProperties().Build()

        static member Pipeline: MarkdownPipeline =
            MarkdownPipelineBuilder().UseYamlFrontMatter().Build()

    let ofMarkdownString (markdown: string) : Post =
        let document = Markdown.Parse(markdown, MarkdownExtensions.Pipeline)
        let block = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault()

        let metadataTxt =
            block
                .Lines
                .Lines
                .OrderByDescending(fun s -> s.Line)
                .Select(fun s -> $"{s}\n")
                .ToList()
                .Select(fun s -> s.Replace("---", ""))
                .Where(fun s -> String.IsNullOrWhiteSpace(s) = false)
                .Aggregate(fun s a -> a + s)

        let post = MarkdownExtensions.YamlDeserializer.Deserialize<Post>(metadataTxt)
        post
