#if UNITY_EDITOR
using UnityEngine;

public static class UtilGizmo
{
    public enum DrawingAxis { XY, XZ, YZ };

    #region Methods
    public static void DrawCircle(Transform transform, float radius, int segments)
    {
        DrawField(transform, radius, segments, 360f);
    }

    public static void DrawField(Transform transform, float radius, int segments, float sweepAngle)
    {
        Vector3 center = transform.position;
        float startAngle = -sweepAngle / 2f;
        float endAngle = sweepAngle / 2f;

        Vector3 startDirection = Quaternion.Euler(0, startAngle, 0) * transform.forward;
        Vector3 endDirection = Quaternion.Euler(0, endAngle, 0) * transform.forward;

        Vector3 previousPoint = center + startDirection * radius;

        if (sweepAngle != 360)
        {
            Gizmos.DrawLine(center, previousPoint);
            Gizmos.DrawLine(center, center + endDirection * radius);
        }

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Lerp(startAngle, endAngle, i / (float)segments);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            Vector3 newPoint = center + direction * radius;
            Gizmos.DrawLine(previousPoint, newPoint);
            previousPoint = newPoint;
        }
    }

    public static void DrawRect(Vector3 position, Vector2 size, DrawingAxis drawingAxis)
    {

        float length = size.x / 2f;
        float width = size.y / 2f;

        Vector3 topLeft = new Vector3(position.x, position.y, position.z);
        Vector3 topRight = new Vector3(position.x, position.y, position.z);
        Vector3 bottomRight = new Vector3(position.x, position.y, position.z);
        Vector3 bottomLeft = new Vector3(position.x, position.y, position.z);

        switch (drawingAxis)
        {
            case DrawingAxis.XY:
                topLeft.x -= length; topLeft.y += width;
                topRight.x += length; topRight.y += width;
                bottomRight.x += length; bottomRight.y -= width;
                bottomLeft.x -= length; bottomLeft.y -= width;
                break;
            case DrawingAxis.XZ:
                topLeft.x -= length; topLeft.z += width;
                topRight.x += length; topRight.z += width;
                bottomRight.x += length; bottomRight.z -= width;
                bottomLeft.x -= length; bottomLeft.z -= width;
                break;
            case DrawingAxis.YZ:
                topLeft.y -= length; topLeft.z += width;
                topRight.y += length; topRight.z += width;
                bottomRight.y += length; bottomRight.z -= width;
                bottomLeft.y -= length; bottomLeft.z -= width;
                break;
        }

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }


    #endregion
}
#endif