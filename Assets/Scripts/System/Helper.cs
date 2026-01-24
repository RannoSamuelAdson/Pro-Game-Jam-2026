using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public static class Helper
{
    public static void SetGradient(Color uiColor, TMP_ColorGradient gradient)
    {
        gradient.topLeft = uiColor;
        gradient.topRight = uiColor;
        gradient.bottomLeft = uiColor;
        gradient.bottomRight = uiColor;
    }
    // https://discussions.unity.com/t/how-to-convert-from-world-space-to-canvas-space/117981/17
    public static Vector2 WorldToCanvasPosition(Canvas canvas, Camera worldCamera, Vector3 worldPosition)
    {
        //Vector position (percentage from 0 to 1) considering camera size.
        //For example (0,0) is lower left, middle is (0.5,0.5)
        Vector2 viewportPoint = worldCamera.WorldToViewportPoint(worldPosition);

        var rootCanvasTransform = (canvas.isRootCanvas ? canvas.transform : canvas.rootCanvas.transform) as RectTransform;
        var rootCanvasSize = rootCanvasTransform!.rect.size;
        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
        var rootCoord = (viewportPoint - rootCanvasTransform.pivot) * rootCanvasSize;
        if (canvas.isRootCanvas)
            return rootCoord;

        var rootToWorldPos = rootCanvasTransform.TransformPoint(rootCoord);
        return canvas.transform.InverseTransformPoint(rootToWorldPos);
    }
    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime.ToString("dd.MM.yy");
    }

    // from https://stackoverflow.com/questions/47143837/resizing-a-list-to-fit-a-new-length
    public static void PaddedResize<T>(List<T> list, int size, T padding = default
)
    {
        // Compute difference between actual size and desired size
        var deltaSize = list.Count - size;

        if (deltaSize < 0)
        {
            // If the list is smaller than target size, fill with `padding`
            list.AddRange(Enumerable.Repeat(padding, -deltaSize));
        }
        else
        {
            // If the list is larger than target size, remove end of list
            list.RemoveRange(size, deltaSize);
        }
    }


    public static Transform GetChildGameObject(Transform fromGameObject, string withName)
    {
        var allKids = fromGameObject.GetComponentsInChildren<Transform>();
        var kid = allKids.FirstOrDefault(k => k.gameObject.name == withName);
        if (kid == null) return null;
        return kid;
    }

    public static void ResizeSaveList(List<bool> unlockedItems, int itemCount)
    {
        if (unlockedItems.Count != itemCount)
        {
            Helper.PaddedResize(unlockedItems, itemCount, false);
        }
    }
    public static void Reshuffle<T>(List<T> list)
    {
        // Knuth shuffle algorithm
        for (int t = 0; t < list.Count; t++)
        {
            T tmp = list[t];
            int r = UnityEngine.Random.Range(t, list.Count);
            list[t] = list[r];
            list[r] = tmp;
        }
    }



    //https://stackoverflow.com/questions/4488969/split-a-string-by-capital-letters
    public static string SplitCamelCase(string source)
    {
        return string.Join(" ", Regex.Split(source, @"(?<!^)(?=[A-Z](?![A-Z]|$))"));
    }

    public static Color ToPastel(Color color, float blendFactor = 0.5f)
    {
        // Blend the color with white
        Color white = Color.white;
        Color pastelColor = Color.Lerp(color, white, blendFactor);
        return pastelColor;
    }

    public static Color ToDarker(Color color, float blendFactor = 0.5f)
    {
        // Blend the color with black
        Color black = Color.black;
        Color darkerColor = Color.Lerp(color, black, blendFactor);
        return darkerColor;
    }

    public static Color TransparentColor(Color input)
    {
        return new Color(input.r, input.g, input.b, 0);
    }

    // https://stackoverflow.com/a/71231025
    public static void AddExplosionForce(this Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Impulse)
    {
        var explosionDir = rb.position - explosionPosition;
        var explosionDistance = (explosionDir.magnitude / explosionRadius);

        // Normalize without computing magnitude again
        if (upwardsModifier == 0)
        {
            explosionDir /= explosionDistance;
        }
        else
        {
            // If you pass a non-zero value for the upwardsModifier parameter, the direction
            // will be modified by subtracting that value from the Y component of the centre point.
            explosionDir.y += upwardsModifier;
            explosionDir.Normalize();
        }

        rb.AddForce(Mathf.Lerp(0, explosionForce, (1 - explosionDistance)) * explosionDir, mode);
    }
}
