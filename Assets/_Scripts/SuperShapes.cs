using UnityEditor;
using UnityEngine;
using static UnityEngine.Mathf;

public class SuperShapes : MonoBehaviour {
    private const float TAU = 2 * PI;

    [SerializeField, Min(10)] private int resolution;
    [SerializeField, Min(1f)] private float scale;
    [SerializeField] private float m;
    [SerializeField] private float n1, n2, n3;

    private float a = 1f, b = 1f;

    private float SuperShape(float theta) {
        float eq1 = Pow(Abs((1 / a) * Cos(theta * m / 4)), n2);
        float eq2 = Pow(Abs((1 / b) * Sin(theta * m / 4)), n3);

        return 1 / Pow(eq1 + eq2, 1 / n1);
    }

    private void OnDrawGizmos() {
        float increment = TAU / resolution;

        Vector2 prevPoint = Vector2.zero;

        for (int i = 0; i <= resolution; i++) {
            float theta = i * increment;
            float value = SuperShape(theta);
            Vector2 newPoint = new Vector2(Cos(theta), Sin(theta)) * value * scale;

            if (i <= 0) {
                prevPoint = newPoint;
                continue;
            }

            Handles.DrawAAPolyLine(prevPoint, newPoint);

            prevPoint = newPoint;
        }
    }
}