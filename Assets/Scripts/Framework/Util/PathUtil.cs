using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtil
{
    //��Ŀ¼
    public static readonly string AssetsPath = Application.dataPath;

    //��Ҫ��Bundle��Ŀ¼
    public static readonly string BuildresourcesPath = Application.dataPath + "/BuildResources/";

    //Bundle���Ŀ¼
    public static readonly string BundleOutPath = Application.streamingAssetsPath;

    //��ȡUnity�����·��
    public static string GetUnityPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return string.Empty;
        return path.Substring(path.IndexOf("Assets"));
    }

    //��ȡ��׼·��
    public static string GetStandardPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return string.Empty;
        return path.Trim().Replace("\\","/");
    }
}
