// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public interface IConsentManager
    {
        event EventHandler<ConsentInfoUpdateFailedEventArgs> OnConsentInfoUpdateFailed;
        event EventHandler OnConsentInfoUpdateSucceeded;
        event EventHandler<ConsentFormLoadFailedEventArgs> OnConsentFormLoadFailed;
        event EventHandler<ConsentFormLoadSucceededEventArgs> OnConsentFormLoadSucceeded;
        event EventHandler<ConsentFormDismissedEventArgs> OnConsentFormDismissed;

        ConsentStatus ConsentStatus { get; }

        void Load();
        void LoadAndShowConsentFormIfRequired();
        void RequestConsentInfoUpdate(ConsentInfoParameters consentInfoParameters);
        void Revoke();
    }
}
