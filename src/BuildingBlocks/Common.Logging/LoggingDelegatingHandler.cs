using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Logging;

public class LoggingDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingDelegatingHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        // Request logging
        _logger.LogInformation("HTTP Request: {Method} {Url}", request.Method, request.RequestUri);

        if (request.Content != null)
        {
            var requestBody = await request.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Request Body: {RequestBody}", requestBody);
        }

        HttpResponseMessage response;

        try
        {
            var correlationId = _httpContextAccessor.HttpContext?.TraceIdentifier;

            if (!string.IsNullOrEmpty(correlationId))
            {
                request.Headers.Remove("X-Correlation-ID");
                request.Headers.Add("X-Correlation-ID", correlationId);
            }

            response = await base.SendAsync(request, cancellationToken);

            stopwatch.Stop();

            // Response logging
            _logger.LogInformation(
                "HTTP Response: {StatusCode} in {ElapsedMs}ms for {Method} {Url}",
                (int)response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                request.Method,
                request.RequestUri);

            if (response.Content != null)
            {
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogDebug("Response Body: {ResponseBody}", responseBody);
            }

            return response;
        }
        catch (OperationCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            // Timeout (very important in microservices)
            _logger.LogCritical(ex,
                "HTTP Timeout: {Method} {Url}",
                request.Method,
                request.RequestUri);
        }
        catch (HttpRequestException ex)
        {
            // General HTTP errors (DNS failure, connection refused, etc.)
            _logger.LogCritical(ex,
                "HTTP Request error: {Method} {Url} - {Message}",
                request.Method,
                request.RequestUri,
                ex.Message);
        }
        catch (System.Net.Sockets.SocketException ex)
        {
            // Low-level network issue
            _logger.LogCritical(ex,
                "Socket error: {Method} {Url} - {SocketErrorCode}",
                request.Method,
                request.RequestUri,
                ex.SocketErrorCode);
        }
        catch (Exception ex)
        {
            // Fallback
            _logger.LogCritical(ex,
                "Unexpected error during HTTP call: {Method} {Url}",
                request.Method,
                request.RequestUri);
        }

        return new HttpResponseMessage(HttpStatusCode.BadGateway)
        {
            RequestMessage = request
        };
    }
}