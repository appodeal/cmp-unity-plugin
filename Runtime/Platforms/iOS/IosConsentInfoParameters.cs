#if UNITY_IOS || APPODEAL_DEV_IOS

// ReSharper disable CheckNamespace

using System.Runtime.InteropServices;

namespace AppodealStack.Cmp
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct IosConsentInfoParameters
    {
        public string AppKey;
        public bool IsUnderAgeToConsent;
        public string Sdk;
        public string SdkVersion;

        public static IosConsentInfoParameters FromDtoClass(ConsentInfoParameters parameters)
        {
            return new IosConsentInfoParameters
            {
                AppKey = parameters.AppKey,
                IsUnderAgeToConsent = parameters.IsUnderAgeToConsent,
                Sdk = parameters.Sdk,
                SdkVersion = parameters.SdkVersion,
            };
        }
    }
}
#endif
