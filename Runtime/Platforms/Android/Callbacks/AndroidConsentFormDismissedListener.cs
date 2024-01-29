#if UNITY_ANDROID || APPODEAL_DEV_ANDROID

// ReSharper Disable CheckNamespace

using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AndroidConsentFormDismissedListener : AndroidJavaProxy, IAndroidConsentFormDismissedListener
    {
        private readonly IAndroidConsentFormDismissedListener _listener;

        internal AndroidConsentFormDismissedListener(IAndroidConsentFormDismissedListener listener) : base("com.appodeal.consent.OnConsentFormDismissedListener")
        {
            _listener = listener;
        }

        public void onConsentFormDismissed(AndroidJavaObject error)
        {
            SyncContextHelper.Post(obj => _listener?.onConsentFormDismissed(error));
        }
    }
}
#endif
