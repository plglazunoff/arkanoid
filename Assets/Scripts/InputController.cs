using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IDragHandler
{
    public Transform PlatformTransform;
    public GameObject MiddleBorder;
    public GameObject RightBorder;
    public float Sensitivity = 0.04f;

    private float minX;
    private float maxX;

    private IEnumerator WaitAndSet()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        SetBorders();
    }

    private void Start()
    {
        StartCoroutine(WaitAndSet());
    }

    private void SetBorders() 
    {
        RectTransform middleRect = MiddleBorder.GetComponent<RectTransform>();
        RectTransform rightRect = RightBorder.GetComponent<RectTransform>();

        Vector3[] middleCorners = new Vector3[4];
        middleRect.GetWorldCorners(middleCorners);

        Vector3[] rightCorners = new Vector3[4];
        rightRect.GetWorldCorners(rightCorners);

        float borderMiddleEdge = middleCorners[2].x;
        float borderRightEdge = rightCorners[0].x;

        var platformCollider = PlatformTransform.GetComponent<Collider2D>();
        float platformHalfWidth = platformCollider != null ? platformCollider.bounds.extents.x : 0.5f;

        minX = borderMiddleEdge + platformHalfWidth;
        maxX = borderRightEdge - platformHalfWidth;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float moveDelta = eventData.delta.x * Sensitivity;
        Vector3 currentPosition = PlatformTransform.position;
        float newPositionX = Mathf.Clamp(currentPosition.x + moveDelta, minX, maxX);
        PlatformTransform.position = new Vector3(newPositionX, currentPosition.y, currentPosition.z);
    }

    private void Update()
    {
        //Debug.DrawLine(new Vector3(minX, -10, 0), new Vector3(minX, 10, 0), Color.red);
        //Debug.DrawLine(new Vector3(maxX, -5, 0), new Vector3(maxX, 5, 0), Color.red);
    }
}
