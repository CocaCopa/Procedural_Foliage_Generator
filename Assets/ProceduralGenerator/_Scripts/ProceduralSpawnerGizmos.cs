#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
public class ProceduralSpawnerGizmos : MonoBehaviour {

    [SerializeField] private Color gizmosColor = Color.white;
    private BoxCollider boxCollider;

    [Header("--- Temp: DEBUG ---")]
    public bool logDistances = false;
    [Tooltip("Select an object in the scene and then click this boolean. After that, any other object you select " +
        "in the scene will lead to a console log, that prints the distance between the 2 objects")]
    public bool replaceInspectedObject = false;

    private GameObject gameObj_1;
    private GameObject gameObj_2;

    private void Update() {
        if (replaceInspectedObject && !logDistances) {
            logDistances = true;
        }
        if (!logDistances) {
            return;
        }
        TEMP_FUNC_DebugDistances();
    }

    private void TEMP_FUNC_DebugDistances() {
        if (replaceInspectedObject) {
            replaceInspectedObject = false;
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
