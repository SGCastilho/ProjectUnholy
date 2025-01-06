using UnityEngine;

namespace Core.UI
{
    public sealed class UIMainMenuButtonsActions : MonoBehaviour
    {
        public void NewGame()
        {
            Debug.Log("New Game");
        }

        public void LoadGame()
        {
            Debug.Log("Load Game");
        }

        public void Options()
        {
            Debug.Log("Options");
        }

        public void Credits()
        {
            Debug.Log("Credits");
        }
    }
}
