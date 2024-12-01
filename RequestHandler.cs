public abstract class RequestHandler
{
    protected RequestHandler _nextHandler;

    public void SetNext(RequestHandler nextHandler)
    {
        _nextHandler = nextHandler;
    }

    public abstract string HandleRequest(Request request);
}

public class LoggingHandler : RequestHandler
{
    public override string HandleRequest(Request request)
    {
        if (request.Type == RequestType.Logging)
        {
            return $"Logging: {request.Content}";
        }
        else if (_nextHandler != null)
        {
            return _nextHandler.HandleRequest(request);
        }
        return null;
    }
}

public class AuthenticationHandler : RequestHandler
{
    public override string HandleRequest(Request request)
    {
        if (request.Type == RequestType.Authentication)
        {
            return $"Authenticating: {request.Content}";
        }
        else if (_nextHandler != null)
        {
            return _nextHandler.HandleRequest(request);
        }
        return null;
    }
}

public class NotificationHandler : RequestHandler
{
    public override string HandleRequest(Request request)
    {
        if (request.Type == RequestType.Notification)
        {
            return $"Notifying: {request.Content}";
        }
        else if (_nextHandler != null)
        {
            return _nextHandler.HandleRequest(request);
        }
        return null;
    }
}

public enum RequestType
{
    Logging,
    Authentication,
    Notification
}

public class Request
{
    public RequestType Type { get; }
    public string Content { get; }

    public Request(RequestType type, string content)
    {
        Type = type;
        Content = content;
    }
}
