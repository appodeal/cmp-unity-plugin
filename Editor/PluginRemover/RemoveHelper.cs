// ReSharper Disable CheckNamespace

using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using AppodealStack.Cmp.Editor.Utilities;

namespace AppodealStack.Cmp.Editor.PluginRemover
{
    internal static class RemoveHelper
    {
        private static bool IsFolderEmpty(string path)
        {
            if (!Directory.Exists(path)) return false;
            var filesPaths = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            var s = new List<string>(filesPaths);
            for (var i = 0; i < s.Count; i++)
            {
                if (s[i].Contains(".DS_Store"))
                {
                    s.RemoveAt(i);
                }
            }

            return s.Count == 0;
        }

        private static IEnumerable<ItemToRemove> ReadXML()
        {
            var itemToRemoveList = new List<ItemToRemove>();
            var xDoc = new XmlDocument();
            xDoc.Load(EditorConstants.PackageRemoveListFilePath);
            var xRoot = xDoc.DocumentElement;

            if (xRoot == null) return itemToRemoveList.ToArray();
            foreach (XmlNode xNode in xRoot)
            {
                var itemToRemove = new ItemToRemove();
                foreach (XmlNode childNode in xNode.ChildNodes)
                {
                    if (childNode.Name.Equals("name"))
                    {
                        itemToRemove.name = childNode.InnerText;
                    }

                    if (childNode.Name.Equals("is_confirmation_required"))
                    {
                        if (childNode.InnerText.Equals("true"))
                        {
                            itemToRemove.isConfirmationRequired = true;
                        }
                        else if (childNode.InnerText.Equals("true"))
                        {
                            itemToRemove.isConfirmationRequired = false;
                        }
                    }

                    if (childNode.Name.Equals("path"))
                    {
                        itemToRemove.path = childNode.InnerText;
                    }

                    if (childNode.Name.Equals("description"))
                    {
                        itemToRemove.description = childNode.InnerText;
                    }

                    if (childNode.Name.Equals("check_if_empty"))
                    {
                        if (childNode.InnerText.Equals("true"))
                        {
                            itemToRemove.checkIfEmpty = true;
                        }
                        else if (childNode.InnerText.Equals("false"))
                        {
                            itemToRemove.checkIfEmpty = false;
                        }
                    }

                    if (childNode.Name.Equals("perform_only_if_total_remove"))
                    {
                        if (childNode.InnerText.Equals("true"))
                        {
                            itemToRemove.performOnlyIfTotalRemove = true;
                        }
                        else if (childNode.InnerText.Equals("false"))
                        {
                            itemToRemove.performOnlyIfTotalRemove = false;
                        }
                    }

                    if (childNode.Name.Equals("filter"))
                    {
                        itemToRemove.filter = childNode.InnerText;
                    }
                }

                itemToRemoveList.Add(itemToRemove);
            }

            return itemToRemoveList.ToArray();
        }

        public static void RemovePlugin(bool isCleanBeforeUpdate = false)
        {
            if (!EditorUtility.DisplayDialog("Remove Appodeal CMP Plugin",
                    "Are you sure you want to remove the Appodeal CMP plugin from your project?",
                    "Yes",
                    "Cancel")) return;

            var items = ReadXML();
            foreach (var t in items)
            {
                if (t.performOnlyIfTotalRemove && isCleanBeforeUpdate) continue;
                var confirmed = !t.isConfirmationRequired || isCleanBeforeUpdate;
                var fullItemPath = Path.Combine(Application.dataPath, t.path);

                if (!confirmed)
                {
                    if (EditorUtility.DisplayDialog("Removing " + t.name, t.description, "Yes", "No"))
                    {
                        confirmed = true;
                    }
                }

                if (!confirmed) continue;
                var isChecked = !t.checkIfEmpty;
                if (!isChecked) isChecked = IsFolderEmpty(fullItemPath);
                if (!isChecked) continue;

                if (string.IsNullOrEmpty(t.filter))
                {
                    FileUtil.DeleteFileOrDirectory(fullItemPath);
                    FileUtil.DeleteFileOrDirectory(fullItemPath + ".meta");
                    continue;
                }

                var isDirectoryExists = Directory.Exists(fullItemPath);
                if (!isDirectoryExists) continue;
                var filesList =
                    new List<string>(Directory.GetFiles(fullItemPath, "*", SearchOption.TopDirectoryOnly));
                filesList.AddRange(Directory.GetDirectories(fullItemPath, "*", SearchOption.TopDirectoryOnly));
                foreach (var t1 in from t1 in filesList
                         let fileName = Path.GetFileName(t1)
                         where Regex.IsMatch(fileName, t.filter, RegexOptions.IgnoreCase)
                         select t1)
                {
                    FileUtil.DeleteFileOrDirectory(t1);
                    FileUtil.DeleteFileOrDirectory(t1 + ".meta");
                }

                if (!IsFolderEmpty(fullItemPath)) continue;
                FileUtil.DeleteFileOrDirectory(fullItemPath);
                FileUtil.DeleteFileOrDirectory(fullItemPath + ".meta");
            }

            Client.Remove(EditorConstants.PackageName);
        }
    }
}
