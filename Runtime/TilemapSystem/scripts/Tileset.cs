using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealmGames
{
    [Serializable]
    public class TileSprite
    {
        public string name;
        public Sprite sprite;
    }

    [CreateAssetMenu(fileName = "tileset", menuName = "New Tileset Definition")]
    public class Tileset : ScriptableObject
    {
        public TileSprite[] tileSprites;

        public Sprite GetSprite (string name)
        {
            foreach(TileSprite tileSprite in tileSprites)
            {
                if (tileSprite.name == name)
                    return tileSprite.sprite;
            }
            return null;
        }
    }
}