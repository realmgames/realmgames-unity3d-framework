using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealmGames
{
    public class MouseUtility
    {
        public static Vector2 MousePositionOnPlaneVec2(Transform transform, Camera camera)
        {
            float z_plane = transform.position.z - camera.transform.position.z;

            Vector3 mouse = Input.mousePosition;

            Vector3 world = camera.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, z_plane));

            return new Vector2(world.x, world.y);
        }

        public static Vector2 MousePositionOnPlaneVec3(Transform transform, Camera camera)
        {
            float z_plane = transform.position.z - camera.transform.position.z;

            Vector3 mouse = Input.mousePosition;

            return camera.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, z_plane));

        }
    }
}