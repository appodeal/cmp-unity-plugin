// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ConsentFormDismissedEventArgs : EventArgs
    {
        public ConsentManagerError? Error { get; }

        public ConsentFormDismissedEventArgs(ConsentManagerError? error = null)
        {
            Error = error;
        }
    }
}
