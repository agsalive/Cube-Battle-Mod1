#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class AssetBundleCreator  {
   
    [MenuItem("Assets/Create AssetBundle")]
    static void Create() {
//        var prevSerializationMode = EditorSettings.serializationMode;
//        EditorSettings.serializationMode = SerializationMode.ForceText;
        var build = new AssetBundleBuild();

        var dir = AssetDatabase.GetAssetPath(Selection.activeObject);

//        var scriptTextNames = new List<string>();
//        //create temp script text
//        foreach (var assetName in GetAllAssetNames(dir).Where(x=>Path.GetExtension(x) == ".cs"))
//        {
//            var newName = assetName+".txt";
//            AssetDatabase.CopyAsset(assetName,newName);
//            AssetDatabase.ImportAsset(newName);
//            scriptTextNames.Add(newName);
//        }

        AssetDatabase.Refresh();


        build.assetBundleName = Path.GetFileName(dir)+".assetbundle";
        build.assetNames = GetAllAssetNames(dir).ToArray();

        var buildTarget = EditorUserBuildSettings.activeBuildTarget ;
        var tempOutputFolderPath = "Temp/AssetBundleOutput";
        if(!Directory.Exists(tempOutputFolderPath))
            Directory.CreateDirectory(tempOutputFolderPath);
        
        BuildPipeline.BuildAssetBundles(tempOutputFolderPath, new AssetBundleBuild[]{build},BuildAssetBundleOptions.None,buildTarget);

//        //clear temp script text
//        foreach (var assetName in scriptTextNames)
//        {
//            AssetDatabase.DeleteAsset(assetName);
//        }
        FileUtil.ReplaceFile(tempOutputFolderPath+"/"+build.assetBundleName, dir+"/"+build.assetBundleName);


        AssetDatabase.Refresh();
    }

    static IEnumerable<string> GetAllAssetNames(string folderPath){
        return Directory.GetFiles(folderPath,"*.*",SearchOption.AllDirectories)
            .Select(x=>x.Replace("\\","/")
            .Replace(Application.dataPath+"/","Assets/"))
            .Where(x=>AssetDatabase.LoadAssetAtPath<Object>(x)!=null)
            .Where(x=>Path.GetExtension(x) != ".assetbundle");
    }

    [MenuItem("Assets/Create AssetBundle",true)]
    static bool CanCreate(){
        return Directory.Exists( AssetDatabase.GetAssetPath(Selection.activeObject));
    }
}
#endif