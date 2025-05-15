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

    /// <summary>
    /// 포물선 구하기 함수. 중력이 (0, gravity)라고 가정.
    /// </summary>
    /// <param name="direction">시점으로부터 쏘아진 방향</param>
    /// <param name="power">쏘아진 힘</param>
    /// <param name="gravity">중력</param>
    /// <param name="t"></param>
    /// <returns>포물선 함수에 t를 넣었을때 나오는 값 반환</returns>
    public static Vector2 Parabola(Vector2 direction, float power, float gravity, float t)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);

        Vector2 v = new Vector2(power * Mathf.Cos(angle), power * Mathf.Sin(angle));
        Vector2 result = new Vector2(v.x * t, v.y * t - (gravity * t * t) * 0.5f);
        return result;
    }

    /// <summary>
    /// 포물선 구하기 함수. 중력을 커스텀
    /// </summary>
    /// <param name="direction">시점으로부터 쏘아진 방향</param>
    /// <param name="power">쏘아진 힘</param>
    /// <param name="gravity">중력</param>
    /// <param name="t"></param>
    /// <returns>포물선 함수에 t를 넣었을때 나오는 값 반환</returns>
    public static Vector2 Parabola(Vector2 direction, float power, Vector2 gravity, float t)
    {
        Vector2 velocity = power * direction;
        Vector2 gravityEffect = gravity * t * t * 0.5f;

        Vector2 result = velocity * t + gravityEffect;
        return result;
    }

    public static Vector2 Parabola(float angle, float power, Vector2 gravity, float t)
    {
        Vector2 velocity = new Vector2(power * Mathf.Cos(angle), power * Mathf.Sin(angle));
        Vector2 gravityEffect = gravity * t * t * 0.5f;

        Vector2 result = velocity * t + gravityEffect;
        return result;
    }
}
