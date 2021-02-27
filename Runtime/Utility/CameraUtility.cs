using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealmGames
{
    public class CameraUtility
    {
        public static float ScreenHeightInWorldUnits
        {
            get
            {
                return Camera.main.orthographicSize * 2f;
            }
        }

        public static float ScreenWidthInWorldUnits
        {
            get
            {
                return ScreenHeightInWorldUnits * Camera.main.aspect;
            }
        }

        public static Vector3 CenterScreen
        {
            get
            {
                return new Vector3(ScreenWidthInWorldUnits / 2f, ScreenHeightInWorldUnits / 2f, 0f);
            }
        }
    }
}