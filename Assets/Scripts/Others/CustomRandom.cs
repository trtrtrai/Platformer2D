using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomRandom
{
    public static Vector2 RdVector2(float xMin, float xMax, float yMin, float yMax)
    {
        var x = Random.Range(xMin, xMax);
        var y = Random.Range(yMin, yMax);

        return new Vector2(x, y);
    }

    public static int RdPositiveRange(int num, int min = 0)
    {
        var n = Random.Range(min * 100, num * 100);

        return n / 100;
    }
}
