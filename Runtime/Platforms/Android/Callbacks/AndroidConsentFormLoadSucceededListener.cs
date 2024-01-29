#if UNITY_ANDROID || APPODEAL_DEV_ANDROID

// ReSharper Disable CheckNamespace

using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AndroidConsentFormLoadSucceededListener : AndroidJavaProxy, IAndroidConsentFormLoadSucceededListener
    {
        private readonly IAndroidConsentFormLoadSucceededListener _listener;

        internal AndroidConsentFormLoadSucceededListener(IAndroidConsentFormLoadSucceededListener listener) : base("com.appodeal.consent.OnConsentFormLoadSuccessListener")
        {
            _listener = listener;
        }

        public void onConsentFormLoadSuccess(AndroidJavaObject consentForm)
        {
            SyncContextHelper.Post(obj => _listener?.onConsentFormLoadSuccess(consentForm));
        }
    }
}
#endif
