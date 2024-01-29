// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ConsentInfoUpdateFailedEventArgs : EventArgs
    {
        public ConsentManagerError Cause { get; }

        public ConsentInfoUpdateFailedEventArgs(ConsentManagerError cause)
        {
            Cause = cause;
        }
    }
}
