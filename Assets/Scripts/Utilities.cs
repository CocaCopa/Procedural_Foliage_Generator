using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {
    /*private static Vector3 RandomVectorPointInCircle(Vector3 center, float radius, Vector3 forwardDirection) {
        Vector2 vector2 = Random.insideUnitCircle * radius;
        float signedAngle = Vector3.SignedAngle(forwardDirection, vector2, Vector3.up);
        if (signedAngle > 90 || signedAngle < -90) {
            vector2 = -vector2;
        }
        return new Vector3(vector2.x, 0, vector2.y) + center;
    }

    private static bool OutOfBounds(Vector3 point, float limitsX, float limitsZ) {
        Vector3 pointWorldToLocal = transform.InverseTransformPoint(point);
        bool outOfBoundsUp    = pointWorldToLocal.z - limitsZ / 2 > 0;
        bool outOfBoundsDown  = pointWorldToLocal.z + limitsZ / 2 < 0;
        bool outOfBoundsRight = pointWorldToLocal.x - limitsX / 2 > 0;
        bool outOfBoundsLeft  = pointWorldToLocal.x + limitsX / 2 < 0;
        return outOfBoundsUp || outOfBoundsDown || outOfBoundsRight || outOfBoundsLeft;
    }

    private static bool IsPositionCloseToAnyPosition(Vector3 position, List<Vector3> positions, float distance) {
        bool tooClose = false;
        for (int i = 0; i < positions.Count; i++) {
            if (Vector3.Distance(positions[i], position) <= distance) {
                tooClose = true;
                break;
            }
        }
        return tooClose;
    }*/
}
