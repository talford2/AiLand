using UnityEngine;

public class Utility
{
    public static Vector3 AtHeight(Vector3 postion, float height)
    {
        return new Vector3(postion.x, height, postion.z);
    }
}