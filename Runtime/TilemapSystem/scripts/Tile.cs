using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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

    public class Tile : MonoBehaviour, IPointerClickHandler
    {
        public Vector2Int position;
        public bool free;
        public SpriteRenderer foreground;
        public SpriteRenderer background;

        private GameObject m_block;
        private Tilemap m_tilemap;

        public GameObject Block
        {
            get
            {
                return m_block;
            }
        }

        public Tilemap Tilemap
        {
            get
            {
                if (m_tilemap == null)
                    m_tilemap = transform.parent.GetComponent<Tilemap>();

                return m_tilemap;
            }
        }

        public void Cleanup()
        {
            free = true;
            if (m_block != null)
            {
                Destroy(m_block);
                m_block = null;
            }
        }

        public void Free()
        {
            free = true;
            m_block = null;
        }

        public void DestroyBlock()
        {
            free = true;
            if (m_block != null)
            {
                Rigidbody2D rigidbody2D = m_block.AddComponent<Rigidbody2D>();

                rigidbody2D.AddForce(new Vector2(UnityEngine.Random.Range(-2f, 2f), 3f), ForceMode2D.Impulse);
                rigidbody2D.AddTorque(2f * (UnityEngine.Random.value - 0.5f), ForceMode2D.Impulse);

                //m_block.AddComponent<TimedDestructor>().delay = 2f;

                m_block = null;
            }
        }

        public void SetBlock(GameObject block)
        {
            m_block = block;
            m_block.transform.SetParent(transform);
            m_block.transform.localPosition = Vector3.zero;
            free = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Tilemap?.TileClicked(this);
        }
    }
}