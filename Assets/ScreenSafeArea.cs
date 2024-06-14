using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSafeArea : MonoBehaviour
{
    Vector2 minAnchor = Vector2.zero;
    Vector2 maxAnchor = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        // �ν����Ϳ� �ִ� �� -> Rect : RectTransform�� ���ð�
        var CurRect = this.GetComponent<RectTransform>();

        minAnchor = Screen.safeArea.min;
        maxAnchor = Screen.safeArea.max;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;

        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        CurRect.anchorMin = minAnchor;
        CurRect.anchorMax = maxAnchor;
    }

}
