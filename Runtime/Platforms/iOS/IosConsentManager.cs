#if UNITY_IOS || APPODEAL_DEV_IOS

// ReSharper disable CheckNamespace

using System;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using AOT;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class IosConsentManager : IConsentManager
    {
        private static IosConsentManager _instance;

        public event EventHandler<ConsentInfoUpdateFailedEventArgs> OnConsentInfoUpdateFailed;
        public event EventHandler OnConsentInfoUpdateSucceeded;
        public event EventHandler<ConsentFormLoadFailedEventArgs> OnConsentFormLoadFailed;
        public event EventHandler<ConsentFormLoadSucceededEventArgs> OnConsentFormLoadSucceeded;
        public event EventHandler<ConsentFormDismissedEventArgs> OnConsentFormDismissed;

        internal IosConsentManager() { _instance = this; }

        public ConsentStatus ConsentStatus { get => CmpConsentManagerGetConsentStatus(); }

        public void Load()
        {
            CmpConsentManagerLoad(ConsentFormLoadFailed, ConsentFormLoadSucceeded);
        }

        public void LoadAndShowConsentFormIfRequired()
        {
            CmpConsentManagerLoadAndShowConsentFormIfRequired(ConsentFormDismissed);
        }

        public void RequestConsentInfoUpdate(ConsentInfoParameters consentInfoParameters)
        {
            var paramsStruct = IosConsentInfoParameters.FromDtoClass(consentInfoParameters);
            var paramsStructPtr = Marshal.AllocHGlobal(Marshal.SizeOf(paramsStruct));
            try
            {
                Marshal.StructureToPtr(paramsStruct, paramsStructPtr, false);
                CmpConsentManagerRequestConsentInfoUpdate(paramsStructPtr, ConsentInfoUpdateFailed, ConsentInfoUpdateSucceeded);
            }
            finally
            {
                Marshal.FreeHGlobal(paramsStructPtr);
            }
        }

        public void Revoke()
        {
            CmpConsentManagerRevoke();
        }

        [DllImport("__Internal", EntryPoint = "CMPUnityPluginConsentManagerGetConsentStatus")]
        private static extern ConsentStatus CmpConsentManagerGetConsentStatus();

        [DllImport("__Internal", EntryPoint = "CMPUnityPluginConsentManagerLoad")]
        private static extern void CmpConsentManagerLoad(
            ConsentFormLoadFailedCallback onConsentFormLoadFailed,
            ConsentFormLoadSucceededCallback onConsentFormLoadSucceeded
        );

        [DllImport("__Internal", EntryPoint = "CMPUnityPluginConsentManagerLoadAndShowConsentFormIfRequired")]
        private static extern void CmpConsentManagerLoadAndShowConsentFormIfRequired(ConsentFormDismissedCallback onConsentFormDismissed);

        [DllImport("__Internal", EntryPoint = "CMPUnityPluginConsentManagerRequestConsentInfoUpdate")]
        private static extern void CmpConsentManagerRequestConsentInfoUpdate(
            IntPtr consentInfoParametersPtr,
            ConsentInfoUpdateFailedCallback onConsentInfoUpdateFailed,
            ConsentInfoUpdateSucceededCallback onConsentInfoUpdateSucceeded
        );

        [DllImport("__Internal", EntryPoint = "CMPUnityPluginConsentManagerRevoke")]
        private static extern void CmpConsentManagerRevoke();

        [MonoPInvokeCallback(typeof(ConsentInfoUpdateFailedCallback))]
        private static void ConsentInfoUpdateFailed(int cause)
        {
            var args = new ConsentInfoUpdateFailedEventArgs(IosCmpHelper.GetConsentManagerErrorFromInt(cause));
            SyncContextHelper.Post(state => _instance.OnConsentInfoUpdateFailed?.Invoke(_instance, args));
        }

        [MonoPInvokeCallback(typeof(ConsentInfoUpdateSucceededCallback))]
        private static void ConsentInfoUpdateSucceeded()
        {
            SyncContextHelper.Post(state => _instance.OnConsentInfoUpdateSucceeded?.Invoke(_instance, EventArgs.Empty));
        }

        [MonoPInvokeCallback(typeof(ConsentFormLoadFailedCallback))]
        private static void ConsentFormLoadFailed(int cause)
        {
            var args = new ConsentFormLoadFailedEventArgs(IosCmpHelper.GetConsentManagerErrorFromInt(cause));
            SyncContextHelper.Post(state => _instance.OnConsentFormLoadFailed?.Invoke(_instance, args));
        }

        [MonoPInvokeCallback(typeof(ConsentFormLoadSucceededCallback))]
        private static void ConsentFormLoadSucceeded(IntPtr iosConsentFormPtr)
        {
            var consentForm = ConsentForm.CreateInstance(new IosConsentForm(iosConsentFormPtr, ConsentFormDismissed));
            var args = new ConsentFormLoadSucceededEventArgs(consentForm);
            SyncContextHelper.Post(state => _instance.OnConsentFormLoadSucceeded?.Invoke(_instance, args));
        }

        [MonoPInvokeCallback(typeof(ConsentFormDismissedCallback))]
        private static void ConsentFormDismissed(int error)
        {
            var cause = error < 0 ? (ConsentManagerError?)null : IosCmpHelper.GetConsentManagerErrorFromInt(error);
            var args = new ConsentFormDismissedEventArgs(cause);
            SyncContextHelper.Post(state => _instance.OnConsentFormDismissed?.Invoke(_instance, args));
        }
    }
}
#endif
