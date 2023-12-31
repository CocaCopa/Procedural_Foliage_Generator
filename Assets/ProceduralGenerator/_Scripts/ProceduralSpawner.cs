#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(ProceduralSpawnerGizmos))]
public class ProceduralSpawner : MonoBehaviour {

    [Header("--- Prefabs ---")]
    [Tooltip("Prefab to use as parent")]
    [SerializeField] private GameObject[] primitives;
    [Tooltip("Prefab to use as child")]
    [SerializeField] private GameObject[] children;

    [Header("--- Information ---")]
    [Tooltip("Name of the parent GameObject which it will hold all of the generated foliage")]
    public string foliageHolderName;
    [Tooltip("The given layer will be assigned to every generated GameObject")]
    public string foliageLayer;
    [Tooltip("The given layer will be assigned to every generated GameObject")]
    public string foliageTag;

    [Header("--- Spawner General Settings ---")]
    [SerializeField] private LayerMask spawnOnLayer;
    [Tooltip("Controls the density of foliage in the specified area")]
    public int intensity;
    [Tooltip("Cut out 'x' meters of the specified area on the X axis")]
    public float marginX;
    [Tooltip("Cut out 'x' meters of the specified area on the Z axis")]
    public float marginZ;

    [Header("--- Parent Folliage ---")]
    [Tooltip("Controls how far a parent should spread new seeds that will generate children")]
    public float spreadDistance;
    [Space(5)]
    [Tooltip("Distance at which new parents cannot spawn near existing parents.")]
    public float minParentBlankArea;
    [Tooltip("Distance at which new parents cannot spawn near existing parents.")]
    public float maxParentBlankArea;
    [Space(5)]
    [Tooltip("Minimum scale of parent foliage")]
    public float minParentScale;
    [Tooltip("Maximum scale of parent foliage")]
    public float maxParentScale;

    [Header("--- Child Folliage ---")]
    [Tooltip("Maximum number of children a parent can spawn")]
    public int numberOfChildren;
    [Space(5)]
    [Tooltip("Should the child keep the desired 'distanceFromParent' from all of the generated parents or only from its own parent?")]
    [SerializeField] private ParentDistanceMode parentDistanceMode;
    [Tooltip("Minimum distance of children foliage from their parent")]
    public float distanceFromParent;
    [Space(5)]
    [Tooltip("Distance at which new children cannot spawn near existing children.")]
    public float minChildBlankArea;
    [Tooltip("Distance at which new children cannot spawn near existing children.")]
    public float maxChildBlankArea;
    [Space(5)]
    [Tooltip("Minimum scale of children foliage")]
    public float minChildScale;
    [Tooltip("Maximum scale of children foliage")]
    public float maxChildScale;

    private enum ParentDistanceMode { AllParents, OwnParent };
    private float boxLimitsX;
    private float boxLimitsZ;
    private float raycastDistance;

    private List<float> radiiOfParents = new();
    private List<float> radiiOfChildren = new();
    private List<Vector3> positionsOfParents = new();
    private List<Vector3> positionsOfChildren = new();

    public void GenerateFolliage() {
        ClearLists();
        Initialize(out var boxCollider);
        boxCollider.enabled = false;
        Generate();
        boxCollider.enabled = true;
    }

    private void ClearLists() {
        radiiOfParents.Clear();
        radiiOfChildren.Clear();
        positionsOfParents.Clear();
        positionsOfChildren.Clear();
    }

    private void Initialize(out BoxCollider boxCollider) {
        boxCollider = GetComponent<BoxCollider>();
        raycastDistance = boxCollider.bounds.size.y;
        boxLimitsX = boxCollider.size.x - marginX;
        boxLimitsZ = boxCollider.size.z - marginZ;
    }

    private void Generate() {
        // This list is used to make every generated child, a child object of its generated parent object.
        List<GameObject> generatedParents = new();
        if (!primitives.Any()) {
            return;
        }
        GenerateParents(ref generatedParents);
        if (!children.Any() || numberOfChildren == 0) {
            return;
        }
        GenerateChildren(generatedParents);
    }

