using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildTool : Editor
{
    [MenuItem("Tools/Buid Windows Bundle")]
    static void BundleWindowsBuild()
    {
        Build(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Tools/Buid Andriod Bundle")]
    static void BundleAndroidBuild()
    {
        Build(BuildTarget.Android);
    }

    [MenuItem("Tools/Buid iPhone Bundle")]
    static void BundleiPhoneBuild()
    {
        Build(BuildTarget.iOS);
    }

    static void Build(BuildTarget target)
    {
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();

        //文件信息列表
        List<string> bundleInfos = new List<string>();

        //查找文件夹下所有文件
        string[] files = Directory.GetFiles(PathUtil.BuildresourcesPath, "*",SearchOption.AllDirectories);
        for (int i = 0; i < files.Length ; i++)
        {
            //将Meta文件除外
            if (files[i].EndsWith(".meta"))
                continue;
           
            AssetBundleBuild assetBuild = new AssetBundleBuild();

            string fileName = PathUtil.GetStandardPath(files[i]);
            Debug.Log("file:" + fileName);
             
            string assetName = PathUtil.GetUnityPath(fileName);

            //设置AssetBundleBuild的资源路径
            assetBuild.assetNames =  new string[] { assetName };

            //设置AssetBundleBuild的名字 
            string bundleName = fileName.Replace(PathUtil.BuildresourcesPath,"").ToLower();
            assetBuild.assetBundleName = bundleName + ".ab";

            assetBundleBuilds.Add(assetBuild);

            //添加文件信息和依赖信息
            List<string> dependenceInfo = GetDependence(assetName);
            string bundleInfo = assetName + "|" + bundleName + ".ab";

            if (dependenceInfo.Count > 0)
                bundleInfo = bundleInfo + "|" + string.Join("|", dependenceInfo);

            bundleInfos.Add(bundleInfo);
        }

        if (Directory.Exists(PathUtil.BundleOutPath))
            Directory.Delete(PathUtil.BundleOutPath, true);
        Directory.CreateDirectory(PathUtil.BundleOutPath);

        //打Bundle包
        BuildPipeline.BuildAssetBundles(PathUtil.BundleOutPath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);
        File.WriteAllLines(PathUtil.BundleOutPath + "/" + AppConst.FileListName, bundleInfos);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取依赖文件列表
    /// </summary>
    /// <param name="curFile"></param>
    /// <returns></returns>
    static List<string> GetDependence(string curFile)
    {
        List<string> dependence = new List<string>();

        //获得依赖文件（就一行代码）
        string[] files = AssetDatabase.GetDependencies(curFile);
        dependence = files.Where(file => !file.EndsWith(".cs") && !file.Equals(curFile)).ToList();
        return dependence;
    }

}
