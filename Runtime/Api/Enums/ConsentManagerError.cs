// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum ConsentManagerError
    {
        TimeoutError,
        RequestError,
        ServerError,
        FormCachingFailed,
        FormIsNotRequested,
        FormIsAlreadyBeingPresented,
        FormPresentationIsNotRequired,
        FormIsNotReady,
        InternalError,
        Unspecified,
        ActivityIsDestroyed,
    }
}