    private void GenerateParents(ref List<GameObject> generatedParents) {
        GameObject parent = new(foliageHolderName) {
            tag = foliageTag,
            layer = LayerMask.NameToLayer(foliageLayer)
        };
        Vector3 centerPoint = transform.position;
        Vector3 edgeRight = transform.right * boxLimitsX / 2;
        Vector3 edgeUpwards = transform.forward * boxLimitsZ / 2;
        Vector3 colliderEdge = centerPoint + edgeRight + edgeUpwards;
        float circleRadius = (colliderEdge - transform.position).magnitude;

        for (int i = 0; i < intensity; i++) {

            Vector3 parentPosition = Utilities.RandomVectorPointInCircle(transform.position, circleRadius, transform.forward);
            
            if (Utilities.OutOfBounds(transform, parentPosition, boxLimitsX, boxLimitsZ)) {
                continue;
            }
            if (InvalidPosition(parentPosition)) {
                continue;
            }

            parentPosition.y = CorrectPositionHeight(parentPosition);

            if (parentPosition.y == Mathf.PI || Utilities.IsPositionCloseToAnyPosition(parentPosition, positionsOfParents, radiiOfParents)) {
                continue;
            }

            int randomParentIndex = Random.Range(0, primitives.Length);
            GameObject m_obj = Instantiate(primitives[randomParentIndex]);
            SetObjectValues(m_obj, parent, parentPosition, new(minParentScale, maxParentScale));
            positionsOfParents.Add(parentPosition);
            radiiOfParents.Add(Random.Range(minParentBlankArea, maxParentBlankArea));
            generatedParents.Add(m_obj);
        }
    }

    private void GenerateChildren(List<GameObject> generatedParents) {
        for (int i = 0; i < positionsOfParents.Count; i++) {

            GameObject m_obj = generatedParents[i];

            for (int j = 0; j < numberOfChildren; j++) {

                Vector3 childPosition = Utilities.RandomVectorPointInCircle(positionsOfParents[i], spreadDistance, transform.forward);

                if (Utilities.OutOfBounds(transform, childPosition, boxLimitsX, boxLimitsZ)) {
                    continue;
                }
                if (InvalidPosition(childPosition)) {
                    continue;
                }
                if (ChildCloseToParent(childPosition, positionsOfParents[i])) {
                    continue;
                }
                
                childPosition.y = CorrectPositionHeight(childPosition);

                if (childPosition.y == Mathf.PI || Utilities.IsPositionCloseToAnyPosition(childPosition, positionsOfChildren, radiiOfChildren)) {
                    continue;
                }

                int randomChildIndex = Random.Range(0, children.Length);
                GameObject m_child = Instantiate(children[randomChildIndex]);
                SetObjectValues(m_child, m_obj, childPosition, new(minChildScale, maxChildScale));
                positionsOfChildren.Add(childPosition);
                radiiOfChildren.Add(Random.Range(minChildBlankArea, maxChildBlankArea));
            }
        }
    }

    private bool ChildCloseToParent(Vector3 childPosition, Vector3 parentPosition) {
        bool tooClose = false;
        switch (parentDistanceMode) {
            case ParentDistanceMode.AllParents:
            tooClose = Utilities.IsPositionCloseToAnyPosition(childPosition, positionsOfParents, distanceFromParent);
            break;
            case ParentDistanceMode.OwnParent:
            tooClose = Vector3.Distance(childPosition, parentPosition) <= distanceFromParent;
            break;
        }
        return tooClose;
    }

    public void DestroyAll() {
        GameObject foliageHolder = GameObject.Find(foliageHolderName);
        DestroyImmediate(foliageHolder);
    }

    private float CorrectPositionHeight(Vector3 checkPosition) {
        ShootRayToPosition(out RaycastHit hit, checkPosition, raycastDistance, spawnOnLayer);
        return hit.transform == null ? Mathf.PI : hit.point.y;
    }

    private void SetObjectValues(GameObject childObj, GameObject parentObj, Vector3 position, Vector2 scaleRange) {
        childObj.transform.parent = parentObj.transform;
        childObj.transform.position = position;
        childObj.transform.localScale = childObj.transform.localScale.x * Random.Range(scaleRange.x, scaleRange.y) * Vector3.one;
        childObj.transform.tag = foliageTag;
        childObj.layer = LayerMask.NameToLayer(foliageLayer);
    }

    private bool InvalidPosition(Vector3 position) {
        if (ShootRayToPosition(out RaycastHit invalidHit, position, raycastDistance, ~spawnOnLayer)) {
            if (ShootRayToPosition(out RaycastHit validHit, position, raycastDistance, spawnOnLayer)) {
                if (invalidHit.point.y > validHit.point.y) {
                    return true;
                }
            }
            else
                return true;
        }
        return false;
    }

    private bool ShootRayToPosition(out RaycastHit hit, Vector3 position, float distance, int layer) {
        position.y = transform.position.y;
        Ray ray = new(position + Vector3.up * distance / 2, Vector3.down);
        return Physics.Raycast(ray, out hit, distance, layer);
    }
}
#endif
