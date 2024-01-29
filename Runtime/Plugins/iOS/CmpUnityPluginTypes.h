//
//  CmpUnityPluginTypes.h
//  CMP Unity Plugin
//
//  Created by Dmitrii Feshchenko on 28/01/2024.
//

typedef struct {
    const char* AppKey;
    bool        IsUnderAgeToConsent;
    const char* Sdk;
    const char* SdkVersion;
} CMPUnityPluginConsentInfoParameters;

typedef const void* CFCMPUnityPluginConsentFormRef;

typedef void (*ConsentInfoUpdateFailedCallback)(int cause);
typedef void (*ConsentInfoUpdateSucceededCallback)();
typedef void (*ConsentFormLoadFailedCallback)(int cause);
typedef void (*ConsentFormLoadSucceededCallback)(CFCMPUnityPluginConsentFormRef consentFormPtr);
typedef void (*ConsentFormDismissedCallback)(int error);
