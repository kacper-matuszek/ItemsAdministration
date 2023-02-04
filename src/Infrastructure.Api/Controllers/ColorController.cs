using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ItemsAdministration.Application.Abstractions.Commands;
using ItemsAdministration.Application.Abstractions.Queries;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Dispatchers;
using ItemsAdministration.Common.Infrastructure.Hosting;
using ItemsAdministration.Common.Shared.Responses;
using ItemsAdministration.PublishedLanguage.Requests;
using ItemsAdministration.PublishedLanguage.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItemsAdministration.Infrastructure.Api.Controllers;

[ApiController]
[Authorize]
[Route("colors")]
public class ColorController : BaseController
{
    public ColorController(IDispatcher dispatcher, IMapper mapper)
        : base(dispatcher, mapper) { }

    [HttpPost]
    public async Task<IActionResult> Create(CreateColorRequest request, CancellationToken cancellationToken = default)
    {
        var command = Mapper.Map<CreateColorCommand>(request);
        await Dispatcher.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateColorRequest request, CancellationToken cancellationToken = default)
    {
        var command = Mapper.Map<UpdateColorCommand>(request);
        await Dispatcher.Send(command, cancellationToken);
        return Ok();
    }

    [HttpGet]
    public Task<ColorResponse> Get(Guid id, CancellationToken cancellationToken = default) =>
        Dispatcher.Query(new GetColorQuery(id), cancellationToken);

    [HttpGet("pagination")]
    public async Task<PaginatedListResponse<ColorResponse>> Get(
        [FromQuery] GetPaginatedColorsRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = Mapper.Map<GetPaginatedColorsQuery>(request);
        var colors = await Dispatcher.Query(query, cancellationToken);
        return Mapper.Map<PaginatedListResponse<ColorResponse>>(colors);
    }
}