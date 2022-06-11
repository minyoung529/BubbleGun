using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethods
{
    public static class MyExtension
    {
        public static Quaternion RandomRotation()
        {
            Vector3 angles;
            angles.x = Random.Range(0f, 360f);
            angles.y = Random.Range(0f, 360f);
            angles.z = Random.Range(0f, 360f);

            return Quaternion.Euler(angles);
        }

        public static Vector3 RandomPositionInRadius(Vector3 position, float radius)
        {
            Vector3 randomPos = position;
            randomPos.x += Random.Range(-radius, radius);
            randomPos.z += Random.Range(-radius, radius);

            // LATER: HAVE TO FIX
            randomPos.y = 8f;

            return randomPos;
        }
    }
}