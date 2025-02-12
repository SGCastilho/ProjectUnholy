using System.Collections;
using UnityEngine;

namespace Core.Managers
{
    public class PlaytimeManager : MonoBehaviour
    {
        #region Singleton
        public static PlaytimeManager Instance { get => instance; }
        #endregion

        private static PlaytimeManager instance;

        #region Encapsulation
        public int Seconds { get => seconds; }
        public int Minutes { get => minutes; }
        public int Hours { get => hours; }

        public int ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
        #endregion

        [Header("Data")]
        [SerializeField] private int seconds;
        [SerializeField] private int minutes;
        [SerializeField] private int hours;

        [Header("Settings")]
        [SerializeField] private bool startCounterOnAwake = true;

        private int elapsedTime;

        private void Awake()
        {
            if(instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if(startCounterOnAwake)
            {
                StartCoroutine(Playtimer());
            }
        }

        IEnumerator Playtimer()
        {
            while(true)
            {
                yield return new WaitForSeconds(1);
                elapsedTime += 1;
                seconds = elapsedTime % 60;
                minutes = elapsedTime / 60 % 60;
                hours = elapsedTime / 3600;
            }
        }

        internal void DestroyInstance() => Destroy(gameObject);

        internal string ReturnPlayTimeInString(int savedElapsedTime)
        {
            int l_hours = savedElapsedTime / 3600;
            int l_minutes = savedElapsedTime / 60 % 60;
            int l_seconds = savedElapsedTime % 60;

            string playTime = $"{l_hours}:{l_minutes}:{l_seconds}";

            return playTime;
        }
    }
}
