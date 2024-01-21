module Middlewares

open Microsoft.AspNetCore.Http
open Giraffe

/// Extract the dynamic route information and add to the
/// HTTP context object
let extractRouteInfo (args: 'T) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        ctx.Items.Add("routeInfo", args)
        next ctx

/// Helper function that extracts the dynamic route information
/// from the HTTP context object
let getRouteInfo<'T> (ctx: HttpContext) : Result<'T, string> =
    let routeInfo = ctx.Items.TryGetValue "routeInfo"

    match routeInfo with
    | true, info -> Ok(info :?> 'T)
    | false, _ -> Error "Route info not available"
