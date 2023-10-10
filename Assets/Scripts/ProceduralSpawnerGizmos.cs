#if UNITY_EDITOR
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ProceduralSpawnerGizmos : MonoBehaviour {

    [SerializeField] private Color gizmosColor = Color.white;
    private BoxCollider boxCollider;

    private void OnDrawGizmos() {
        if (boxCollider == null) {
            boxCollider = GetComponent<BoxCollider>();
        }
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
        KeepColliderCentered();
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
