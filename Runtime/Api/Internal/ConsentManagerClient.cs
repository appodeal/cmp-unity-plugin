// ReSharper disable CheckNamespace

using System;

namespace AppodealStack.Cmp
{
    internal class ConsentManagerClient : IConsentManager
    {
        private readonly IConsentManager _consentManagerImpl;

        public event EventHandler<ConsentInfoUpdateFailedEventArgs> OnConsentInfoUpdateFailed;
        public event EventHandler OnConsentInfoUpdateSucceeded;
        public event EventHandler<ConsentFormLoadFailedEventArgs> OnConsentFormLoadFailed;
        public event EventHandler<ConsentFormLoadSucceededEventArgs> OnConsentFormLoadSucceeded;
        public event EventHandler<ConsentFormDismissedEventArgs> OnConsentFormDismissed;

        public ConsentStatus ConsentStatus { get => _consentManagerImpl?.ConsentStatus ?? ConsentStatus.Unknown; }

        internal ConsentManagerClient()
        {
#if UNITY_EDITOR
            _consentManagerImpl = new EditorConsentManager();
#elif UNITY_ANDROID
            _consentManagerImpl = new AndroidConsentManager();
#elif UNITY_IOS
            _consentManagerImpl = new IosConsentManager();
#else
            _consentManagerImpl = new DummyConsentManager();
#endif

            _consentManagerImpl.OnConsentInfoUpdateFailed += (sender, args) => OnConsentInfoUpdateFailed?.Invoke(this, args);
            _consentManagerImpl.OnConsentInfoUpdateSucceeded += (sender, args) => OnConsentInfoUpdateSucceeded?.Invoke(this, args);
            _consentManagerImpl.OnConsentFormLoadFailed += (sender, args) => OnConsentFormLoadFailed?.Invoke(this, args);
            _consentManagerImpl.OnConsentFormLoadSucceeded += (sender, args) => OnConsentFormLoadSucceeded?.Invoke(this, args);
            _consentManagerImpl.OnConsentFormDismissed += (sender, args) => OnConsentFormDismissed?.Invoke(this, args);
        }

        public void Load() => _consentManagerImpl?.Load();

        public void LoadAndShowConsentFormIfRequired() => _consentManagerImpl?.LoadAndShowConsentFormIfRequired();

        public void RequestConsentInfoUpdate(ConsentInfoParameters consentInfoParameters) => _consentManagerImpl?.RequestConsentInfoUpdate(consentInfoParameters);

        public void Revoke() => _consentManagerImpl?.Revoke();
    }
}
