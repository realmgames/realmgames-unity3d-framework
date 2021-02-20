using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealmGames
{
    public class StageDefinition : ScriptableObject
    {
        private bool m_initialized = false;
        private bool m_isComplete = false;

        private void Init()
        {
            m_initialized = true;
            m_isComplete = PlayerPrefs.GetInt("stage." + name + ".complete", 0) == 1;
        }

        public bool IsComplete
        {
            get
            {
                if(!m_initialized)
                {
                    Init();
                }

                return m_isComplete;
            }
            set
            {
                m_isComplete = value;

                PlayerPrefs.SetInt("stage." + name + ".complete", value ? 1 : 0);
            }
        }
    }
}