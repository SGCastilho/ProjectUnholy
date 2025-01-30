using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Managers
{
    public sealed class ChapterEventsManager : MonoBehaviour
    {
        [System.Serializable]
        internal struct ChapterEvent
        {
            #region Encapsulation
            internal string Key { get => chapterKey; }
            internal bool IsTriggered { get => eventTriggered; set => eventTriggered = value; }
            #endregion

            [SerializeField] private string chapterKey;
            [SerializeField] private bool eventTriggered;

            [Space(10)]

            [SerializeField] internal UnityEvent OnEventTriggered;
        }

        [Header("Settings")]
        [SerializeField] private ChapterEvent[] chapterEvents;
        
        public void EndEvent(string eventKey)
        {
            for(int i = 0; i < chapterEvents.Length; i++)
            {
                if(chapterEvents[i].Key == eventKey)
                {
                    chapterEvents[i].IsTriggered = true;
                }
            }
        }

        /// <summary>
        /// End a event and execute OnEventTriggered
        /// </summary>
        /// <param name="eventKey"></param>
        public void EndEventAndExcute(string eventKey)
        {
            for(int i = 0; i < chapterEvents.Length; i++)
            {
                if(chapterEvents[i].Key == eventKey)
                {
                    chapterEvents[i].IsTriggered = true;
                }
            }

            ExecuteEndedEvents();
        }

        /// <summary>
        /// Execute all the ended events triggered by the player
        /// </summary>
        internal void ExecuteEndedEvents()
        {
            for(int i = 0; i < chapterEvents.Length; i++)
            {
                if(chapterEvents[i].IsTriggered)
                {
                    chapterEvents[i].OnEventTriggered?.Invoke();
                }
            }
        }

        /// <summary>
        /// Execute the ended of a event using a specify key
        /// </summary>
        /// <param name="key">Chapter event key</param>
        internal void ExecuteEndedEventsByKey(string key)
        {
            for(int i = 0; i < chapterEvents.Length; i++)
            {
                if(chapterEvents[i].Key == key)
                {
                    chapterEvents[i].OnEventTriggered?.Invoke();
                    break;
                }
            }
        }

        public string[] GetAllEndedEvents()
        {
            List<string> endedEvents = new List<string>();

            for(int i = 0; i < chapterEvents.Length; i++)
            {
                if(chapterEvents[i].IsTriggered)
                {
                    endedEvents.Add(chapterEvents[i].Key);
                }
            }

            return endedEvents.ToArray();
        }
    }
}
