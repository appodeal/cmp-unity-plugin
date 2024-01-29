#if UNITY_EDITOR

// ReSharper disable CheckNamespace

using AppodealStack.Cmp.Editor.Utilities;

namespace AppodealStack.Cmp
{
    internal static class Constants
    {
        public const string ConsentValueKey = "ApdEditorConsent";

        public const string ConsentFormPrefabName = "ApdEditorConsentForm";
        public const string ConsentFormPrefabFilePath = EditorConstants.PackageRootDirectory + "/Runtime/Editor/ConsentForm/" + ConsentFormPrefabName + ".prefab";
    }
}
#endif
