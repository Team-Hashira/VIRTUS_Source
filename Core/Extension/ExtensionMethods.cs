using Hashira.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public static class ExtensionMethods
{
    public static string ToRGBHex(this Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255f);
        int g = Mathf.RoundToInt(color.g * 255f);
        int b = Mathf.RoundToInt(color.b * 255f);

        string rgbHex = $"{r.ToString("X2")}{g.ToString("X2")}{b.ToString("X2")}";
        return rgbHex;
    }

    public static List<Vector3> ToVector3List(this List<Transform> transformList)
    {
        return transformList.Select(trm => trm.position).ToList();
    }

    public static List<Vector2> ToVector2List(this List<Transform> transformList)
    {
        return transformList.Select(trm => (Vector2)trm.position).ToList();
    }

    public static void AnimateText(this TextMeshProUGUI textMeshProUGUI, string text, float speed = 10f)
    {
        StartCoroutine(AnimateTextCoroutine(textMeshProUGUI, text, speed));
    }

    public static float GetAnimationDuration(this string text, float speed)
    {
        float totalDuration = 0f;
        float characterDelay = 1f / speed;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '{')
            {
                int closeIndex = text.IndexOf('}', i);
                if (closeIndex != -1)
                {
                    string delayStr = text.Substring(i + 1, closeIndex - i - 1);
                    if (float.TryParse(delayStr, out float additionalDelay))
                    {
                        totalDuration += additionalDelay;
                    }

                    i = closeIndex;
                    continue;
                }
            }

            totalDuration += characterDelay;
        }
        return totalDuration;
    }

    private static IEnumerator AnimateTextCoroutine(TextMeshProUGUI textMeshProUGUI, string text, float speed)
    {
        textMeshProUGUI.text = "";
        int currentIndex = 0;
        float delay = 1f / speed;
        WaitForSeconds ws = new WaitForSeconds(delay);
        StringBuilder stringBuilder = new StringBuilder();

        while (currentIndex < text.Length)
        {
            char curChar = text[currentIndex];

            if (curChar == '{')
            {
                int closeIndex = text.IndexOf('}', currentIndex);
                if (closeIndex != -1)
                {
                    string delayStr = text.Substring(currentIndex + 1, closeIndex - currentIndex - 1);
                    if (float.TryParse(delayStr, out float additionalDelay))
                    {
                        yield return new WaitForSeconds(additionalDelay);
                    }

                    currentIndex = closeIndex + 1;
                    continue;
                }
            }

            stringBuilder.Append(curChar);
            textMeshProUGUI.text = stringBuilder.ToString();
            currentIndex++;
            yield return ws;
        }
    }

    private static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return GameManager.Instance.StartCoroutine(coroutine);
    }
}