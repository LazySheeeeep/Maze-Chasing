using UnityEngine;
using System.IO;
///<summary>
///DataManager数据管理器，负责把对象写入文件以及从Json文件中读取并生成对象
///</summary>
public static class DataManager<T> where T : class, new()
{
    /// <summary>
    /// 从Json文件中读取并生成对象
    /// </summary>
    /// <param name="jsonFileName">文件名记得加后缀</param>
    /// <returns>生成的对象</returns>
    public static T LoadFromJsonFile(string jsonFileName)
    {
        using StreamReader sr = new(Path.Combine(Application.streamingAssetsPath, jsonFileName));
        T data = JsonUtility.FromJson<T>(sr.ReadToEnd());
        sr.Close();
        sr.Dispose();
        return data;
    }

    /// <summary>
    /// 把对象写入文件
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="jsonFileName">文件名记得加后缀</param>
    /// <param name="prettyPrint"></param>
    public static void WriteToJsonFile(T obj, string jsonFileName, bool prettyPrint = false)
    {
        string json = JsonUtility.ToJson(obj, prettyPrint);
        using StreamWriter sw = new(Path.Combine(Application.streamingAssetsPath, jsonFileName));
        sw.WriteLine(json);
        sw.Close();
        sw.Dispose();
    }
}
