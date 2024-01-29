#if UNITY_EDITOR

// ReSharper disable CheckNamespace

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AppodealStack.Cmp
{
    internal class EditorConsentManager : IConsentManager
    {
        public event EventHandler<ConsentInfoUpdateFailedEventArgs> OnConsentInfoUpdateFailed { add { } remove { } }
        public event EventHandler OnConsentInfoUpdateSucceeded;
        public event EventHandler<ConsentFormLoadFailedEventArgs> OnConsentFormLoadFailed;
        public event EventHandler<ConsentFormLoadSucceededEventArgs> OnConsentFormLoadSucceeded;
        public event EventHandler<ConsentFormDismissedEventArgs> OnConsentFormDismissed;

        private bool _isConsentInfoRequested;

        public ConsentStatus ConsentStatus
        {
            get
            {
                if (!_isConsentInfoRequested) return ConsentStatus.Unknown;
                return String.IsNullOrEmpty(PlayerPrefs.GetString(Constants.ConsentValueKey)) ? ConsentStatus.Required : ConsentStatus.Obtained;
            }
        }

        public void Load()
        {
            if (!_isConsentInfoRequested) return;

            var consentFormGameObject = GetConsentFormGameObject();
            if (consentFormGameObject == null)
            {
                OnConsentFormLoadFailed?.Invoke(this, new ConsentFormLoadFailedEventArgs(ConsentManagerError.InternalError));
                return;
            }

            var editorConsentForm = new EditorConsentForm(consentFormGameObject);
            editorConsentForm.OnConsentFormDismissed += (sender, args) =>
            {
                OnConsentFormDismissed?.Invoke(this, args);
            };

            var consentForm = ConsentForm.CreateInstance(editorConsentForm);
            OnConsentFormLoadSucceeded?.Invoke(this, new ConsentFormLoadSucceededEventArgs(consentForm));
        }

        public void LoadAndShowConsentFormIfRequired()
        {
            if (!_isConsentInfoRequested) return;

            if (!String.IsNullOrEmpty(PlayerPrefs.GetString(Constants.ConsentValueKey)))
            {
                OnConsentFormDismissed?.Invoke(this, new ConsentFormDismissedEventArgs(ConsentManagerError.FormPresentationIsNotRequired));
                return;
            }

            var consentFormGameObject = GetConsentFormGameObject();
            if (consentFormGameObject == null)
            {
                OnConsentFormDismissed?.Invoke(this, new ConsentFormDismissedEventArgs(ConsentManagerError.InternalError));
                return;
            }

            var editorConsentForm = new EditorConsentForm(consentFormGameObject);
            editorConsentForm.OnConsentFormDismissed += (sender, args) =>
            {
                OnConsentFormDismissed?.Invoke(this, args);
            };

            editorConsentForm.Show();
        }

        public void RequestConsentInfoUpdate(ConsentInfoParameters consentInfoParameters)
        {
            _isConsentInfoRequested = true;
            OnConsentInfoUpdateSucceeded?.Invoke(this, EventArgs.Empty);
        }

        public void Revoke()
        {
            PlayerPrefs.DeleteKey(Constants.ConsentValueKey);
            _isConsentInfoRequested = false;
        }

        private static GameObject GetConsentFormGameObject()
        {
            string[] assetGuids = AssetDatabase.FindAssets($"{Constants.ConsentFormPrefabName} t:prefab");
            string prefabPath = assetGuids.Length < 1 ? Constants.ConsentFormPrefabFilePath : AssetDatabase.GUIDToAssetPath(assetGuids[0]);

            var consentFormPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            var consentFormGameObject = UnityEngine.Object.Instantiate(consentFormPrefab, Vector3.zero, Quaternion.identity);

            if (consentFormGameObject == null) return null;

            consentFormGameObject.name = Constants.ConsentFormPrefabName;
            consentFormGameObject.SetActive(false);

            if (UnityEngine.Object.FindObjectsOfType<EventSystem>().Length < 1)
            {
                consentFormGameObject.transform.Find("EventSystem")?.gameObject.SetActive(true);
            }

            UnityEngine.Object.DontDestroyOnLoad(consentFormGameObject);

            return consentFormGameObject;
        }
    }
}
#endif
