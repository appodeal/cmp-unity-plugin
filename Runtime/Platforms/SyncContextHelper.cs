#if UNITY_ANDROID || UNITY_IOS || APPODEAL_DEV_ANDROID || APPODEAL_DEV_IOS

// ReSharper Disable CheckNamespace

using System.Threading;
using UnityEngine;

namespace AppodealStack.Cmp
{
    internal static class SyncContextHelper
    {
        private static SynchronizationContext _unitySynchronizationContext;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GetContext() => _unitySynchronizationContext = SynchronizationContext.Current;

        public static void Post(SendOrPostCallback d, object state = null) => _unitySynchronizationContext.Post(d, state);
    }
}
#endif
