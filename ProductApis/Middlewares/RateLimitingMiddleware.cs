using System.Collections.Concurrent;

namespace ProductApis.Middlewares
{
    /*
     3. How to do Testing
        Use Postman or curl to send multiple requests to your API endpoint and verify that the rate-limiting logic is enforced.
     */
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _nextRequestDelegate;

        // Configuration for rate limiting
        private readonly int _maxRequestCount = 2;
        private readonly TimeSpan _timeoutWindow = TimeSpan.FromSeconds(10);//For a quick Testing //TimeSpan.FromMinutes(1);

        // ConcurrentDictionary to store request counts for each IP
        private readonly ConcurrentDictionary<string, ClientRequestInfo> _clients = new();

        public RateLimitingMiddleware(RequestDelegate nextRequestDelegate)
        {
            _nextRequestDelegate = nextRequestDelegate;
        }

        private class ClientRequestInfo
        {
            public DateTime LastRequestTime { get; set; }
            public int RequestCount { get; set; }
        }


        public async Task Invoke( HttpContext httpContext)
        {
            //Step1: get client IP address
            var clientIP = httpContext.Connection.RemoteIpAddress?.ToString();
            if (clientIP == null)
            {
                await _nextRequestDelegate(httpContext);
                return;
            }

            //Step2:  Get or add the client IP in the dictionary
            var currentClient = _clients.GetOrAdd(clientIP,
                                                  new ClientRequestInfo { 
                                                      LastRequestTime = DateTime.UtcNow ,
                                                      RequestCount = 0
                                                  });

            //Step3: Process Ratelimiting
            lock (currentClient) // Ensure thread safety for the specific client record
            {

                #region Reset count if the time window has passed
                if (DateTime.UtcNow - currentClient.LastRequestTime > _timeoutWindow )
                {
                    currentClient.LastRequestTime = DateTime.UtcNow;
                    currentClient.RequestCount = 0;

                }
                #endregion

                // Increment the request count
                currentClient.RequestCount++;

                // If the request count exceeds the limit, deny access
                #region CheckRequestCountLimit
                if(currentClient.RequestCount > _maxRequestCount)
                {
                    //set Response Info
                    httpContext.Response.ContentType = "text/plain";
                    httpContext.Response.Headers["Retry-After"] = _timeoutWindow.TotalSeconds.ToString();
                    httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    //await httpContext.Response.WriteAsync("Rate limit exceeded. Please try again later."); //Note: Compile Error Cannot await in the body of lock statement
                    httpContext.Response.WriteAsync("Rate limit exceeded. Please try again later.");
                    return;
                }
                #endregion
            }

            // Call the next middleware if within the rate limit
            await _nextRequestDelegate(httpContext);


        }
    }
}
