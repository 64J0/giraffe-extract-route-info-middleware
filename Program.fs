open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe

let SUCCESS_CODE = 0
let FAILURE_CODE = 1

let webApp =
    choose
        [ GET >=> route "/ping" >=> setStatusCode 200 >=> text "pong"

          GET
          >=> routef "/users/%s" Middlewares.extractRouteInfo
          >=> Handlers.User.GET.printUserId

          GET
          >=> routef "/users/%s/%s" Middlewares.extractRouteInfo
          >=> Handlers.User.GET.printUserIdAndProductId

          // If none of the routes matched then return a 404
          setStatusCode 404 >=> RequestErrors.NOT_FOUND "Not Found" ]

let configureApp (app: IApplicationBuilder) = app.UseGiraffe webApp

let configureServices (services: IServiceCollection) = services.AddGiraffe() |> ignore

[<EntryPoint>]
let main (_args: string array) : int =
    try
        Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(fun webHostBuilder ->
                webHostBuilder
                    .Configure(configureApp)
                    .ConfigureServices(configureServices)
                    .UseUrls([| "http://localhost:3000" |])
                |> ignore)
            .Build()
            .Run()

        SUCCESS_CODE
    with exn ->
        [ "Server did not start successfully"; $"{exn.Message}"; $"{exn.StackTrace}" ]
        |> List.iter (eprintfn "%s")

        FAILURE_CODE
