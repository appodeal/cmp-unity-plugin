// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ConsentFormLoadFailedEventArgs : EventArgs
    {
        public ConsentManagerError Cause { get; }

        public ConsentFormLoadFailedEventArgs(ConsentManagerError cause)
        {
            Cause = cause;
        }
    }
}
