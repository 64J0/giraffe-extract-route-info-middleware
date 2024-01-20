module Middlewares

open Microsoft.AspNetCore.Http
open Giraffe

let extractRouteInfo (args: 'T) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        ctx.Items.Add("routeInfo", args)
        next ctx

// TODO extract to another location
let getRouteInfo<'T> (ctx: HttpContext) : Result<'T, string> =
    let routeInfo = ctx.Items.TryGetValue "routeInfo"

    match routeInfo with
    | true, info -> Ok(info :?> 'T)
    | false, _ -> Error "Route info not available"
