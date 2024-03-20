using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TogetherBoardsApp.Backend.Api.UserAccounts.Requests;
using TogetherBoardsApp.Backend.Application.UserAccounts.LogInUserAccount;
using TogetherBoardsApp.Backend.Application.UserAccounts.LogOutUserAccount;
using TogetherBoardsApp.Backend.Application.UserAccounts.RefreshUserAccountToken;

namespace TogetherBoardsApp.Backend.Api.UserAccounts;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }
    
    [AllowAnonymous]
    [HttpPost("log-in")]
    public async Task<IActionResult> LogIn(
        LogInRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LogInUserAccountCommand(request.Email, request.Password);

        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [Authorize]
    [HttpPost("log-out")]
    public async Task<IActionResult> LogOut(
        CancellationToken cancellationToken)
    {
        var command = new LogOutUserAccountCommand();

        await _sender.Send(command, cancellationToken);

        return Ok();
    }
    
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RefreshUserAccountTokenCommand(request.RefreshToken);

        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }
}