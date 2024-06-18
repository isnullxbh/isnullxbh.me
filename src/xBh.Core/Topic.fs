namespace xBh.Core

open System
open Microsoft.FSharp.Core

type Topic =
    | CompositeTopic of string * Topic list

module Topic =
    let getName (t: Topic) =
        match t with
        | CompositeTopic (name, _) -> name

    let getSubtopics (t: Topic) =
        match t with
        | CompositeTopic (_, child) -> child

    let hasSubtopic (s: string) (t: Topic) =
        match t with
        | CompositeTopic (_, child) ->
            List.exists (fun t -> match t with | CompositeTopic (name, _) -> name = s) child

    let getSubtopic (s: string) (t: Topic) : Topic option =
        let rec find (s: string) (l: Topic list) =
            match l with
            | [] -> None
            | head::tail ->
                if (getName head) = s
                    then Some(head)
                    else find s tail

        find s (getSubtopics t)

    let ofString (s: string) =
        let rec make (t: Topic) (p: string list) =
            match p with
            | [] -> t
            | head::tail ->
                match t with
                | CompositeTopic (name, child) ->
                    let node = make (CompositeTopic(head, [])) tail
                    CompositeTopic (name, child @ [node])

        let parts = s.Split([|'.'|]) |> Array.toList
        make (CompositeTopic (parts.Head, [])) parts.Tail

    let merge (t1: Topic) (t2: Topic) =
        let rec iter (t: Topic) (s: Topic list) =
            match s with
            | [] -> t
            | head::tail ->
                let t' =
                    match getSubtopic (getName head) t with
                    | Some s ->
                        let subtopicsWithoutS = List.filter (fun t -> getName t <> getName head) (getSubtopics t)
                        let s' = iter s (getSubtopics head)
                        CompositeTopic (getName t, subtopicsWithoutS @ [s'])
                    | None   -> CompositeTopic (getName t, (getSubtopics t) @ [head])
                iter t' tail

        let result =
            match (getName t1, getName t2) with
            | a, b when a = b -> iter t1 (getSubtopics t2) 
            | "root", _       -> iter t1 [t2]
            | _, "root"       -> iter t2 [t1]
            | _, _            -> iter (CompositeTopic ("root", [t1])) [t2]

        result

    let rec print (t: Topic) =
        let rec printImpl (t: Topic) (depth: int) =
            let offset = String.replicate depth "  "
            match t with
            | CompositeTopic (name, child) ->
                printf $"{offset}- {name}\n"
                List.map (fun t -> printImpl t (depth + 1)) child |> ignore

        printImpl t 0

    let rec traverse (f: Topic -> unit) (t: Topic) =
        match t with
        | CompositeTopic (_, child) ->
            (t |> f) |> ignore
            List.map (traverse f) child |> ignore

    let rec fold<'state> (folder: 'state -> Topic -> 'state) (s: 'state) (t: Topic) =
        let rec f (folder: 'state -> Topic -> 'state) (s: 'state) (t: Topic list) =
            match t with
            | [] -> s
            | head::tail -> f folder (fold folder s head) tail

        match t with
        | CompositeTopic (_, child) ->
            let s' = folder s t
            f folder s' child

    let rec fold2<'state> (folder: 'state -> Topic -> string -> 'state) (s: 'state) (t: Topic) (path: string) =
        let rec f (folder: 'state -> Topic -> string -> 'state) (s: 'state) (t: Topic list) (path: string) =
            match t with
            | [] -> s
            | head::tail -> f folder (fold2 folder s head path) tail path

        match t with
        | CompositeTopic (name, child) ->
            let s' = folder s t path
            let path' =
                match path with
                | "" -> name
                | _  -> path + "." + name
            f folder s' child path'
