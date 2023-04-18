using DripChip.Application.Abstractions;
using DripChip.Application.Features.LocationPoints.Commands;
using DripChip.Application.Features.LocationPoints.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

public sealed class LocationsController : ApiControllerBase
{
    private readonly IGeoHasher _hasher;

    public LocationsController(IGeoHasher hasher) => _hasher = hasher;

    [HttpGet("{pointId}")]
    public async Task<GetById.Response> GetById([FromRoute] long pointId) =>
        await Mediator.Send(new GetById.Query(pointId));

    [HttpGet]
    public async Task<long> Find([FromQuery] Find.Query query)
    {
        var response = await Mediator.Send(query);
        return response.Id;
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<Create.Response>> Create([FromBody] Create.Command command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { PointId = response.Id }, response);
    }

    [HttpPut("{pointId}"), Authorize]
    public async Task<Update.Response> Update([FromRoute] long pointId, [FromBody] Update.Command command) =>
        await Mediator.Send(command with { Id = pointId });

    [HttpDelete("{pointId}"), Authorize]
    public async Task Delete([FromRoute] long pointId) =>
        await Mediator.Send(new Delete.Command(pointId));

    #region GeoHash

    [HttpGet("geohash")]
    public string GeoHashV1([FromQuery] double latitude, [FromQuery] double longitude) =>
        _hasher.EncodeV1(latitude, longitude);

    [HttpGet("geohashv2")]
    public string GeoHashV2([FromQuery] double latitude, [FromQuery] double longitude) =>
        _hasher.EncodeV2(latitude, longitude);

    [HttpGet("geohashv3")]
    public string GeoHashV3([FromQuery] double latitude, [FromQuery] double longitude) =>
        _hasher.EncodeV3(latitude, longitude);

    #endregion
}