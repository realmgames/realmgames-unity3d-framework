using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealmGames
{
    [Serializable]
    public class SpriteState : IState {
        public string name;

        [Header("Background")]
        public Sprite background;
        public Color backgroundColor = Color.white;

        [Header("Foreground")]
        public Sprite foreground;
        public Color foregroundColor = Color.white;
    }

    public class SpriteStateMachine : MonoBehaviour, IStateMachine
    {
        public SpriteRenderer foreground;
        public SpriteRenderer background;

        public SpriteState[] states;

        private SpriteState m_current;

        public int StateCount {
            get {
                return states != null ? states.Length : 0;
            }
        }

        public IState GetState() {
            return m_current;
        }

        public IState GetState(int index)
        {
            if (index < 0 || index > states.Length - 1)
                return null;

            return states[index];
        }

        public IState GetState(string name)
        {
            for (int i = 0; i < states.Length; i++)
            {
                if (string.Equals(states[i].name, name))
                    return states[i];
            }

            return null;
        }

        public int GetStateIndex()
        {
            for (int i = 0; i < states.Length; i++)
            {
                if (states[i] == m_current)
                    return i;
            }

            return -1;
        }

        public int GetStateIndex(string name)
        {
            for (int i = 0; i < states.Length; i++)
            {
                if (string.Equals(states[i].name, name))
                    return i;
            }

            return -1;
        }

        public string GetStateName()
        {
            if (m_current == null)
                return string.Empty;
            
            return m_current.name;
        }

        public void SetOpacity(float opacity) {

            if (background != null)
            {
                Color color = background.color;
                color.a *= opacity;
                background.color = color;
            }

            if (foreground != null)
            {
                Color color = foreground.color;
                color.a *= opacity;
                foreground.color = color;
            }
        }

        public void SetOrderInLayer(int backgroundOrder, int foregroundOrder) {
            if (background != null)
                background.sortingOrder = backgroundOrder;

            if (foreground != null)
                foreground.sortingOrder = foregroundOrder;
        }

        public void SetStateColor(int index, Color bk, Color fg) {

            SpriteState state = (SpriteState)GetState(index);

            if (state == null)
            {
                Debug.LogError("invalid state index.");
                return;
            }

            state.backgroundColor = bk;
            state.foregroundColor = fg;

            if(state == m_current) {
                background.color = bk;
                foreground.color = fg;
            }
        }

        public void Set(IState state) {
            m_current = (SpriteState)state;

            if (foreground != null)
            {
                foreground.color = m_current.foregroundColor;
                foreground.sprite = m_current.foreground;
            }
            background.color = m_current.backgroundColor;
            background.sprite = m_current.background;
        }

        public void Set(IState state, Color backgroundColor)
        {
            m_current = (SpriteState)state;

            if (foreground != null)
            {
                foreground.color = m_current.foregroundColor;
                foreground.sprite = m_current.foreground;
            }
            background.color = backgroundColor;
            background.sprite = m_current.background;
        }

        public void SetForegroundState(string name)
        {
            SpriteState state = (SpriteState)GetState(name);

            if (state == null)
                return;
                
            if (foreground != null)
            {
                foreground.color = state.foregroundColor;
                foreground.sprite = state.foreground;
            }
        }

        public void SetState(int index)
        {
            Set(states[index]);
        }

        public void SetState(int index, Color backgroundColor)
        {
            Set(states[index], backgroundColor);
        }

        public void SetState(string name)
        {
            if(m_current != null && string.Equals(m_current.name, name) )
            {
                Set(m_current);
                return;
            }

            foreach(SpriteState state in states)
            {
                if(string.Equals(name, state.name)) {
                    Set(state);
                    return;
                }
            }
        }

        public void SetState(string name, Color backgroundColor)
        {
            if (m_current != null && string.Equals(m_current.name, name))
            {
                Set(m_current, backgroundColor);
                return;
            }

            foreach (SpriteState state in states)
            {
                if (string.Equals(name, state.name))
                {
                    Set(state, backgroundColor);
                    return;
                }
            }
        }
    }
}