using AutoMapper;
using ItemsAdministration.Common.Infrastructure.Hosting.Formatters.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Middlewares;

public class ExceptionResponseMiddleware
{
    private readonly IExceptionResponseFormatterFactory _exceptionFormatterFactory;
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionResponseMiddleware> _logger;

    public ExceptionResponseMiddleware(
        RequestDelegate next,
        IExceptionResponseFormatterFactory exceptionFormatterFactory,
        ILogger<ExceptionResponseMiddleware> logger)
    {
        _next = next;
        _exceptionFormatterFactory = exceptionFormatterFactory;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        using var memory = new MemoryStream();

        var originalStream = context.Response.Body;
        context.Response.Body = memory;

        try
        {
            await _next.Invoke(context);
        }
        catch (AggregateException exc)
        {
            var exception = exc.InnerException ?? exc;

            if (exc.InnerException is not null)
                await FormatException(context, memory, exc.InnerException);

            _logger.LogError(exception, message: null);
        }
        catch (Exception exc)
        {
            var exception = exc is AutoMapperMappingException ? exc.InnerException : exc;

            await FormatException(context, memory, exception!);
            _logger.LogError(exception, message: null);
        }

        memory.Seek(0, SeekOrigin.Begin);

        await memory.CopyToAsync(originalStream);
        context.Response.Body = originalStream;
    }

    private Task FormatException(HttpContext context, MemoryStream memory, Exception exception) =>
        _exceptionFormatterFactory.Create(exception).Format(context, memory, exception);
}
