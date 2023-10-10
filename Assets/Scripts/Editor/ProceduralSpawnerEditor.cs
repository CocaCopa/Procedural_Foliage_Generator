using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralSpawner))]
public class ProceduralSpawnerEditor : Editor {

    private ProceduralSpawner spawner;
    private BoxCollider boxCollider;

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        if (spawner == null) {
            spawner = target as ProceduralSpawner;
        }

        Buttons();
        BoxColliderKeepEnabled();
        DefaultInformation();
        if (spawner.distanceFromParent > spawner.spreadDistance) {
            spawner.distanceFromParent = spawner.spreadDistance - 0.01f;
            Debug.LogWarning("Distance from parent cannot be more than the spread distance");
        }
    }

    private void Buttons() {
        GUILayout.Space(10);
        if (GUILayout.Button("Generate")) {
            spawner.GenerateFolliage();
        }
        if (GUILayout.Button("Delete")) {
            spawner.DestroyAll();
        }
    }

    private void BoxColliderKeepEnabled() {
        if (boxCollider == null) {
            boxCollider = spawner.GetComponent<BoxCollider>();
        }
        if (!boxCollider.enabled) {
            boxCollider.enabled = true;
        }
    }

    private void DefaultInformation() {
        if (spawner.foliageHolderName == string.Empty)
            spawner.foliageHolderName = "FoliageHolder";
        if (spawner.foliageLayer == string.Empty || spawner.foliageLayer.Contains(" "))
            spawner.foliageLayer = "Default";
        if (spawner.foliageTag == string.Empty || spawner.foliageTag.Contains(" "))
            spawner.foliageTag = "Untagged";
    }
}
