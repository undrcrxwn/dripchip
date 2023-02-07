using DripChip.Application.Features.Accounts.Get;
using DripChip.Application.Features.Accounts.Get.ById;
using DripChip.Application.Features.Accounts.Register;
using DripChip.Application.Features.Accounts.Search;

namespace DripChip.Application.Abstractions.Common;

public interface IAccountService
{
    public Task<RegisterAccountResponse> RegisterAsync(RegisterAccountRequest request);
    public Task<AccountResponse> GetByIdAsync(GetAccountByIdRequest request);
    public Task<IEnumerable<AccountResponse>> SearchAsync(SearchAccountRequest request);
}