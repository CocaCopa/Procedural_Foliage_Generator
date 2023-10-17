#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
public class ProceduralSpawnerGizmos : MonoBehaviour {

    [SerializeField] private Color gizmosColor = Color.white;
    private BoxCollider boxCollider;

    private GameObject gameObj_1;
    private GameObject gameObj_2;

    private void OnDrawGizmos() {
        GetBoxCollider();
        DrawBoxColliderGizmo();
        KeepColliderCentered();
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
}
#endif
