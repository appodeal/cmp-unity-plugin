//
//  CmpConsentManager.m
//  CMP Unity Plugin
//
//  Created by Dmitrii Feshchenko on 28/01/2024.
//

#import <UnityAppController.h>
#import <CmpUnityPluginTypes.h>
#import <StackConsentManager/StackConsentManager-Swift.h>

int CMPUnityPluginConsentManagerGetConsentStatus() {
    return (int)[APDConsentManager.shared status];
}

int CMPUnityPluginConsentManagerGetPrivacyOptionsStatus() {
    switch ([APDConsentManager.shared privacyOptionsRequirementStatus]) {
        case APDPrivacyOptionsStatusRequired:    return 1;
        case APDPrivacyOptionsStatusNotRequired: return 2;
        default:                                 return 0;
    }
}

void CMPUnityPluginConsentManagerLoad(ConsentFormLoadFailedCallback loadFailedCallback,
                                      ConsentFormLoadSucceededCallback loadSucceededCallback) {
    [APDConsentManager.shared loadWithCompletion:^(APDConsentDialog* dialog, NSError* error) {
        if (error) {
            if (loadFailedCallback) loadFailedCallback((int)error.code);
            return;
        }

        if (dialog) {
            if (loadSucceededCallback) loadSucceededCallback((__bridge_retained CFCMPUnityPluginConsentFormRef)dialog);
        }
    }];
}

void CMPUnityPluginConsentManagerLoadAndShowConsentFormIfRequired(ConsentFormDismissedCallback callback) {
    [APDConsentManager.shared loadAndPresentIfNeededWithRootViewController:[GetAppController() rootViewController] completion:^(NSError* error) {
        if (error) {
            if (callback) callback((int)error.code);
            return;
        }

        if (callback) callback(-1);
    }];
}

void CMPUnityPluginConsentManagerSetDebugSettings(int geography, const char* testDeviceIds) {
    if (geography < 0) {
        APDConsentManager.shared.debugSettings = nil;
        return;
    }

    APDConsentDebugGeography debugGeography;
    switch (geography) {
        case 0:  debugGeography = APDConsentDebugGeographyEEA; break;
        case 1:  debugGeography = APDConsentDebugGeographyRegulatedUSState; break;
        case 2:
        default: debugGeography = APDConsentDebugGeographyOther; break;
    }

    NSString* joinedTestDeviceIds = testDeviceIds ? [NSString stringWithUTF8String:testDeviceIds] : @"";
    NSArray<NSString*>* ids = joinedTestDeviceIds.length > 0 ? [joinedTestDeviceIds componentsSeparatedByString:@","] : @[];
    APDConsentManager.shared.debugSettings = [[APDConsentDebugSettings alloc] initWithGeography:debugGeography testDeviceIds:ids];
}

void CMPUnityPluginConsentManagerRequestConsentInfoUpdate(CMPUnityPluginConsentInfoParameters consentInfoParameters,
                                                          ConsentInfoUpdateFailedCallback updateFailedCallback,
                                                          ConsentInfoUpdateSucceededCallback updateSucceededCallback) {
    NSString* appKey = [NSString stringWithUTF8String:consentInfoParameters.AppKey];
    NSString* sdk = [NSString stringWithUTF8String:consentInfoParameters.Sdk];
    NSString* sdkVersion = [NSString stringWithUTF8String:consentInfoParameters.SdkVersion];
    APDConsentUpdateRequestParameters* parameters = [[APDConsentUpdateRequestParameters alloc] initWithAppKey:appKey
                                                                                               mediationSdkName:sdk
                                                                                               mediationSdkVersion:sdkVersion
                                                                                               COPPA:consentInfoParameters.IsUnderAgeToConsent];

    [APDConsentManager.shared requestConsentInfoUpdateWithParameters:parameters completion:^(NSError* error) {
        if (error) {
            if (updateFailedCallback) updateFailedCallback((int)error.code);
            return;
        }

        if (updateSucceededCallback) updateSucceededCallback();
    }];
}

void CMPUnityPluginConsentManagerShowPrivacyOptionsForm(ConsentFormDismissedCallback callback) {
    [APDConsentManager.shared showPrivacyOptionsFormWithRootViewController:[GetAppController() rootViewController] completion:^(NSError* error) {
        if (error) {
            if (callback) callback((int)error.code);
            return;
        }

        if (callback) callback(-1);
    }];
}

void CMPUnityPluginConsentManagerRevoke() {
    [APDConsentManager.shared revoke];
}
