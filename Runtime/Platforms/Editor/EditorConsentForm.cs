#if UNITY_EDITOR

// ReSharper disable CheckNamespace

using System;
using UnityEngine;

namespace AppodealStack.Cmp
{
    internal class EditorConsentForm : IConsentForm
    {
        private GameObject _consentForm;

        public event EventHandler<ConsentFormDismissedEventArgs> OnConsentFormDismissed;

        internal EditorConsentForm(GameObject consentForm)
        {
            _consentForm = consentForm;

            _consentForm.GetComponent<ApdEditorConsentFormVisual>().OnConsentButtonClicked += (sender, args) =>
            {
                PlayerPrefs.SetString(Constants.ConsentValueKey, "GIVEN");
                OnConsentFormDismissed?.Invoke(this, new ConsentFormDismissedEventArgs());
                UnityEngine.Object.Destroy(_consentForm);
                _consentForm = null;
            };

            _consentForm.GetComponent<ApdEditorConsentFormVisual>().OnDoNotConsentButtonClicked += (sender, args) =>
            {
                PlayerPrefs.SetString(Constants.ConsentValueKey, "NOT_GIVEN");
                OnConsentFormDismissed?.Invoke(this, new ConsentFormDismissedEventArgs());
                UnityEngine.Object.Destroy(_consentForm);
                _consentForm = null;
            };
        }

        public void Show()
        {
            if (_consentForm == null)
            {
                OnConsentFormDismissed?.Invoke(this, new ConsentFormDismissedEventArgs(ConsentManagerError.FormIsNotReady));
                return;
            }

            if (_consentForm.activeSelf)
            {
                OnConsentFormDismissed?.Invoke(this, new ConsentFormDismissedEventArgs(ConsentManagerError.FormIsAlreadyBeingPresented));
                return;
            }

            _consentForm.SetActive(true);
        }
    }
}
#endif
