using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using src.Utilities;

namespace src.Features.Shared.Endpoints;

public abstract class BaseEndpoint<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor)
{
    private ILogger? _logger;
    
    // Access to per-request services
    protected HttpContext HttpContext => httpContextAccessor.HttpContext ??
                                         throw new InvalidOperationException("HttpContext is not available.");
    protected IServiceProvider Services => HttpContext.RequestServices;
    
    // DI helpers
    protected T GetRequired<T>() where T : notnull => Services.GetRequiredService<T>();
    protected T? GetService<T>() => Services.GetService<T>();
    
    // Logger helper on-demand
    protected ILogger Logger => _logger ??= GetRequired<ILoggerFactory>().CreateLogger(GetType());
    
    // User/Identity helpers
    protected ClaimsPrincipal User => HttpContext.User;
    protected bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;
    protected bool IsInRole(string role) => User.IsInRole(role);
    protected string? UserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
    
    // Result Helpers
    protected IResult Ok(object? value = null) => Results.Ok(value);
    protected IResult Created(string uri, object? value = null) => Results.Created(uri, value);
    protected IResult CreatedAt(string routeName, object? routeValues, object? value) => Results.CreatedAtRoute(routeName, routeValues, value);
    protected IResult NoContent() => Results.NoContent();
    protected IResult NotFound(object? value = null) => Results.NotFound(value);
    protected IResult BadRequest(object? error = null) => Results.BadRequest(error);
    protected IResult Unauthorized() => Results.Unauthorized();
    protected IResult Forbid(params string[] authenticationSchemes) => Results.Forbid(authenticationSchemes: authenticationSchemes);
    protected IResult Problem(string title, int status = 500, string? detail = null, string? instance = null) =>
        Results.Problem(title: title, statusCode: status, detail: detail, instance: instance);
    protected IResult ValidationProblem(IDictionary<string, string[]> errors, int status = 400) =>
        Results.ValidationProblem(errors, statusCode: status);
    
    // Route/Query helpers
    protected T? RouteValue<T>(string key)
    {
        if (!HttpContext.Request.RouteValues.TryGetValue(key, out object? raw) || raw is null)
            return default;

        try
        {
            Type targetType = typeof(T);

            if (targetType != Types.Int && targetType != Types.NullableInt ||
                targetType != Types.Guid && targetType != Types.NullableGuid)
                return (T?)Convert.ChangeType(raw, typeof(T));

            if (int.TryParse(raw.ToString(), out int integer))
                return (T?)(object)integer;
            
            if (Guid.TryParse(raw.ToString(), out Guid guid))
                return (T?)(object)guid;
            
            return default;
        }
        catch
        {
            return default;
        }
    }
    
    protected string? Query(string key) => 
        HttpContext.Request.Query.TryGetValue(key, out StringValues value) 
            ? value.ToString() 
            : null;

    protected (int page, int pageSize) Paging(
        int? page, int? pageSize,
        int defaultPage = 1, int defaultPageSize = 20,
        int maxPageSize = 100)
    {
        int p = page.GetValueOrDefault(defaultPage);
        int pSize = Math.Clamp(pageSize.GetValueOrDefault(defaultPageSize), 1, maxPageSize);
        return (p < 1 ? defaultPage : p, pSize);
    }
    
    // ETag helpers (recommended: use resource version when possible; otherwise compute a hash)
    protected StringSegment ComputeETag(
        object payload, 
        JsonSerializerOptions? options = null)
    {
        string json = JsonSerializer.Serialize(payload, options);
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(json));
        StringSegment base64 = Convert.ToBase64String(hash);

        return base64;
    }
    protected bool IsNotModified(StringSegment etag)
    {
        RequestHeaders? typedHeaders = HttpContext.Request.GetTypedHeaders();
        if (typedHeaders?.IfNoneMatch == null || typedHeaders.IfNoneMatch.Count == 0)
            return false;

        if (typedHeaders.IfNoneMatch.Any(value => value.Tag == "*"))
            return true;

        foreach (EntityTagHeaderValue value in typedHeaders.IfNoneMatch)
        {
            if (StringSegmentComparer.Ordinal.Equals(value.Tag, etag))
                return true;

            StringSegment valueTag = value.Tag.Trim();
            if (valueTag != StringSegment.Empty && valueTag == etag)
                return true;
        }
        
        return false;
    }

    protected void SetETag(StringSegment etag)
    {
        ResponseHeaders headers = HttpContext.Response.GetTypedHeaders();
        headers.ETag = EntityTagHeaderValue.Parse(etag);
    }
        
    
    /// Validation hook (override if needed)
    protected virtual (bool IsValid, IDictionary<string, string[]> Errors) ValidateRequest(TRequest request)
        => (true, new Dictionary<string, string[]>());

    /// Safe executor to unify try/catch, logging and validation
    protected async Task<IResult> ExecuteAsync(
        TRequest request,
        Func<CancellationToken, Task<IResult>> action,
        Func<Exception, IResult>? onError = null,
        bool validate = true,
        CancellationToken ct = default)
    {
        if (validate)
        {
            (bool isValid, IDictionary<string, string[]> errors) = ValidateRequest(request);
            if (!isValid)
                return ValidationProblem(errors);
        }

        try
        {
            return await action(ct).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return Results.StatusCode(StatusCodes.Status499ClientClosedRequest);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Unhandled exception in {Endpoint}", GetType().Name);
            return onError?.Invoke(e) ?? Problem("An unexpected error occured.");
        }
    }

    /// Map / Configure endpoints. Override in the derived endpoints
    public virtual void Configure(IEndpointRouteBuilder app)
    { }
}