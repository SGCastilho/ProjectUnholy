using UnityEngine;

namespace Core.Managers
{
    public sealed class PursuerManager : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private GameObject pursuerPrefab;

        [Header("Settings")]
        [SerializeField] private bool enablePursuer;

        [Space(10)]

        [SerializeField] [Range(4f, 18f)] private float timeToSpawn = 12f;
        [SerializeField] [Range(4f, 18f)] private float timeToEndChasing = 12f;

        [Space(10)]

        [SerializeField] private int stepsRange = 3;
        [SerializeField] private int currentStep;
        [SerializeField] private AudioClip[] stepsAudioClips = new AudioClip[3];

        [Space(10)]

        [SerializeField] [Range(4f, 12f)] private float timeToNextStep = 6f;

        private bool _chasingStarted;
        private bool _pursuerSpawnned;
        private bool _pursuerAppears;
        private float _currentTimeToSpawn;
        private float _currentTimeToNextStep;
        private float _currentTimeToEndChasing;

        private void OnEnable() 
        {
            pursuerPrefab.SetActive(false);

            if(!enablePursuer) { enabled = false; }
        }

        private void Update() 
        {
            if(!_pursuerSpawnned)
            {
                _currentTimeToSpawn += Time.deltaTime;
                if(_currentTimeToSpawn > timeToSpawn)
                {
                    //INICIAR SONS DE STEPS

                    _pursuerSpawnned = true;
                    _currentTimeToSpawn = 0f;
                }
            }

            if(_pursuerSpawnned && !_pursuerAppears)
            {
                _currentTimeToNextStep += Time.deltaTime;
                if(_currentTimeToNextStep > timeToNextStep)
                {
                    IncreaseSteps();
                }
            }
            
            //CHASING STARTED SERA INICIADO ASSIM QUE O PURSUER IDENTIFICAR O JOGADOR, CASO AO CONTRARIO ELE COMEÇARÁ A PERSEGUIR
            //O JOGADOR DURANTE AS SALAS, EM SEU MODO SEARCH
            if(_chasingStarted && _pursuerSpawnned && _pursuerAppears)
            {
                _currentTimeToEndChasing += Time.deltaTime;
                if(_currentTimeToEndChasing > timeToEndChasing)
                {
                    _chasingStarted = false;
                    _currentTimeToEndChasing = 0f;
                }
            }
        }

        public void EndPursuerChasing()
        {
            if(_chasingStarted) return;

            pursuerPrefab.SetActive(false);

            _pursuerSpawnned = false;
            _pursuerAppears = false;
        }

        private void IncreaseSteps()
        {
            currentStep++;
            if(currentStep > stepsRange)
            {
                currentStep = 0;

                //PARA SONS DE STEPS

                //REPOSICIONAR O PURSUER NO SPAWN MAIS LONGE DA SALA
                pursuerPrefab.SetActive(true);

                _pursuerAppears = true;

                _currentTimeToNextStep = 0f;

                return;
            }

            //TROCAR SONS DE STEPS

            _currentTimeToNextStep = 0f;
        }
    }
}
