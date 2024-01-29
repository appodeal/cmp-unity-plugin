#if UNITY_ANDROID || APPODEAL_DEV_ANDROID

// ReSharper Disable CheckNamespace

using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AndroidConsentFormLoadFailedListener : AndroidJavaProxy, IAndroidConsentFormLoadFailedListener
    {
        private readonly IAndroidConsentFormLoadFailedListener _listener;

        internal AndroidConsentFormLoadFailedListener(IAndroidConsentFormLoadFailedListener listener) : base("com.appodeal.consent.OnConsentFormLoadFailureListener")
        {
            _listener = listener;
        }

        public void onConsentFormLoadFailure(AndroidJavaObject cause)
        {
            SyncContextHelper.Post(obj => _listener?.onConsentFormLoadFailure(cause));
        }
    }
}
#endif
