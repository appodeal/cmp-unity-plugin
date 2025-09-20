// ReSharper disable CheckNamespace

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AppodealStack.Cmp;
using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;

namespace AppodealSample
{
    public class AppodealCmpDemo : MonoBehaviour
    {
        [SerializeField] private Button             nextSceneButton;
        [SerializeField] private Text               pluginVersionText;

#if UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IOS
        private const string DefaultAppKey = "";
#elif UNITY_ANDROID
        private const string DefaultAppKey = "fee50c333ff3825fd6ad6d38cff78154de3025546d47a84f";
#elif UNITY_IOS
        private const string DefaultAppKey = "466de0d625e01e8811c588588a42a55970bc7c132649eede";
#else
        private const string DefaultAppKey = "";
#endif

        private static string AppKey => PlayerPrefs.GetString(AppodealConstants.AppKeyPlayerPrefsKey, DefaultAppKey);

        private ConsentForm _consentForm;

        private void Awake()
        {
            Assert.IsNotNull(nextSceneButton);
            Assert.IsNotNull(pluginVersionText);
        }

        private void Start()
        {
            nextSceneButton.interactable = GetSceneInfo().hasNext;
            pluginVersionText.text = $"Appodeal Unity Plugin v{AppodealVersions.GetPluginVersion()}";

            ConsentManager.Instance.OnConsentFormDismissed += (_, args) =>
            {
                string message = "[Appodeal CMP] OnConsentFormDismissed event triggered.";
                if (args.Error != null) message += $" Error: {args.Error}";
                Debug.Log(message);
                _consentForm = null;
            };

            ConsentManager.Instance.OnConsentFormLoadFailed += (_, args) =>
            {
                Debug.Log($"[Appodeal CMP] OnConsentFormLoadFailed event triggered. Cause: {args.Cause}");
            };

            ConsentManager.Instance.OnConsentFormLoadSucceeded += (_, args) =>
            {
                Debug.Log("[Appodeal CMP] OnConsentFormLoadSucceeded event triggered.");
                _consentForm = args.ConsentForm;
            };

            ConsentManager.Instance.OnConsentInfoUpdateFailed += (_, args) =>
            {
                Debug.Log($"[Appodeal CMP] OnConsentInfoUpdateFailed event triggered. Cause: {args.Cause}");
            };

            ConsentManager.Instance.OnConsentInfoUpdateSucceeded += (_, _) =>
            {
                Debug.Log("[Appodeal CMP] OnConsentInfoUpdateSucceeded event triggered.");
            };
        }

        public void ShowNextScene()
        {
            var sceneInfo = GetSceneInfo();

            if (sceneInfo.hasNext)
            {
                SceneManager.LoadScene(sceneInfo.nextIndex);
            }
        }

        public void RequestConsentInfoUpdate()
        {
            Debug.Log("[Appodeal CMP] RequestInfoUpdate() method called");

            var parameters = new ConsentInfoParameters
            {
                AppKey = AppKey,
                IsUnderAgeToConsent = false,
                Sdk = "Appodeal",
                SdkVersion = Appodeal.GetNativeSDKVersion()
            };

            Debug.Log($"[Appodeal CMP] init params: {parameters.ToJsonString()}");

            ConsentManager.Instance.RequestConsentInfoUpdate(parameters);
        }

        public void GetConsentStatus()
        {
            var status = ConsentManager.Instance.ConsentStatus;
            Debug.Log($"[Appodeal CMP] GetConsentStatus() method called, status: {status}");
        }

        public void LoadConsentForm()
        {
            Debug.Log("[Appodeal CMP] LoadConsentForm() method called");
            ConsentManager.Instance.Load();
        }

        public void ShowConsentForm()
        {
            Debug.Log("[Appodeal CMP] ShowConsentForm() method called");
            _consentForm?.Show();
        }

        public void LoadAndShowConsentForm()
        {
            Debug.Log("[Appodeal CMP] LoadAndShowConsentForm() method called");
            ConsentManager.Instance.LoadAndShowConsentFormIfRequired();
        }

        public void RevokeConsent()
        {
            Debug.Log("[Appodeal CMP] RevokeConsent() method called");
            ConsentManager.Instance.Revoke();
        }

        private static (int currentIndex, int nextIndex, bool hasNext) GetSceneInfo()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;
            bool hasNext = nextIndex < SceneManager.sceneCountInBuildSettings;
            return (currentIndex, nextIndex, hasNext);
        }
    }
}
