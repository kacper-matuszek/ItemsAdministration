using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Formatters.Interfaces;

public interface IExceptionResponseFormatter
{
    Task Format(HttpContext context, Stream stream, Exception exception);
    string GetSerializedMessages(Exception exception, JsonSerializerOptions? options = null);
}
