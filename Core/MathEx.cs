using System.Collections.Generic;
using UnityEngine;

public static class MathEx
{
    #region Ease
    public static float InSine(float x)
        => 1f - Mathf.Cos((x * Mathf.PI) / 2f);
    public static float OutSine(float x)
        => Mathf.Sin((x * Mathf.PI) / 2f);
    public static float InOutSine(float x)
        => -(Mathf.Cos(Mathf.PI * x) - 1f) / 2f;
    public static float InQuad(float x)
        => x * x;
    public static float OutQuad(float x)
        => 1f - (1f - x) * (1f - x);
    public static float InOutQuad(float x)
        => x < 0.5f ? 2f * x * x : 1f - Mathf.Pow(-2f * x + 2f, 2f) / 2f;
    public static float InCubic(float x)
        => x * x * x;
    public static float OutCubic(float x)
        => 1f - Mathf.Pow(1f - x, 3f);
    public static float InOutCubic(float x)
        => x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
    public static float InCirc(float x)
        => 1f - Mathf.Sqrt(1f - Mathf.Pow(x, 2f));
    public static float OutCirc(float x)
        => Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
    public static float InOutCirc(float x)
        => x < 0.5f ? (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * x, 2f))) / 2f : (Mathf.Sqrt(1f - Mathf.Pow(-2f * x + 2f, 2f)) + 1f) / 2f;
    #endregion

    public static Vector2 Bezier(float t, params Vector2[] positions)
    {
        if (positions.Length == 0)
            return Vector2.zero;
        if (positions.Length == 1)
            return positions[0];
        if (positions.Length == 2)
            return Vector2.Lerp(positions[0], positions[1], t);

        Vector2[] nextPositions = new Vector2[positions.Length - 1];

        for (int i = 0; i < nextPositions.Length; i++)
        {
            nextPositions[i] = Vector2.Lerp(positions[i], positions[i + 1], t);
        }

        return Bezier(t, nextPositions);
    }

    public static Vector2 Bezier(float t, List<Vector2> positionList)
    {
        if (positionList.Count == 0)
            return Vector2.zero;
        if (positionList.Count == 1)
            return positionList[0];
        if (positionList.Count == 2)
            return Vector2.Lerp(positionList[0], positionList[1], t);

        Vector2[] nextPositions = new Vector2[positionList.Count - 1];

        for (int i = 0; i < nextPositions.Length; i++)
        {
            nextPositions[i] = Vector2.Lerp(positionList[i], positionList[i + 1], t);
        }

        return Bezier(t, nextPositions);
    }
}
