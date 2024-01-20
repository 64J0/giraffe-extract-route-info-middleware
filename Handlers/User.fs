namespace Handlers.User

open Microsoft.AspNetCore.Http
open Giraffe

// to extract information from http context
// not used for now
// type RouteInfo =
//     | UserId of string
//     | UserIdAndProductId of string * string

type UserId = string
type UserIdAndProductId = string * string

module GET =

    let printUserId: HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            let routeInfo = Middlewares.getRouteInfo<UserId> ctx

            match routeInfo with
            | Ok userId ->
                setStatusCode 200 |> ignore
                text $"User id: {userId}" next ctx
            | Error err ->
                setStatusCode 400 |> ignore
                text err next ctx

    let printUserIdAndProductId: HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            let routeInfo = Middlewares.getRouteInfo<UserIdAndProductId> ctx

            match routeInfo with
            | Ok(userId, productId) ->
                setStatusCode 200 |> ignore
                text $"User id: {userId}, Product id: {productId}" next ctx
            | Error err ->
                setStatusCode 400 |> ignore
                text err next ctx
