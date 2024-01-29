#if UNITY_ANDROID || APPODEAL_DEV_ANDROID

// ReSharper disable CheckNamespace

using UnityEngine;

namespace AppodealStack.Cmp
{
    internal class AndroidConsentForm : IConsentForm
    {
        private readonly AndroidJavaObject _activityJavaObject;
        private readonly AndroidJavaObject _consentFormJavaObject;
        private readonly AndroidConsentFormDismissedListener _listener;

        internal AndroidConsentForm(AndroidJavaObject activity, AndroidJavaObject consentForm, AndroidConsentFormDismissedListener listener)
        {
            _activityJavaObject = activity;
            _consentFormJavaObject = consentForm;
            _listener = listener;
        }

        public void Show()
        {
            _consentFormJavaObject?.Call("show", _activityJavaObject, _listener);
        }
    }
}
#endif
