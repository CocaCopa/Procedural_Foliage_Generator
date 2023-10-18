#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
public class ProceduralSpawnerGizmos : MonoBehaviour {

    [SerializeField] private Color gizmosColor = Color.white;
    private BoxCollider boxCollider;

    private void OnDrawGizmos() {
        GetBoxCollider();
        DrawBoxColliderGizmo();
    }

    private void Update() {
        KeepColliderCentered();
        DontChangeTheScale();
    }

    private void GetBoxCollider() {
        if (boxCollider == null) {
            boxCollider = GetComponent<BoxCollider>();
        }
    }

    private void DrawBoxColliderGizmo() {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
    }

    private void KeepColliderCentered() {
        if (boxCollider != null) {
            if (boxCollider.center != Vector3.zero) {
                boxCollider.center = Vector3.zero;
            }
        }
    }

    private void DontChangeTheScale() {
        if (boxCollider != null) {
            if (transform.localScale != Vector3.one) {
                transform.localScale = Vector3.one;
                Debug.LogWarning("To alter the size of the spawn area, adjust the box collider's dimensions.");
            }
        }
    }
}
#endif
