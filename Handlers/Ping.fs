namespace Handlers.Ping

open Microsoft.AspNetCore.Http
open Giraffe

module GET =

    let ping: HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) -> text "pong" next ctx
