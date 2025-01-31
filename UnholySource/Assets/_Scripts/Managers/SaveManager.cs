using System.IO;
using UnityEngine;

namespace Core.Managers
{
    public class SaveFile
    {
        public string currentChapter;

        public string currentScene;
        public string currentLoadedRoom;

        public Vector3 currentPlayerLocation;

        public int currentPlayerHealth;
        public int currentPlayerHealthBottles;
        public int currentPlayerBullets;
        public int currentPlayerMunition;

        public bool currentWeaponMeleeStatus;
        public bool currentWeaponRangedStatus;

        public string[] currentInventoryItems;

        public bool currentCharserEnabled;

        public string[] currentTriggeredScenarioEvents;
    }

    public sealed class SaveManager : MonoBehaviour
    {
        #region Events
        public delegate string GetCurrentChapter();
        public event GetCurrentChapter OnGetCurrentChapter;

        public delegate string GetCurrentLevel();
        public event GetCurrentLevel OnGetCurrentLevel;

        public delegate string GetCurrentRoom();
        public event GetCurrentRoom OnGetCurrentRoom;

        public delegate Vector3 GetPlayerCurrentPos();
        public event GetPlayerCurrentPos OnGetPlayerCurrentPos;

        public delegate int GetPlayerCurrentHealth();
        public event GetPlayerCurrentHealth OnGetPlayerCurrentHealth;

        public delegate int GetPlayerCurrentHealthBottles();
        public event GetPlayerCurrentHealthBottles OnGetPlayerCurrentHealthBottles;

        public delegate int GetPlayerCurrentBullets();
        public event GetPlayerCurrentBullets OnGetPlayerCurrentBullets;

        public delegate int GetPlayerCurrentMunition();
        public event GetPlayerCurrentMunition OnGetPlayerCurrentMunition;

        public delegate bool GetPlayerMeleeStatus();
        public event GetPlayerMeleeStatus OnGetPlayerMeleeStatus;

        public delegate bool GetPlayerRangedStatus();
        public event GetPlayerRangedStatus OnGetPlayerRangedStatus;

        public delegate string[] GetInventoryItems();
        public event GetInventoryItems OnGetInventoryItems;

        public delegate bool GetChaserStatus();
        public event GetChaserStatus OnGetChaserStatus;

        public delegate string[] GetChapterEventsTriggereds();
        public event GetChapterEventsTriggereds OnGetChapterEventsTriggereds;
        #endregion

        private string _savePath = Application.dataPath + "/Saved/saveFile.save";

        public void SaveGame()
        {
            if(!Directory.Exists(Application.dataPath + "/Saved"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Saved");
            }

            SaveFile saveFile = new SaveFile();

            saveFile.currentChapter = OnGetCurrentChapter();

            saveFile.currentScene = OnGetCurrentLevel();
            saveFile.currentLoadedRoom = OnGetCurrentRoom();

            saveFile.currentPlayerLocation = OnGetPlayerCurrentPos();

            saveFile.currentPlayerHealth = OnGetPlayerCurrentHealth();
            saveFile.currentPlayerHealthBottles = OnGetPlayerCurrentHealthBottles();

            saveFile.currentPlayerBullets = OnGetPlayerCurrentBullets();
            saveFile.currentPlayerMunition = OnGetPlayerCurrentMunition();

            saveFile.currentWeaponMeleeStatus = OnGetPlayerMeleeStatus();
            saveFile.currentWeaponRangedStatus = OnGetPlayerRangedStatus();

            saveFile.currentInventoryItems = OnGetInventoryItems();

            saveFile.currentCharserEnabled = OnGetChaserStatus();
            
            saveFile.currentTriggeredScenarioEvents = OnGetChapterEventsTriggereds();

            string jsonOutput = JsonUtility.ToJson(saveFile, true);
            
            if(File.Exists(_savePath))
            {
                File.Delete(_savePath);

                Debug.LogWarning("Substituindo save atual");
            }

            File.WriteAllText(_savePath, jsonOutput);

            Debug.Log("Jogo salvo com sucesso");
        }
    }
}
