using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ItemsAdministration.Application.Abstractions.Commands;
using ItemsAdministration.Application.Abstractions.Queries;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Dispatchers;
using ItemsAdministration.Common.Infrastructure.Hosting;
using ItemsAdministration.PublishedLanguage.Requests;
using ItemsAdministration.PublishedLanguage.Response;
using Microsoft.AspNetCore.Mvc;

namespace ItemsAdministration.Infrastructure.Api.Controllers;

[ApiController]
[Route("items")]
public class ItemController : BaseController
{
    public ItemController(IDispatcher dispatcher, IMapper mapper)
        : base(dispatcher, mapper) { }

    [HttpPost]
    public async Task<IActionResult> Create(CreateItemRequest request, CancellationToken cancellationToken = default)
    {
        var command = Mapper.Map<CreateItemCommand>(request);
        await Dispatcher.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateItemRequest request, CancellationToken cancellationToken = default)
    {
        var command = Mapper.Map<UpdateItemCommand>(request);
        await Dispatcher.Send(command, cancellationToken);
        return Ok();
    }

    [HttpGet]
    public Task<ItemResponse> Get(Guid id, CancellationToken cancellationToken = default) => 
        Dispatcher.Query(new GetItemQuery(id), cancellationToken);
}