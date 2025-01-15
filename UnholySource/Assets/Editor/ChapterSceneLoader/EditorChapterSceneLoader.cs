using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class EditorChapterSceneLoader : EditorWindow
{
    private const string PATH_CHAPTER_ONE = "Assets/Scenes/Levels/Chapter 1/Rooms";

    public Scene[] openedScenes;

    public bool chapterLoaded;
    public string loadedChapterInfo;

    public string[] chapterOneScenes;

    [MenuItem("Tools/SGC/ChapterLoader")]
    static void OpenWindow()
    {
        //Essa função cria nossa instancia da janela
        EditorChapterSceneLoader window = (EditorChapterSceneLoader)GetWindow(typeof(EditorChapterSceneLoader));

        window.loadedChapterInfo = "Chapter not loaded";

        window.GetChapters();

        window.minSize = new Vector2(400, 400);
        window.Show();
    }

    void OnGUI() 
    {
        GUILayout.BeginVertical();

        GUILayout.Label(loadedChapterInfo);

        GUILayout.Space(10);

        if(GUILayout.Button("Load Chapter One"))
        {
            LoadChapterOne();

            loadedChapterInfo = "Chapter One loaded";
        }

        if(GUILayout.Button("Save & Unload Chapter"))
        {
            UnloadChapter();

            loadedChapterInfo = "Chapter not loaded";
        }

        GUILayout.EndVertical();
    }
    
    public void GetChapters()
    {
        GetChapterOneScenes();
    }

    private void GetChapterOneScenes()
    {
        string[] scenesPaths = Directory.GetFiles(PATH_CHAPTER_ONE, "*.unity", SearchOption.AllDirectories);

        chapterOneScenes = scenesPaths;
    }

    public void LoadChapterOne()
    {
        openedScenes = new Scene[chapterOneScenes.Length];

        for(int i = 0; i < chapterOneScenes.Length; i++)
        {
            openedScenes[i] = EditorSceneManager.OpenScene(chapterOneScenes[i], OpenSceneMode.Additive);
        }

        Debug.Log("Chapter One loaded with success.");
    }

    public void UnloadChapter()
    {
        try
        {
            EditorSceneManager.SaveOpenScenes();

            for(int i = 0; i < openedScenes.Length; i++)
            {
                EditorSceneManager.CloseScene(openedScenes[i], true);
            }

            GetChapters();

            Debug.Log("All the scenes are saved with success.");
        }
        catch
        {
            Debug.LogError("Something goes wrong when trying to save and close the scenes.");
        }
    }
}
