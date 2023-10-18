#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralSpawner))]
public class ProceduralSpawnerEditor : Editor {

    private ProceduralSpawner spawner;
    private BoxCollider boxCollider;

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();
        FindTargetScript();
        Buttons();
        BoxColliderKeepEnabled();
        DefaultInformation();
        ManageValues();
        NonNegativeValues();
    }

    private void FindTargetScript() {
        if (spawner == null) {
            spawner = target as ProceduralSpawner;
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

    private void ManageValues() {
        if (spawner.distanceFromParent > spawner.spreadDistance) {
            spawner.distanceFromParent = spawner.spreadDistance;
            Debug.LogWarning("Distance from parent cannot be more than the spread distance");
            Debug.Log("It is recommended to leave a noticeable margin between these 2 values, or else there's a high chance no child foliage will spawn");
        }
        if (spawner.minChildBlankArea > spawner.maxChildBlankArea) {
            spawner.minChildBlankArea = spawner.maxChildBlankArea;
            Debug.LogWarning("Minimum distance cannot be more than the maximum distance");
        }
        if (spawner.minParentBlankArea > spawner.maxParentBlankArea) {
            spawner.minParentBlankArea = spawner.maxParentBlankArea;
            Debug.LogWarning("Minimum distance cannot be more than the maximum distance");
        }
    }

    private void NonNegativeValues() {
        if (spawner.intensity < 0) {
            spawner.intensity = 0;
        }
        if (spawner.marginX < 0) {
            spawner.marginX = 0;
        }
        if (spawner.marginZ < 0) {
            spawner.marginZ = 0;
        }
        if (spawner.minParentBlankArea < 0) {
            spawner.minParentBlankArea = 0;
        }
        if (spawner.maxParentBlankArea < 0) {
            spawner.maxParentBlankArea = 0;
        }
        if (spawner.spreadDistance < 0) {
            spawner.spreadDistance = 0;
        }
        if (spawner.minParentScale < 0) {
            spawner.minParentScale = 0;
        }
        if (spawner.maxParentScale < 0) {
            spawner.maxParentScale = 0;
        }
        if (spawner.numberOfChildren < 0) {
            spawner.numberOfChildren = 0;
        }
        if (spawner.distanceFromParent < 0) {
            spawner.distanceFromParent = 0;
        }
        if (spawner.minChildBlankArea < 0) {
            spawner.minChildBlankArea = 0;
        }
        if (spawner.maxChildBlankArea < 0) {
            spawner.maxChildBlankArea = 0;
        }
        if (spawner.minChildScale < 0) {
            spawner.minChildScale = 0;
        }
        if (spawner.maxChildScale < 0) {
            spawner.maxChildScale = 0;
        }
    }
}
#endif
