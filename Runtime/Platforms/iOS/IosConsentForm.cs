#if UNITY_IOS || APPODEAL_DEV_IOS

// ReSharper disable CheckNamespace

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace AppodealStack.Cmp
{
    internal class IosConsentForm : IConsentForm, IDisposable
    {
        private IntPtr _consentFormPtr;
        private ConsentFormDismissedCallback _dismissedCallback;

        private bool _disposed;

        internal IosConsentForm(IntPtr consentFormPtr, ConsentFormDismissedCallback dismissedCallback)
        {
            _consentFormPtr = consentFormPtr;
            _dismissedCallback = dismissedCallback;
        }

        ~IosConsentForm() => Dispose(false);

        public void Show()
        {
            if (IsDisposed()) return;
            CmpConsentFormShow(_consentFormPtr, _dismissedCallback);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _dismissedCallback = null;
            }

            if (_consentFormPtr != IntPtr.Zero)
            {
                CmpConsentFormDispose(_consentFormPtr);
                _consentFormPtr = IntPtr.Zero;
            }

            _disposed = true;
        }

        private bool IsDisposed()
        {
            if (!_disposed) return false;
            Debug.LogError($"[Appodeal CMP] {GetType().FullName} instance is disposed. Calling any methods on this instance is not allowed.");
            return true;
        }

        [DllImport("__Internal", EntryPoint = "CMPUnityPluginConsentFormShow")]
        private static extern void CmpConsentFormShow(IntPtr consentFormPtr, ConsentFormDismissedCallback dismissedCallback);

        [DllImport("__Internal", EntryPoint = "CMPUnityPluginConsentFormDispose")]
        private static extern void CmpConsentFormDispose(IntPtr consentFormPtr);
    }
}
#endif
