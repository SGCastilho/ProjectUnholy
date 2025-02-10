using System.IO;
using UnityEditor;
using UnityEngine;

public class DeleteConfigFile
{
    [MenuItem("Tools/SGC/Delete Game Config")]
    public static void DeleteGameConfig()
    {
        if(File.Exists(Application.persistentDataPath + "/config.txt"))
        {
            File.Delete(Application.persistentDataPath + "/config.txt");

            Debug.Log("Game Config has been deleted");
        }
    }

    [MenuItem("Tools/SGC/Open Config Directory")]
    public static void OpenGameConfigDirectory()
    {
        System.Diagnostics.Process.Start("explorer.exe", Application.persistentDataPath);
    }
}
