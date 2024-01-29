#if UNITY_ANDROID || APPODEAL_DEV_ANDROID

// ReSharper disable CheckNamespace

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AppodealStack.Cmp
{
    internal static class AndroidCmpJavaHelper
    {
        private static readonly AndroidJavaObject ActivityJavaObject;

        private static readonly Dictionary<AndroidJavaClass, ConsentManagerError> ConsentManagerErrors;

        static AndroidCmpJavaHelper()
        {
            try
            {
                ActivityJavaObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

                ConsentManagerErrors = new Dictionary<AndroidJavaClass, ConsentManagerError>
                {
                    { new AndroidJavaClass("com.appodeal.consent.ConsentManagerError$ActivityIsDestroyedError"), ConsentManagerError.ActivityIsDestroyed },
                    { new AndroidJavaClass("com.appodeal.consent.ConsentManagerError$FormAlreadyShown"), ConsentManagerError.FormIsAlreadyBeingPresented },
                    { new AndroidJavaClass("com.appodeal.consent.ConsentManagerError$FormCacheError"), ConsentManagerError.FormCachingFailed },
                    { new AndroidJavaClass("com.appodeal.consent.ConsentManagerError$FormNotReadyError"), ConsentManagerError.FormIsNotReady },
                    { new AndroidJavaClass("com.appodeal.consent.ConsentManagerError$FormPresentationNotRequired"), ConsentManagerError.FormPresentationIsNotRequired },
                    { new AndroidJavaClass("com.appodeal.consent.ConsentManagerError$InternalError"), ConsentManagerError.InternalError },
                    { new AndroidJavaClass("com.appodeal.consent.ConsentManagerError$RequestError"), ConsentManagerError.RequestError },
                    { new AndroidJavaClass("com.appodeal.consent.ConsentManagerError$ServerError"), ConsentManagerError.ServerError },
                    { new AndroidJavaClass("com.appodeal.consent.ConsentManagerError$TimeoutError"), ConsentManagerError.TimeoutError }
                };
            }
            catch (Exception e)
            {
                Debug.LogError($"[Appodeal CMP] Plugin operation is not possible due to incorrect integration: {e.Message}");
            }
        }

        public static ConsentManagerError GetConsentManagerError(AndroidJavaObject cause)
        {
            if (cause == null)
            {
                Debug.LogError("[Appodeal CMP] ConsentManagerError java object can not be null");
                return ConsentManagerError.Unspecified;
            }

            var res = ConsentManagerErrors.FirstOrDefault(error => AndroidJNI.IsInstanceOf(cause.GetRawObject(), error.Key.GetRawClass()));
            return res.Equals(default(KeyValuePair<AndroidJavaClass, ConsentManagerError>)) ? ConsentManagerError.Unspecified : res.Value;
        }

        public static ConsentStatus GetConsentStatus(AndroidJavaObject status)
        {
            if (status == null)
            {
                Debug.LogError("[Appodeal CMP] ConsentStatus java object can not be null");
                return ConsentStatus.Unknown;
            }

            string javaStatus = status.Call<string>("getStatusName");

            return javaStatus switch
            {
                "UNKNOWN" => ConsentStatus.Unknown,
                "REQUIRED" => ConsentStatus.Required,
                "NOT_REQUIRED" => ConsentStatus.NotRequired,
                "OBTAINED" => ConsentStatus.Obtained,
                _ => throw new ArgumentOutOfRangeException(nameof(javaStatus), javaStatus, "value must be assignable to ConsentStatus")
            };
        }

        public static AndroidJavaObject GetConsentInfoParametersJavaObject(ConsentInfoParameters parameters)
        {
            if (parameters == null || String.IsNullOrEmpty(parameters.AppKey))
            {
                Debug.LogError("[Appodeal CMP] Invalid AppKey passed to the ConsentManager");
                return null;
            }

            return new AndroidJavaObject(
                "com.appodeal.consent.ConsentUpdateRequestParameters",
                ActivityJavaObject,
                GetJavaObject(parameters.AppKey),
                GetJavaObject(parameters.IsUnderAgeToConsent),
                GetJavaObject(parameters.Sdk),
                GetJavaObject(parameters.SdkVersion)
            );
        }

        private static object GetJavaObject(object value)
        {
            return value switch
            {
                bool _ => new AndroidJavaObject("java.lang.Boolean", value),
                char _ => new AndroidJavaObject("java.lang.Character", value),
                int _ => new AndroidJavaObject("java.lang.Integer", value),
                long _ => new AndroidJavaObject("java.lang.Long", value),
                float _ => new AndroidJavaObject("java.lang.Float", value),
                double _ => new AndroidJavaObject("java.lang.Double", value),
                string _ => value,
                _ => throw new ArgumentException("Not supported type was passed as an argument")
            };
        }
    }
}
#endif
