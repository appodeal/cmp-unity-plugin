#if UNITY_IOS || APPODEAL_DEV_IOS

// ReSharper disable CheckNamespace

using System;

namespace AppodealStack.Cmp
{
    internal delegate void ConsentInfoUpdateFailedCallback(int cause);
    internal delegate void ConsentInfoUpdateSucceededCallback();
    internal delegate void ConsentFormLoadFailedCallback(int cause);
    internal delegate void ConsentFormLoadSucceededCallback(IntPtr iosConsentFormPtr);
    internal delegate void ConsentFormDismissedCallback(int error);
}
#endif
