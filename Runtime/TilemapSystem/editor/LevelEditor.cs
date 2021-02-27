using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RealmGames.TileSystem
{
    public class LevelEditor : EditorWindow
    {
        string m_path = "Assets/Realm Games/MainGame/assets/tilemaps";
        int m_slider = 0;
        bool m_mouseDown = false;

        Color[] m_colors = new Color[] {
                Color.black,
                Color.blue,
                Color.green,
                Color.magenta,
                Color.red,
                Color.yellow
            };

        [MenuItem("RealmGames/Tilemap Level Editor")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(LevelEditor));
        }

        void OnInspectorUpdate()
        {
            if (EditorWindow.focusedWindow == this &&
                EditorWindow.mouseOverWindow == this)
            {
                this.Repaint();
            }
        }

        void OnGUI()
        {
            if (Selection.activeObject == null)
                return;

            if (Selection.activeObject.GetType() != typeof(TilemapDefinition))
                return;

            TilemapDefinition targetObj = (TilemapDefinition)Selection.activeObject;

            Vector2Int offset = new Vector2Int(25,50);

            int blocksize = 25;

            m_slider = EditorGUI.IntSlider(new Rect(25, 10, 200, 30), m_slider, 0, m_colors.Length-1);

            Event e = Event.current;
            //GUILayout.Label("Mouse pos: " + e.mousePosition);

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                m_mouseDown = true;
            }
            else if (e.type == EventType.MouseUp && e.button == 0)
            {
                m_mouseDown = false;
            }
            else if (e.type == EventType.MouseLeaveWindow)
            {
                m_mouseDown = false;
            }

            for (int y = 0; y < targetObj.height; y++)
            {
                for (int x = 0; x < targetObj.width; x++)
                {
                    int index = x + (y * targetObj.width);

                    Color color = m_colors[targetObj.map[index]];

                    Vector2Int pos = new Vector2Int(x * blocksize, -y * blocksize) + offset + new Vector2Int(0, targetObj.height * blocksize);

                    if(e.mousePosition.x >= pos.x &&
                       e.mousePosition.x < pos.x+ blocksize &&
                       e.mousePosition.y >= pos.y &&
                       e.mousePosition.y < pos.y + blocksize)
                    {
                        color = Color.yellow;

                        if (m_mouseDown)
                        {
                            targetObj.map[index] = (byte)m_slider;

                            EditorUtility.SetDirty(targetObj);
                        }

                        GUILayout.Label("Mouse pos: " + x + "," + y);
                    }

                    EditorGUI.DrawRect(new Rect(pos.x, pos.y, blocksize, blocksize), color);
                }
            }
        }
    }
}