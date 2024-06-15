namespace xBh.Core

open System

type Post =
    { Title: string
      Topic: Topic
      Tags: Set<string>
      CreatedAt: DateTime
      Path: string }

type PostsPerTopic =
    { Topic: Topic
      Posts: Post list
      Level: int }
