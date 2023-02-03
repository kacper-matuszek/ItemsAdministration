using ItemsAdministration.Common.Infrastructure.Hosting.Descriptions;
using ItemsAdministration.Common.Infrastructure.Hosting.Localizations.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Formatters.Base;

internal abstract class ExceptionResponseFormatter<TException>
    where TException : Exception
{
    protected ExceptionResponseFormatter(ILogger<ExceptionResponseFormatter<TException>> logger, ILocalizationService localizationService)
    {
        Logger = logger;
        LocalizationService = localizationService;
    }

    protected ILogger<ExceptionResponseFormatter<TException>> Logger { get; }
    protected ILocalizationService LocalizationService { get; }

    public async Task Format(HttpContext context, Stream stream, Exception exception)
    {
        if (context.Response.HasStarted)
        {
            throw new InvalidOperationException("Http context already responsing a message.");
        }

        context.Response.Clear();
        context.Response.StatusCode = GetStatusCode();
        context.Response.ContentType = GetContentType();

        await UpdateStream(stream, (TException)exception);
    }

    public string GetSerializedMessages(Exception exception, JsonSerializerOptions? options = null) =>
        JsonSerializer.Serialize(GetErrorDescriptions((TException)exception), options);

    protected virtual int GetStatusCode() => 400;
    protected virtual string GetContentType() => @"application/json";

    protected abstract IEnumerable<ErrorResponseDescription> GetErrorDescriptions(TException exception);

    protected async Task UpdateStream(Stream stream, TException exception)
    {
        byte[]? bytes = null;
        await Task.Run(() => bytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(GetErrorDescriptions(exception))));
        stream.Seek(0, SeekOrigin.Begin);
        await stream.WriteAsync(bytes!, 0, bytes!.Length);
        stream.Seek(0, SeekOrigin.Begin);
    }
}
