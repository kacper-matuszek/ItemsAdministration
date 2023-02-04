using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ItemsAdministration.Application.Abstractions.Commands;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Dispatchers;
using ItemsAdministration.Common.Infrastructure.Hosting;
using ItemsAdministration.PublishedLanguage.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ItemsAdministration.Infrastructure.Api.Controllers;

[ApiController]
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
}