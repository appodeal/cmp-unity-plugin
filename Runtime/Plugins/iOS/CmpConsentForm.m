//
//  CmpConsentForm.m
//  CMP Unity Plugin
//
//  Created by Dmitrii Feshchenko on 28/01/2024.
//

#import <UnityAppController.h>
#import <CmpUnityPluginTypes.h>
#import <StackConsentManager/StackConsentManager-Swift.h>

void CMPUnityPluginConsentFormShow(CFCMPUnityPluginConsentFormRef ptr, ConsentFormDismissedCallback callback) {
    if (!ptr) return;
    [(__bridge APDConsentDialog*)ptr presentWithRootViewController:[GetAppController() rootViewController] completion:^(NSError* error) {
        if (error) {
            if (callback) callback((int)error.code);
            return;
        }

        if (callback) callback(-1);
    }];
}

void CMPUnityPluginConsentFormDispose(CFCMPUnityPluginConsentFormRef ptr) {
    if (!ptr) return;
    (void)(__bridge_transfer APDConsentDialog*)ptr;
}
