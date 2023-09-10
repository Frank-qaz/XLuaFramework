using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        //�����ļ����������ļ�
        string[] files = Directory.GetFiles(PathUtil.BuildresourcesPath, "*",SearchOption.AllDirectories);
        for (int i = 0; i < files.Length ; i++)
        {
            //��Meta�ļ�����
            if (files[i].EndsWith(".meta"))
                continue;
           
            AssetBundleBuild assetBuild = new AssetBundleBuild();

            string fileName = PathUtil.GetStandardPath(files[i]);
            Debug.Log("file:" + fileName);
             
            string assetName = PathUtil.GetUnityPath(fileName);

            //����AssetBundleBuild����Դ·��
            assetBuild.assetNames =  new string[] { assetName };

            //����AssetBundleBuild������ 
            string bundleName = fileName.Replace(PathUtil.BuildresourcesPath,"").ToLower();
            assetBuild.assetBundleName = bundleName + ".ab";

            assetBundleBuilds.Add(assetBuild);
        }
        if (Directory.Exists(PathUtil.BundleOutPath))
            Directory.Delete(PathUtil.BundleOutPath, true);
        Directory.CreateDirectory(PathUtil.BundleOutPath);

        //��Bundle��
        BuildPipeline.BuildAssetBundles(PathUtil.BundleOutPath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);
    }
}
