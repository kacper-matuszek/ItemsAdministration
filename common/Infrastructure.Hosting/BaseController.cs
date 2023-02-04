using AutoMapper;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Dispatchers;
using Microsoft.AspNetCore.Mvc;

namespace ItemsAdministration.Common.Infrastructure.Hosting;

public abstract class BaseController : ControllerBase
{
    protected BaseController(IDispatcher dispatcher, IMapper mapper)
    {
        Dispatcher = dispatcher;
        Mapper = mapper;
    }

    protected IDispatcher Dispatcher { get; }
    protected IMapper Mapper { get; }
}