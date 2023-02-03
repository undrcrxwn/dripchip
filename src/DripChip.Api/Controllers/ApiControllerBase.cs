using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

[ApiController, Route("api/[controller]")]
public abstract class ApiControllerBase : Controller
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator
        ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}