using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ItemsAdministration.Application.Abstractions.Commands;
using ItemsAdministration.Application.Abstractions.Queries;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Dispatchers;
using ItemsAdministration.Common.Infrastructure.Hosting;
using ItemsAdministration.Common.Shared.Responses;
using ItemsAdministration.Infrastructure.Api.Consts;
using ItemsAdministration.PublishedLanguage.Requests;
using ItemsAdministration.PublishedLanguage.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItemsAdministration.Infrastructure.Api.Controllers;

[ApiController]
[Authorize]
[Route("items")]
public class ItemController : BaseController
{
    public ItemController(IDispatcher dispatcher, IMapper mapper)
        : base(dispatcher, mapper) { }

    [HttpPost]
    [Authorize(Roles = RoleNames.ItemsManagement)]
    public async Task<IActionResult> Create(CreateItemRequest request, CancellationToken cancellationToken = default)
    {
        var command = Mapper.Map<CreateItemCommand>(request);
        await Dispatcher.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPut]
    [Authorize(Roles = RoleNames.ItemsManagement)]
    public async Task<IActionResult> Update(UpdateItemRequest request, CancellationToken cancellationToken = default)
    {
        var command = Mapper.Map<UpdateItemCommand>(request);
        await Dispatcher.Send(command, cancellationToken);
        return Ok();
    }

    [HttpGet]
    public Task<ItemResponse> Get(Guid id, CancellationToken cancellationToken = default) => 
        Dispatcher.Query(new GetItemQuery(id), cancellationToken);

    [HttpGet("pagination")]
    public async Task<PaginatedListResponse<ItemResponse>> Get(
        [FromQuery] GetPaginatedItemsRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = Mapper.Map<GetPaginatedItemsQuery>(request);
        var items = await Dispatcher.Query(query, cancellationToken);
        return Mapper.Map<PaginatedListResponse<ItemResponse>>(items);
    }
}