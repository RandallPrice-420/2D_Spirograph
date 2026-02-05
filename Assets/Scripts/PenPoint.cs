using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenPoint : MonoBehaviour
{
    public Vector3 center;
    SpriteRenderer spriteRenderer;
    bool displaySprite;
    LineRenderer lineRenderer;
    Vector3[] positionArray;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetPositionArray(int size)
    {
        positionArray = new Vector3[size];
    }

    public void StorePoint(int index)
    {
        Vector3 newPosition = transform.position;
        positionArray[index] = newPosition;
    }

    public void SetCenter(Vector3 center)
    {
        this.center = center;
        transform.position = center;
    }

    public void DrawCurve(int count)
    {
        lineRenderer.positionCount = count;
        lineRenderer.SetPositions(positionArray);
    }

    public void ResetCurve()
    {
        positionArray = new Vector3[0];
        DrawCurve(positionArray.Length);
    }

    public void SpriteVisible(bool condition)
    {
        spriteRenderer.enabled = condition;
    }
}
