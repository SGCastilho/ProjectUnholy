using System.Collections;
using System.IO;
using UnityEngine;

namespace Core.Managers
{
    public class SaveFile
    {
        public string currentChapter;

        public int currentElapsedTime;
        public int currentDay, currentMonth, currentYear;

        public byte[] screenShootBytes;

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

        private TakeScreenshoot _takeScreenshoot;

        private bool _isSaving;

        void Awake() => _takeScreenshoot = GetComponent<TakeScreenshoot>();

        public void SaveGame(int saveSlotIndex)
        {
            if(_isSaving) return;

            //_takeScreenshoot.TakeScreenShoot = true;

            StartCoroutine(SaveCouroutine(saveSlotIndex));
        }

        IEnumerator SaveCouroutine(int saveSlotIndex)
        {
            _isSaving = true;

            while(_takeScreenshoot.TakeScreenShoot)
            {
                yield return null;
            }

            if(!Directory.Exists(Application.dataPath + "/Saved"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Saved");
            }

            SaveFile saveFile = new SaveFile();

            saveFile.currentChapter = OnGetCurrentChapter();

            saveFile.currentElapsedTime = PlaytimeManager.Instance.ElapsedTime;

            saveFile.currentDay = System.DateTime.Today.Day;
            saveFile.currentMonth = System.DateTime.Today.Month;
            saveFile.currentYear = System.DateTime.Today.Year;

            //Voltar mais tarde
            //saveFile.screenShootBytes = _takeScreenshoot.ScreenShootBytes;

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
            
            string savePath = Application.dataPath + $"/Saved/saveFile{saveSlotIndex}.save";
            string jsonOutput = JsonUtility.ToJson(saveFile, true);
            
            if(File.Exists(savePath))
            {
                File.Delete(savePath);

                Debug.LogWarning("Substituindo save atual");
            }

            File.WriteAllText(savePath, jsonOutput);

            Debug.Log("Jogo salvo com sucesso");

            _isSaving = false;
        }

        public string[] GetSlotInformation(int slotIndex)
        {
            string savePath = Application.dataPath + $"/Saved/saveFile{slotIndex}.save";

            if(File.Exists(savePath))
            {
                string jsonFile = string.Empty;
                SaveFile saveFile = new SaveFile();

                jsonFile = File.ReadAllText(savePath);
                saveFile = JsonUtility.FromJson<SaveFile>(jsonFile);

                string[] slotInformation = new string[3];

                slotInformation[0] = saveFile.currentChapter;
                slotInformation[1] = PlaytimeManager.Instance.ReturnPlayTimeInString(saveFile.currentElapsedTime);
                slotInformation[2] = $"{saveFile.currentMonth}/{saveFile.currentDay}/{saveFile.currentYear}";

                return slotInformation;
            }

            return null;
        }

        public Texture2D GetSlotScreenshoot(int slotIndex)
        {
            string savePath = Application.dataPath + $"/Saved/saveFile{slotIndex}.save";

            if(File.Exists(savePath))
            {
                string jsonFile = string.Empty;
                SaveFile saveFile = new SaveFile();

                jsonFile = File.ReadAllText(savePath);
                saveFile = JsonUtility.FromJson<SaveFile>(jsonFile);

                int width = Screen.width;
                int height = Screen.height;
                Texture2D screenshoot = new Texture2D(width, height, TextureFormat.ARGB32, false);

                screenshoot.LoadImage(saveFile.screenShootBytes);
                screenshoot.Apply();

                return screenshoot;
            }

            return null;
        }

        public bool GetSavingState()
        {
            return _isSaving;
        }
    }
}
