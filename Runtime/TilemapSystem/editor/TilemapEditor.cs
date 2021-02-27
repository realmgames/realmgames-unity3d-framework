using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RealmGames.TileSystem
{
    [CustomEditor(typeof(TilemapDefinition)), CanEditMultipleObjects]
    public class TilemapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            TilemapDefinition targetObj = (TilemapDefinition)target;

            if (targets.Length == 1)
            {
                if (targetObj.width < 1)
                    targetObj.width = 1;

                if (targetObj.height < 1)
                    targetObj.height = 1;

                if (targetObj.map == null || targetObj.map.Length < targetObj.width * targetObj.height)
                {
                    targetObj.map = new byte[targetObj.width * targetObj.height];

                    for (int x = 0; x < targetObj.width; x++)
                        for (int y = 0; y < targetObj.height; y++)
                    {
                        int index = x + (y * targetObj.width);

                            targetObj.map[index] = 1;
                    }
                }

                for (int y = targetObj.height - 1; y >= 0; y--)
                {
                    EditorGUILayout.BeginHorizontal(new GUILayoutOption[]{
                    GUILayout.Width(targetObj.width * 25f),
                    GUILayout.Height(25f),
                });
                    for (int x = 0; x < targetObj.width; x++)
                    {
                        int index = x + (y * targetObj.width);

                        targetObj.map[index] = EditorGUILayout.Toggle(targetObj.map[index] == 1) ? (byte)1 : (byte)0;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            if(GUILayout.Button("Compute Block Count")) {
                foreach(TilemapDefinition t in targets) {
                    t.blockCount = t.CountBlocks();
                }
            }

            // Show default inspector property editor
            DrawDefaultInspector();

        }
    }
}