using UnityEditor;
using UnityEngine;

public class MaurerRose : MonoBehaviour {
    [SerializeField] private int n, d;
    [SerializeField, Min(1f)] private float scale;

    private void OnDrawGizmos() {
        Vector2 prevPoint = Vector2.zero;

        Handles.color = new Color(1f, 1f, 1f, 0.4f);
        for (int i = 0; i <= 360; i++) {
            var k = i * d * Mathf.Deg2Rad;
            var r = scale * Mathf.Sin(n * k);

            Vector2 destination = new Vector2(Mathf.Cos(k), Mathf.Sin(k)) * r;
            Handles.DrawAAPolyLine(prevPoint, destination);

            prevPoint = destination;
        }

        prevPoint = Vector2.zero;

        Handles.color = new Color(1f, 0f, 0f, 0.8f);
        for (int i = 0; i <= 360; i++) {
            var k = i * Mathf.Deg2Rad;
            var r = scale * Mathf.Sin(n * k);

            Vector2 destination = new Vector2(Mathf.Cos(k), Mathf.Sin(k)) * r;
            Handles.DrawAAPolyLine(width: 8f, prevPoint, destination);

            prevPoint = destination;
        }
    }
}