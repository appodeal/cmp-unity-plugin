// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ConsentFormLoadSucceededEventArgs : EventArgs
    {
        public ConsentForm ConsentForm { get; }

        public ConsentFormLoadSucceededEventArgs(ConsentForm consentForm)
        {
            ConsentForm = consentForm;
        }
    }
}
