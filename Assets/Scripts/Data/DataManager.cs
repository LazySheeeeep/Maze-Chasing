using UnityEngine;
using System.IO;
///<summary>
///DataManager���ݹ�����������Ѷ���д���ļ��Լ���Json�ļ��ж�ȡ�����ɶ���
///</summary>
public static class DataManager<T> where T : class, new()
{
    /// <summary>
    /// ��Json�ļ��ж�ȡ�����ɶ���
    /// </summary>
    /// <param name="jsonFileName">�ļ����ǵüӺ�׺</param>
    /// <returns>���ɵĶ���</returns>
    public static T LoadFromJsonFile(string jsonFileName)
    {
        using StreamReader sr = new(Path.Combine(Application.streamingAssetsPath, jsonFileName));
        T data = JsonUtility.FromJson<T>(sr.ReadToEnd());
        sr.Close();
        sr.Dispose();
        return data;
    }

    /// <summary>
    /// �Ѷ���д���ļ�
    /// </summary>
    /// <param name="obj">����</param>
    /// <param name="jsonFileName">�ļ����ǵüӺ�׺</param>
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
