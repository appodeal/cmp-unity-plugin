#if (!UNITY_ANDROID && !UNITY_EDITOR && !UNITY_IOS) || APPODEAL_DEV_DUMMY

// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    internal class DummyConsentManager : IConsentManager
    {
        public event EventHandler<ConsentInfoUpdateFailedEventArgs> OnConsentInfoUpdateFailed { add { } remove { } }
        public event EventHandler OnConsentInfoUpdateSucceeded { add { } remove { } }
        public event EventHandler<ConsentFormLoadFailedEventArgs> OnConsentFormLoadFailed { add { } remove { } }
        public event EventHandler<ConsentFormLoadSucceededEventArgs> OnConsentFormLoadSucceeded { add { } remove { } }
        public event EventHandler<ConsentFormDismissedEventArgs> OnConsentFormDismissed { add { } remove { } }

        public ConsentStatus ConsentStatus { get => ConsentStatus.Unknown; }

        public void Load() => DisplayUnsupportedPlatformWarning();

        public void LoadAndShowConsentFormIfRequired() => DisplayUnsupportedPlatformWarning();

        public void RequestConsentInfoUpdate(ConsentInfoParameters consentInfoParameters) => DisplayUnsupportedPlatformWarning();

        public void Revoke() => DisplayUnsupportedPlatformWarning();

        private static void DisplayUnsupportedPlatformWarning()
        {
            Debug.LogWarning($"The Appodeal CMP Plugin only supports Editor, Android and iOS platforms!");
        }
    }
}
#endif
