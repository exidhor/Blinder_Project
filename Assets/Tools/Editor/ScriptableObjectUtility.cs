using System.IO;
using UnityEditor;
using UnityEngine;

namespace ToolsEditor
{
    public static class ScriptableObjectUtility
    {
        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files.
        // source : http://wiki.unity3d.com/index.php?title=CreateScriptableObjectAsset
        /// </summary>
        public static T CreateAsset<T>(string path, string name) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + name + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            return asset;
        }
    }
}
