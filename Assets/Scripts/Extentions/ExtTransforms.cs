using UnityEngine;

public static class ExtTransforms
{
    public static void DestroyChildren(this Transform t, bool destroyImmediate)
    {
        foreach(Transform child in t)
        {
            if (destroyImmediate)
            {
                MonoBehaviour.DestroyImmediate(child.gameObject);
            }
            else
            {
                MonoBehaviour.Destroy(child.gameObject);
            }
        }
    }
}
