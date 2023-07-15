using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace RouteTests;

internal sealed class AspNetCoreDiagnosticObserver : IDisposable, IObserver<DiagnosticListener>, IObserver<KeyValuePair<string, object?>>
{
    internal const string ActivityOperationName = "Microsoft.AspNetCore.Hosting.HttpRequestIn";
    internal const string OnStartEvent = "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start";
    internal const string OnStopEvent = "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop";
    internal const string OnMvcBeforeActionEvent = "Microsoft.AspNetCore.Mvc.BeforeAction";
    internal const string OnUnhandledHostingExceptionEvent = "Microsoft.AspNetCore.Hosting.UnhandledException";
    internal const string OnUnHandledDiagnosticsExceptionEvent = "Microsoft.AspNetCore.Diagnostics.UnhandledException";

    private static readonly HashSet<string> DiagnosticSourceEvents = new()
    {
        "Microsoft.AspNetCore.Hosting.HttpRequestIn",
        "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start",
        "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop",
        "Microsoft.AspNetCore.Mvc.BeforeAction",
        "Microsoft.AspNetCore.Diagnostics.UnhandledException",
        "Microsoft.AspNetCore.Hosting.UnhandledException",
    };

    private readonly Func<string, object?, object?, bool> isEnabledFilter = (eventName, _, _)
        => true; // DiagnosticSourceEvents.Contains(eventName);

    private readonly List<IDisposable> listenerSubscriptions;
    private IDisposable? allSourcesSubscription;
    private long disposed;

    public AspNetCoreDiagnosticObserver()
    {
        this.listenerSubscriptions = new List<IDisposable>();
        this.allSourcesSubscription = DiagnosticListener.AllListeners.Subscribe(this);
    }

    public void OnNext(DiagnosticListener value)
    {
        if (value.Name == "Microsoft.AspNetCore")
        {
            var subscription = this.isEnabledFilter == null
                ? value.Subscribe(this)
                : value.Subscribe(this, this.isEnabledFilter);

            lock (this.listenerSubscriptions)
            {
                this.listenerSubscriptions.Add(subscription);
            }
        }
    }

    public void OnNext(KeyValuePair<string, object?> value)
    {
        HttpContext? context;
        BeforeActionEventData? actionMethodEventData;
        RouteInfo? info;

        switch (value.Key)
        {
            case OnStartEvent:
                context = value.Value as HttpContext;
                Debug.Assert(context != null);
                info = new RouteInfo();
                info.SetValues(context);
                context.Items["RouteInfo"] = info;
                break;
            case OnMvcBeforeActionEvent:
                actionMethodEventData = value.Value as BeforeActionEventData;
                Debug.Assert(actionMethodEventData != null);
                info = actionMethodEventData.HttpContext.Items["RouteInfo"] as RouteInfo;
                Debug.Assert(info != null);
                info.SetValues(actionMethodEventData.HttpContext);
                info.SetValues(actionMethodEventData.ActionDescriptor);
                break;
            case OnStopEvent:
                // Can't update RouteInfo here because the response is already written.
                break;
            case OnUnhandledHostingExceptionEvent:
            case OnUnHandledDiagnosticsExceptionEvent:
            default:
                break;
        }
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (Interlocked.CompareExchange(ref this.disposed, 1, 0) == 1)
        {
            return;
        }

        lock (this.listenerSubscriptions)
        {
            foreach (var listenerSubscription in this.listenerSubscriptions)
            {
                listenerSubscription?.Dispose();
            }

            this.listenerSubscriptions.Clear();
        }

        this.allSourcesSubscription?.Dispose();
        this.allSourcesSubscription = null;
    }
}
