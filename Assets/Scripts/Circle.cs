using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public float radius;
    public Vector3 center;
    public float angleIncrement;

    SpriteRenderer spriteRenderer;
    bool displaySprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRadius(radius);
        transform.position = center;
    }

    public void Iterate()
    {
        transform.Rotate(0, 0, angleIncrement);
    }

    public void SetRadius(float radius)
    {
        this.radius = radius;
        transform.localScale = new Vector3(radius, radius, 0);
    }

    public void SetCenter(Vector3 center)
    {
        this.center = center;
        transform.position = center;
    }

    public void SpriteVisible(bool condition)
    {
        spriteRenderer.enabled = condition;
    }
}
