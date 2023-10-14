#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
public class ProceduralSpawnerGizmos : MonoBehaviour {

    [SerializeField] private Color gizmosColor = Color.white;
    private BoxCollider boxCollider;

    public bool t = false;
    public bool s = false;

    private GameObject gameObj_1;
    private GameObject gameObj_2;

    private void Update() {
        if (!t) {
            return;
        }

        if (s) {
            s = false;
            gameObj_1 = gameObj_2 = null;
            return;
        }

        if (gameObj_1 != null && gameObj_1 != Selection.activeObject) {
            gameObj_2 = Selection.activeGameObject;
            Debug.Log(Vector3.Distance(gameObj_1.transform.position, gameObj_2.transform.position));
            return;
        }
        gameObj_1 = Selection.activeGameObject;
    }

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
