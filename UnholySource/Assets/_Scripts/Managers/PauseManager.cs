using UnityEngine;

namespace Core.Managers
{
    public sealed class PauseManager : MonoBehaviour
    {
        #region Events
        public delegate void Paused();
        public event Paused OnPaused;

        public delegate void UnPaused();
        public event UnPaused OnUnPaused;
        #endregion

        #region Encapsulation
        public bool IsPaused { get => _isPaused; }
        #endregion

        private bool _isPaused;

        public void Pause()
        {
            if(_isPaused) return;

            OnPaused?.Invoke();

            Time.timeScale = 0;

            _isPaused = true;
        }

        public void UnPause()
        {
            if(!_isPaused) return;

            OnUnPaused?.Invoke();

            Time.timeScale = 1;

            _isPaused = false;
        }
    }
}
