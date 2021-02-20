using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RealmGames.TileSystem
{
    [Serializable]
    public class SpriteState
    {
        public string name;
        public Sprite sprite;
        public Color color = Color.white;
        public UnityEvent OnStateEnabled;
        public UnityEvent OnStateDisabled;
    }

    public class Tile : MonoBehaviour
    {
        public Vector2Int position;
        public SpriteRenderer spriteRenderer;
        public SpriteState[] states;

        private SpriteState m_current;

        public SpriteState GetState()
        {
            return m_current;
        }

        public string GetStateName()
        {
            if (m_current == null)
                return string.Empty;

            return m_current.name;
        }

        public void Set(SpriteState state, Color color)
        {
            if (m_current != null)
                m_current.OnStateDisabled.Invoke();

            m_current = state;
            spriteRenderer.color = color;
            spriteRenderer.sprite = state.sprite;

            m_current.OnStateEnabled.Invoke();
        }

        public void Set(SpriteState state)
        {
            Set(state, state.color);
        }

        public void SetState(int index)
        {
            Set(states[index]);
        }

        public void SetState(string name)
        {
            if (m_current != null && string.Equals(m_current.name, name))
            {
                Set(m_current);
                return;
            }

            foreach (SpriteState state in states)
            {
                if (string.Equals(name, state.name))
                {
                    Set(state);
                    return;
                }
            }
        }

        public void SetState(string name, Color color)
        {
            if (m_current != null && string.Equals(m_current.name, name))
            {
                Set(m_current, color);
                return;
            }

            foreach (SpriteState state in states)
            {
                if (string.Equals(name, state.name))
                {
                    Set(state, color);
                    return;
                }
            }
        }
    }
}