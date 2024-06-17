using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ScreenSafeArea : MonoBehaviour
{
    RectTransform rectTransform;

  
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        Vector2 minAnchor = safeArea.position;
        Vector2 maxAnchor = minAnchor + safeArea.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;
    }


    void Update()
    {
       
        if (rectTransform.anchorMin != (Vector2)Screen.safeArea.position / new Vector2(Screen.width, Screen.height) ||
            rectTransform.anchorMax != (Vector2)(Screen.safeArea.position + Screen.safeArea.size) / new Vector2(Screen.width, Screen.height))
        {
            ApplySafeArea();
        }
    }
}
