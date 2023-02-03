using System;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Formatters.Interfaces;

public interface IExceptionResponseFormatterFactory
{
    IExceptionResponseFormatter Create(Exception exception);
}
