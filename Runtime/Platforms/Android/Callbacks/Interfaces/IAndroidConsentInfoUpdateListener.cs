#if UNITY_ANDROID || APPODEAL_DEV_ANDROID

// ReSharper Disable CheckNamespace

using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal interface IAndroidConsentInfoUpdateListener
    {
        void onFailed(AndroidJavaObject cause);
        void onUpdated();
    }
}
#endif
