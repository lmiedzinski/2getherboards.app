using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TogetherBoardsApp.Backend.Application.UserAccounts.GetCurrentLoggedInUserAccount;

namespace TogetherBoardsApp.Backend.Api.UserAccounts;

[ApiController]
[Route("api/user-accounts")]
public class UserAccountsController : ControllerBase
{
    private readonly ISender _sender;

    public UserAccountsController(ISender sender)
    {
        _sender = sender;
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentLoggedInUserAccount(
        CancellationToken cancellationToken)
    {
        var query = new GetCurrentLoggedInUserAccountQuery();

        var result = await _sender.Send(query, cancellationToken);

        return Ok(result);
    }
}