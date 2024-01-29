#if UNITY_ANDROID || APPODEAL_DEV_ANDROID

// ReSharper Disable CheckNamespace

using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AndroidConsentInfoUpdateListener : AndroidJavaProxy, IAndroidConsentInfoUpdateListener
    {
        private readonly IAndroidConsentInfoUpdateListener _listener;

        internal AndroidConsentInfoUpdateListener(IAndroidConsentInfoUpdateListener listener) : base("com.appodeal.consent.ConsentInfoUpdateCallback")
        {
            _listener = listener;
        }

        public void onUpdated()
        {
            SyncContextHelper.Post(obj => _listener?.onUpdated());
        }

        public void onFailed(AndroidJavaObject cause)
        {
            SyncContextHelper.Post(obj => _listener?.onFailed(cause));
        }
    }
}
#endif
