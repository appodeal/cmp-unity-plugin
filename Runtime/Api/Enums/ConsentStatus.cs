// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum ConsentStatus
    {
        Unknown,
        Required,
        NotRequired,
        Obtained,
    }
}
