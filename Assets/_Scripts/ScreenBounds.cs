using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    private float minX, maxX, minY, maxY;
    private float objectWidth, objectHeight;

    private void Start()
    {
        Camera mainCamera = Camera.main;

        // Tinh toan gio han man hinh
        float vertExtent = mainCamera.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        // lay kich thuoc cua doi tuong
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        objectWidth = spriteRenderer.bounds.extents.x;
        objectHeight = spriteRenderer.bounds.extents.y;

        // Dat gioi han di chuyen
        minX = -horzExtent + objectWidth;
        maxX = horzExtent - objectWidth;
        minY = -vertExtent + objectHeight;
        maxY = vertExtent - objectHeight;
    }

    public Vector3 ClampPosition(Vector3 targetPosition)
    {
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        return targetPosition;
    }
}
