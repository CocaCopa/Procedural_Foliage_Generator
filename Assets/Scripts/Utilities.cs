using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {
    /// <summary>
    /// Generate a random Vector3 inside the given circle
    /// </summary>
    /// <param name="center">Center of the circle</param>
    /// <param name="radius">Radius of the circle</param>
    /// <param name="forwardDirection">Orientation of the circle</param>
    /// <returns>The generated Vector3</returns>
    public static Vector3 RandomVectorPointInCircle(Vector3 center, float radius, Vector3 forwardDirection) {
        var vector2 = Random.insideUnitCircle * radius;
        float signedAngle = Vector3.SignedAngle(forwardDirection, vector2, Vector3.up);
        if (signedAngle > 90 || signedAngle < -90) {
            vector2 = -vector2;
        }
        return new Vector3(vector2.x, 0, vector2.y) + center;
    }
    
    /// <summary>
    /// Checks of the given Vector3 is inside the given box
    /// </summary>
    /// <param name="transform">Transfom of the object calling the function</param>
    /// <param name="point">Position to check</param>
    /// <param name="limitsX">Box size X</param>
    /// <param name="limitsZ">Box size Z</param>
    /// <returns></returns>
    public static bool OutOfBounds(Transform transform, Vector3 point, float limitsX, float limitsZ) {
        Vector3 pointWorldToLocal = transform.InverseTransformPoint(point);
        bool outOfBoundsUp    = pointWorldToLocal.z - limitsZ / 2 > 0;
        bool outOfBoundsDown  = pointWorldToLocal.z + limitsZ / 2 < 0;
        bool outOfBoundsRight = pointWorldToLocal.x - limitsX / 2 > 0;
        bool outOfBoundsLeft  = pointWorldToLocal.x + limitsX / 2 < 0;
        return outOfBoundsUp || outOfBoundsDown || outOfBoundsRight || outOfBoundsLeft;
    }

    /// <summary>
    /// Checks if the given position is close to any of the positions in a list
    /// </summary>
    /// <param name="position">Position to check</param>
    /// <param name="positions">List of positions</param>
    /// <param name="distance">Distance</param>
    /// <returns>True, if the distance is less than the specified 'distance'</returns>
    public static bool IsPositionCloseToAnyPosition(Vector3 position, List<Vector3> positions, float distance) {
        bool tooClose = false;
        for (int i = 0; i < positions.Count; i++) {
            if (Vector3.Distance(position, positions[i]) <= distance) {
                tooClose = true;
                break;
            }
        }
        return tooClose;
    }

    /// <summary>
    /// Checks if the given position is close to any of the positions in a list
    /// </summary>
    /// <param name="position">Position to check</param>
    /// <param name="positions">List of positions</param>
    /// <param name="distances">List of distances</param>
    /// <returns>True, if the distance is less than the specified 'distances'</returns>
    public static bool IsPositionCloseToAnyPosition(Vector3 position, List<Vector3> positions, List<float> distances) {
        bool tooClose = false;
        for (int i = 0; i < positions.Count; i++) {
            if (Vector3.Distance(position, positions[i]) <= distances[i]) {
                tooClose = true;
                break;
            }
        }
        return tooClose;
    }
}
