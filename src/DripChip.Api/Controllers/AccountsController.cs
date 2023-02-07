using DripChip.Application.Abstractions.Common;
using DripChip.Application.Features.Accounts.Get;
using DripChip.Application.Features.Accounts.Get.ById;
using DripChip.Application.Features.Accounts.Register;
using DripChip.Application.Features.Accounts.Search;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

[Route("[controller]")]
public class AccountsController : ApiControllerBase
{
    private readonly IAccountService _accounts;

    public AccountsController(IAccountService accounts) =>
        _accounts = accounts;

    [HttpPost("~/registration")]
    public async Task<RegisterAccountResponse> Register([FromBody] RegisterAccountRequest request) =>
        await _accounts.RegisterAsync(request);

    [HttpGet("{accountId}")]
    public async Task<AccountResponse> GetById([FromRoute] int accountId)
    {
        var request = new GetAccountByIdRequest { AccountId = accountId };
        await new GetAccountByIdRequestValidator().ValidateAndThrowAsync(request);
        return await _accounts.GetByIdAsync(request);
    }

    [HttpGet("[action]")]
    public async Task<IEnumerable<AccountResponse>> Search([FromQuery] SearchAccountRequest request) =>
        await _accounts.SearchAsync(request);
}