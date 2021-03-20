using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealmGames.TileSystem
{
    public class TilemapScaler : MonoBehaviour
    {
        public Vector2Int margin;
        public Vector3 offset;
        public Vector3 padding;

        public void FitToSize(float width, float height)
        {
            if (width > 0 && height > 0)
            {
                float fitToWidth = (width / Camera.main.aspect) * 0.5f;
                float fitToHeight = height / 2f;

                Camera.main.orthographicSize = fitToWidth > fitToHeight ? fitToWidth : fitToHeight;

                Camera.main.transform.position = new Vector3(CameraUtility.ScreenWidthInWorldUnits / 2f, Camera.main.orthographicSize, -1f);

                // UpdateAnchoredPosition();
            }
            else
            {
                Debug.LogWarning("invalid FitToSize: " + width + "," + height);
            }
        }

        public void Center()
        {
            Tilemap tilemap = GetComponent<Tilemap>();

            float w = tilemap.Width + margin.x + padding.x;
            float h = tilemap.Height + margin.y + padding.y;

            FitToSize(w, h);

            float x = (CameraUtility.ScreenWidthInWorldUnits - (float)(tilemap.Width + padding.x)) / 2f;
            float y = (CameraUtility.ScreenHeightInWorldUnits - (float)(tilemap.Height + padding.y)) / 2f;

            transform.position = new Vector3(x, y, 0f) + offset;
        }
    }
}
