using UnityEditor;
using UnityEngine;

public class MarchingCubes : MonoBehaviour {
    [SerializeField, Min(1)] private int space;
    [SerializeField] private int width, height;

    private bool IsInitialized { get; set; }

    private int[,] anchorPointsWeight;

    private void Start() {
        int x = 1 + width / space;
        int y = 1 + height / space;
        anchorPointsWeight = new int[x, y];

        for (int i = 0; i < x; i++) {
            for (int ii = 0; ii < y; ii++) {
                anchorPointsWeight[i, ii] = Mathf.RoundToInt(Random.value);
            }
        }

        IsInitialized = true;
    }

    private void DrawArea() {
        Vector3 offsetVector = new Vector3((space * width * 0.5f).Negate(), space * height * 0.5f);

        Vector3 topLeftCorner = Vector3.zero + offsetVector;
        Vector3 topRightCorner = topLeftCorner + Vector3.right * space * width;
        Vector3 bottomRightCorner = topRightCorner + Vector3.down * space * height;
        Vector3 bottomLeftCorner = bottomRightCorner + Vector3.left * space * width;

        Handles.DrawAAPolyLine(topLeftCorner, topRightCorner);
        Handles.DrawAAPolyLine(topRightCorner, bottomRightCorner);
        Handles.DrawAAPolyLine(bottomRightCorner, bottomLeftCorner);
        Handles.DrawAAPolyLine(bottomLeftCorner, topLeftCorner);
    }

    private void DrawDots() {
        int x = 1 + width / space;
        int y = 1 + height / space;

        for (int i = 0; i < x; i++) {
            for (int ii = 0; ii < y; ii++) {
                DrawDot(i, ii);
            }
        }

        void DrawDot(int i, int ii) {
            const float discRadius = 0.1f;
            Color discColor = IsInitialized ? (anchorPointsWeight[i, ii] == 1 ? Color.green : Color.red) : Color.black;

            var start = new Vector3((space * width * 0.5f).Negate(), space * height * 0.5f);
            var position = start + new Vector3(i * space, -ii * space);

            Handles.color = discColor;
            Handles.DrawSolidDisc(position, Vector3.back, discRadius);
        }
    }

    private void DrawConnectedLines() {
        if (!IsInitialized) return;

        Vector3 offsetVector = new Vector3((space * width * 0.5f).Negate(), space * height * 0.5f);

        int x = width / space;
        int y = height / space;

        for (int i = 0; i < x; i++) {
            for (int ii = 0; ii < y; ii++) {
                int firstNode = anchorPointsWeight[i, ii];
                int secondNode = anchorPointsWeight[i + 1, ii];
                int thirdNode = anchorPointsWeight[i + 1, ii + 1];
                int forthNode = anchorPointsWeight[i, ii + 1];

                Vector3 a = offsetVector + new Vector3((i + 0.5f) * space, (ii * space).Negate());
                Vector3 b = offsetVector + new Vector3((i + 1f) * space, ((ii + 0.5f) * space).Negate());
                Vector3 c = offsetVector + new Vector3((i + 0.5f) * space, ((ii + 1f) * space).Negate());
                Vector3 d = offsetVector + new Vector3(i * space, ((ii + 0.5f) * space).Negate());

                int totalWeight = TotalWeight(firstNode, secondNode, thirdNode, forthNode);
                DrawLine(totalWeight, a, b, c, d);
            }
        }

        int TotalWeight(int firstNode, int secondNode, int thirdNode, int forthNode) {
            return 1 * firstNode + 2 * secondNode + 4 * thirdNode + 8 * forthNode;
        }

        void DrawLine(int calculatedWeight, Vector3 a, Vector3 b, Vector3 c, Vector3 d) {
            switch (calculatedWeight) {
                case 1 or 14: {
                    Handles.DrawAAPolyLine(a, d);
                    break;
                }
                case 2 or 13: {
                    Handles.DrawAAPolyLine(a, b);
                    break;
                }
                case 3 or 12: {
                    Handles.DrawAAPolyLine(b, d);
                    break;
                }
                case 4 or 11: {
                    Handles.DrawAAPolyLine(b, c);
                    break;
                }
                case 5: {
                    Handles.DrawAAPolyLine(a, b);
                    Handles.DrawAAPolyLine(c, d);
                    break;
                }
                case 6 or 9: {
                    Handles.DrawAAPolyLine(a, c);
                    break;
                }
                case 7 or 8: {
                    Handles.DrawAAPolyLine(c, d);
                    break;
                }
                case 10: {
                    Handles.DrawAAPolyLine(a, d);
                    Handles.DrawAAPolyLine(b, c);
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos() {
        DrawArea();
        DrawDots();
        DrawConnectedLines();
    }
}

public static class MathLibrary {
    public static float Negate(this float value) => -Mathf.Abs(value);

    public static int Negate(this int value) => -Mathf.Abs(value);
}