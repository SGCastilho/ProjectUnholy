using UnityEngine;

namespace Core.Managers
{
    public sealed class ChaserManager : MonoBehaviour
    {
        #region Events
        public delegate void ChaserEmitSounds();
        public event ChaserEmitSounds OnChaserEmitSounds;

        public delegate void EmitSound(string sfxKey);
        public event EmitSound OnEmitSound;

        public delegate void StopEmittingSound();
        public event StopEmittingSound OnStopEmittingSound;

        public delegate void ChaserStopEmitingSounds();
        public event ChaserStopEmitingSounds OnChaserStopEmitingSounds;

        public delegate void ChasingEnd();
        public event ChasingEnd OnChasingEnd;

        public delegate void SpawnNoRules(string sfxKey, float delay);
        public event SpawnNoRules OnSpawnNoRules;

        public delegate Transform GetPlayerLastSpawn();
        public event GetPlayerLastSpawn OnGetPlayerLastSpawn;
        #endregion

        #region Constants
        private const string SFX_DOOR_OPEN = "sfx_door_open";
        #endregion

        #region Encapsulation
        public bool IsEnabled { get => chaserEnabled; }

        public bool IsChasing { get => isChasing; }
        public bool FirstTimeSpawn { get => firstTimeSpawn; }
        #endregion

        [Header("Classes")]
        [SerializeField] private GameObject chaserPrefab;

        [Header("Settings")]
        [SerializeField] private bool chaserEnabled;

        [Space(10)]

        [SerializeField] private bool emitSoundWhenGettingCloser;
        [SerializeField] private string[] emitSoundSFXKeys;

        [Space(10)]

        [SerializeField] private AudioSource chaserLocalAudioSource;

        [Space(10)]

        [SerializeField] private bool isChasing;
        [SerializeField] private bool firstTimeSpawn;

        [Space(10)]

        [SerializeField] private int maxRoomsToSpawn = 6;
        [SerializeField] private int currentRoomsToSpawn;

        [Space(6)]

        [SerializeField] private int targetRoomToSinalize = 3;

        [Space(10)]
        [SerializeField] private int maxRoomsToDeSpawn = 6;
        [SerializeField] private int currentNeededRoomsToDeSpawn;

        private int _currentEmittedSound;
        private bool _emitingSounds;

        private void OnEnable() 
        {
            firstTimeSpawn = true;
        }

        public void EnableChaser()
        {
            chaserEnabled = true;
            ResetChaser();
        }

        public void DisableChaser()
        {
            chaserEnabled = false;
            ResetChaser();
        }

        /// <summary>
        /// Spawn the chaser ignoring all the conditions to appears
        /// </summary>
        public void SpawnChaserWithoutRules()
        {
            if(isChasing || !chaserEnabled) return;

            _currentEmittedSound = 0;
            _emitingSounds = false;

            OnChaserStopEmitingSounds?.Invoke();

            isChasing = true;
            currentRoomsToSpawn = maxRoomsToSpawn;

            SpawnChaser(OnGetPlayerLastSpawn());

            OnSpawnNoRules?.Invoke(SFX_DOOR_OPEN, 0.5f);
        }

        /// <summary>
        /// Increment the amount of rooms to spawn the chaser
        /// </summary>
        internal void ChaserGetCloser()
        {
            if(isChasing) return;

            OnStopEmittingSound?.Invoke();

            currentRoomsToSpawn++;

            if(emitSoundWhenGettingCloser)
            {
                if(currentRoomsToSpawn >= targetRoomToSinalize-1 && currentRoomsToSpawn < maxRoomsToSpawn)
                {
                    if(!_emitingSounds)
                    {
                        _emitingSounds = true;
                        OnChaserEmitSounds?.Invoke();
                    }

                    OnEmitSound?.Invoke(emitSoundSFXKeys[_currentEmittedSound]);

                    _currentEmittedSound++;
                }
            }

            if(currentRoomsToSpawn >= maxRoomsToSpawn)
            {
                _currentEmittedSound = 0;
                _emitingSounds = false;

                OnChaserStopEmitingSounds?.Invoke();

                isChasing = true;
                currentRoomsToSpawn = maxRoomsToSpawn;
            }
        }

        internal void SpawnChaser(Transform spawnLocation)
        {
            if(!isChasing) return;

            chaserPrefab.SetActive(false);

            chaserPrefab.transform.position = spawnLocation.position;

            chaserPrefab.SetActive(true);

            UnMuteSFX();

            if(firstTimeSpawn) { firstTimeSpawn = false; }
        }

        /// <summary>
        /// Increase the rooms needed to hide chaser again
        /// </summary>
        internal void ChaserCountToHideAgain()
        {
            if(!isChasing) return;

            currentNeededRoomsToDeSpawn++;
            if(currentNeededRoomsToDeSpawn >= maxRoomsToDeSpawn)
            {
                ResetChaser();
            }
        }

        public void ResetChaser()
        {
            chaserPrefab.SetActive(false);

            currentRoomsToSpawn = 0;
            currentNeededRoomsToDeSpawn = 0;

            firstTimeSpawn = true;

            OnChaserStopEmitingSounds?.Invoke();

            isChasing = false;

            OnChasingEnd?.Invoke();
        }

        public void MuteSFX()
        {
            chaserLocalAudioSource.volume = 0f;
        }

        public void UnMuteSFX()
        {
            chaserLocalAudioSource.volume = 0.2f;
        }

        public bool GetChaserStatus()
        {
            return chaserEnabled;
        }

        public void LoadChaserStatus(bool enabled)
        {
            chaserEnabled = enabled;
        }
    }
}
