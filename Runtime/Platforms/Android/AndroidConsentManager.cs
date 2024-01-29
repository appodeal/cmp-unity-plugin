#if UNITY_ANDROID || APPODEAL_DEV_ANDROID

// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AndroidConsentManager :
        IConsentManager,
        IAndroidConsentFormDismissedListener,
        IAndroidConsentFormLoadFailedListener,
        IAndroidConsentFormLoadSucceededListener,
        IAndroidConsentInfoUpdateListener
    {
        private readonly AndroidJavaObject _activityJavaObject;
        private readonly AndroidJavaClass _consentManagerJavaClass;

        private readonly AndroidConsentInfoUpdateListener _consentInfoUpdateListener;
        private readonly AndroidConsentFormLoadFailedListener _consentFormLoadFailedListener;
        private readonly AndroidConsentFormLoadSucceededListener _consentFormLoadSucceededListener;
        private readonly AndroidConsentFormDismissedListener _consentFormDismissedListener;

        public event EventHandler<ConsentInfoUpdateFailedEventArgs> OnConsentInfoUpdateFailed;
        public event EventHandler OnConsentInfoUpdateSucceeded;
        public event EventHandler<ConsentFormLoadFailedEventArgs> OnConsentFormLoadFailed;
        public event EventHandler<ConsentFormLoadSucceededEventArgs> OnConsentFormLoadSucceeded;
        public event EventHandler<ConsentFormDismissedEventArgs> OnConsentFormDismissed;

        public ConsentStatus ConsentStatus
        {
            get
            {
                var consentStatus = _consentManagerJavaClass?.CallStatic<AndroidJavaObject>("getStatus");
                return AndroidCmpJavaHelper.GetConsentStatus(consentStatus);
            }
        }

        internal AndroidConsentManager()
        {
            try
            {
                _activityJavaObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                _consentManagerJavaClass = new AndroidJavaClass("com.appodeal.consent.ConsentManager");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Appodeal CMP] Plugin operation is not possible due to incorrect integration: {e.Message}");
            }

            _consentInfoUpdateListener = new AndroidConsentInfoUpdateListener(this);
            _consentFormLoadFailedListener = new AndroidConsentFormLoadFailedListener(this);
            _consentFormLoadSucceededListener = new AndroidConsentFormLoadSucceededListener(this);
            _consentFormDismissedListener = new AndroidConsentFormDismissedListener(this);
        }

        public void Load()
        {
            _consentManagerJavaClass?.CallStatic(
                "load",
                _activityJavaObject,
                _consentFormLoadSucceededListener,
                _consentFormLoadFailedListener
            );
        }

        public void LoadAndShowConsentFormIfRequired()
        {
            _consentManagerJavaClass?.CallStatic(
                "loadAndShowConsentFormIfRequired",
                _activityJavaObject,
                _consentFormDismissedListener
            );
        }

        public void RequestConsentInfoUpdate(ConsentInfoParameters consentInfoParameters)
        {
            var paramsJavaObject = AndroidCmpJavaHelper.GetConsentInfoParametersJavaObject(consentInfoParameters);
            if (paramsJavaObject == null) return;

            _consentManagerJavaClass?.CallStatic(
                "requestConsentInfoUpdate",
                paramsJavaObject,
                _consentInfoUpdateListener
            );
        }

        public void Revoke()
        {
            _consentManagerJavaClass?.CallStatic("revoke", _activityJavaObject);
        }

        #region Callbacks

        public void onConsentFormDismissed(AndroidJavaObject error)
        {
            var args = new ConsentFormDismissedEventArgs(
                error == null
                ? (ConsentManagerError?) null
                : AndroidCmpJavaHelper.GetConsentManagerError(error)
            );

            OnConsentFormDismissed?.Invoke(this, args);
        }

        public void onConsentFormLoadFailure(AndroidJavaObject cause)
        {
            var args = new ConsentFormLoadFailedEventArgs(AndroidCmpJavaHelper.GetConsentManagerError(cause));
            OnConsentFormLoadFailed?.Invoke(this, args);
        }

        public void onConsentFormLoadSuccess(AndroidJavaObject consentFormJavaObject)
        {
            var consentFormImpl = new AndroidConsentForm(_activityJavaObject, consentFormJavaObject, _consentFormDismissedListener);
            var consentForm = ConsentForm.CreateInstance(consentFormImpl);
            var args = new ConsentFormLoadSucceededEventArgs(consentForm);
            OnConsentFormLoadSucceeded?.Invoke(this, args);
        }

        public void onFailed(AndroidJavaObject cause)
        {
            var args = new ConsentInfoUpdateFailedEventArgs(AndroidCmpJavaHelper.GetConsentManagerError(cause));
            OnConsentInfoUpdateFailed?.Invoke(this, args);
        }

        public void onUpdated()
        {
            OnConsentInfoUpdateSucceeded?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
#endif
