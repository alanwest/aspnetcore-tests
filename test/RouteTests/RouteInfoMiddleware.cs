using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using static System.Net.Mime.MediaTypeNames;

namespace RouteTests;

public class RouteInfoMiddleware
{
    private readonly RequestDelegate next;

    public RouteInfoMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var response = context.Response;
        
        var originBody = response.Body;
        context.Items["originBody"] = originBody;
        using var newBody = new MemoryStream();
        response.Body = newBody;

        await this.next(context);

        var stream = response.Body;
        using var reader = new StreamReader(stream, leaveOpen: true);
        var originalResponse = await reader.ReadToEndAsync();

        var info = context.Items["RouteInfo"] as RouteInfo;
        Debug.Assert(info != null);
        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        string modifiedResponse = JsonSerializer.Serialize(info, jsonOptions);

        stream.SetLength(0);
        using var writer = new StreamWriter(stream, leaveOpen: true);
        await writer.WriteAsync(modifiedResponse);
        await writer.FlushAsync();
        response.ContentLength = stream.Length;
        response.ContentType = "application/json";
        
        newBody.Seek(0, SeekOrigin.Begin);
        await newBody.CopyToAsync(originBody);
        response.Body = originBody;
    }

    public static void ConfigureExceptionHandler(IApplicationBuilder builder)
    {
        builder.Run(async context =>
        {
            context.Response.Body = (context.Items["originBody"] as Stream)!;

            context.Response.ContentType = Application.Json;

            var info = context.Items["RouteInfo"] as RouteInfo;
            Debug.Assert(info != null);
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            string modifiedResponse = JsonSerializer.Serialize(info, jsonOptions);
            await context.Response.WriteAsync(modifiedResponse);
            

            // var exceptionHandlerPathFeature =
            //     context.Features.Get<IExceptionHandlerPathFeature>();

            // if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            // {
            //     await context.Response.WriteAsync(" The file was not found.");
            // }

            // if (exceptionHandlerPathFeature?.Path == "/")
            // {
            //     await context.Response.WriteAsync(" Page: Home.");
            // }
        });
    }
}
