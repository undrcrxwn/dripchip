using DripChip.Api.Attributes;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

[ApiController, ApiRoute("[controller]")]
public abstract class ApiControllerBase : Controller
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator
        ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}