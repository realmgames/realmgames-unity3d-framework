using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealmGames
{
    public class SafeArea : MonoBehaviour
    {
        public RectTransform rootTransform;

        RectTransform Panel;

        Vector2 _starting = Vector2.zero;

        void Awake()
        {
            Panel = GetComponent<RectTransform>();

            _starting = Panel.anchoredPosition;
        }

        void OnEnable()
        {
            Refresh();
        }

        void Refresh()
        {
            float normalized = Screen.safeArea.y / Screen.height;

            float height = normalized * rootTransform.rect.height;

            Panel.anchoredPosition = _starting + new Vector2(0, -height);
        }
    }
}