// ReSharper Disable CheckNamespace

using UnityEditor;
using UnityEngine;
using AppodealStack.Cmp.Editor.Utilities;
using AppodealStack.Cmp.Editor.PluginRemover;

namespace AppodealStack.Cmp.Editor.TopBarMenu
{
    internal class CmpTopBarMenu : ScriptableObject
    {
        [MenuItem("Appodeal/CMP/Documentation")]
        public static void OpenDocumentation()
        {
            Application.OpenURL(EditorConstants.PluginDocumentationLink);
        }

        [MenuItem("Appodeal/CMP/Remove Plugin")]
        public static void RemovePlugin()
        {
            RemoveHelper.RemovePlugin();
        }
    }
}
