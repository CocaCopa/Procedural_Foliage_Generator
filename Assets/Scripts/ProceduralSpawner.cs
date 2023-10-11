#if UNITY_EDITOR
using System.Collections.Generic;
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
    [SerializeField] public string foliageHolderName;
    [Tooltip("Layer for the foliage GameObjects")]
    [SerializeField] public string foliageLayer;
    [Tooltip("Tag for the foliage GameObjects")]
    [SerializeField] public string foliageTag;

    [Header("--- Spawner Settings ---")]
    [SerializeField] private LayerMask spawnOnLayer;
    [Tooltip("Controls the density of foliage in the specified area")]
    [SerializeField] private int intensity;
    [Tooltip("Cut out 'x' meters of the specified area on the X axis")]
    [SerializeField] private float marginX;
    [Tooltip("Cut out 'x' meters of the specified area on the Z axis")]
    [SerializeField] private float marginZ;

    [Header("--- Parent Folliage ---")]
    [Tooltip("Minimum distance from each parent")]
    [SerializeField] public float minimumDistance;
    [Tooltip("Maximum distance from each parent")]
    [SerializeField] public float maximumDistance;
    [Tooltip("The spread distance of children foliage around their parent")]
    [SerializeField] public float spreadDistance;

    [Header("--- Child Folliage ---")]
    [SerializeField] int numberOfChildren;
    [Tooltip("Minimum distance of children foliage from their parent")]
    [SerializeField] public float distanceFromParent;
    [Tooltip("Should the child keep the desired 'distanceFromParent' from all the generated parents or only its own parent?")]
    [SerializeField] private ParentDistanceMode parentDistanceMode;
    [Tooltip("Minimum distance of children foliage from each other")]
    [SerializeField] public float minDistance;
    [Tooltip("Maximum distance of children foliage from each other")]
    [SerializeField] public float maxDistance;
    [Tooltip("Minimum scale of children foliage")]
    [SerializeField] private float minScale;
    [Tooltip("Maximum scale of children foliage")]
    [SerializeField] private float maxScale;

    private enum ParentDistanceMode { AllParents, MyParent };
    private float boxLimitsX;
    private float boxLimitsZ;
    private float raycastDistance;

    public void GenerateFolliage() {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        raycastDistance = boxCollider.bounds.size.y;
        List<GameObject> generatedParents = new();
        boxCollider.enabled = false;
        boxLimitsX = boxCollider.size.x - marginX;
        boxLimitsZ = boxCollider.size.z - marginZ;

        if (primitives.Length > 0) {
            GenerateParents(ref generatedParents);
        }
        if (primitives.Length > 0 && children.Length > 0) {
            GenerateChildren(generatedParents);
        }
        boxCollider.enabled = true;
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

            Vector3 parentPosition = RandomVectorPointInCircle(transform.position, circleRadius);
            
            if (OutOfBounds(parentPosition)) {
                continue;
            }

            if (ShootRayToPosition(out RaycastHit hit, parentPosition, ~spawnOnLayer)) {
                continue;
            }

            parentPosition.y = CorrectPositionHeight(parentPosition);
            if (parentPosition.y != Mathf.PI) {
                int randomParentIndex = Random.Range(0, primitives.Length);
                GameObject m_obj = Instantiate(primitives[randomParentIndex]);
                SetObjectValues(m_obj, parent, parentPosition, Vector2.one, new(minimumDistance, maximumDistance));
                generatedParents.Add(m_obj);
            }
            //break;
        }

        //return;

        for (int i = 0; i < generatedParents.Count; i++) {
            DestroyImmediate(generatedParents[i].GetComponent<SphereCollider>());
        }
    }

    private void GenerateChildren(List<GameObject> generatedParents) {
        List<GameObject> childrenList = new();
        for (int i = 0; i < generatedParents.Count; i++) {

            GameObject m_obj = generatedParents[i];
            Vector3 parentPosition = generatedParents[i].transform.position;

            for (int j = 0; j < numberOfChildren; j++) {

                Vector3 childPosition = RandomVectorPointInCircle(parentPosition, spreadDistance);
                
                if (OutOfBounds(childPosition)) {
                    continue;
                }

                switch (parentDistanceMode) {
                    case ParentDistanceMode.AllParents:
                    if (TooCloseToParent(generatedParents, childPosition)) {
                        continue;
                    }
                    break;
                    case ParentDistanceMode.MyParent:
                    if (Vector3.Distance(childPosition, parentPosition) <= distanceFromParent) {
                        continue;
                    }
                    break;
                }
                

                if (ShootRayToPosition(out RaycastHit hit, childPosition, ~spawnOnLayer)) {
                    continue;
                }

                childPosition.y = CorrectPositionHeight(childPosition);
                if (childPosition.y != Mathf.PI) {
                    int randomChildIndex = Random.Range(0, children.Length);
                    GameObject m_child = Instantiate(children[randomChildIndex]);
                    SetObjectValues(m_child, m_obj, childPosition, new(minScale, maxScale), new(minDistance, maxDistance));
                    childrenList.Add(m_child);
                }
            }
        }
        
        for (int i = 0; i < childrenList.Count; i++) {
            DestroyImmediate(childrenList[i].GetComponent<SphereCollider>());
        }
    }

    private bool TooCloseToParent(List<GameObject> generatedParents, Vector3 position) {

        bool test = false;
        for (int i = 0; i < generatedParents.Count; i++) {
            if (Vector3.Distance(generatedParents[i].transform.position, position) <= distanceFromParent) {
                test = true;
                break;
            }
        }
        return test;
    }

    public void DestroyAll() {
        GameObject foliageHolder = GameObject.Find(foliageHolderName);
        DestroyImmediate(foliageHolder);
    }

    private bool OutOfBounds(Vector3 point) {
        Vector3 pointWorldToLocal = transform.InverseTransformPoint(point);
        bool outOfBoundsUp    = pointWorldToLocal.z - boxLimitsZ / 2 > 0;
        bool outOfBoundsDown  = pointWorldToLocal.z + boxLimitsZ / 2 < 0;
        bool outOfBoundsRight = pointWorldToLocal.x - boxLimitsX / 2 > 0;
        bool outOfBoundsLeft  = pointWorldToLocal.x + boxLimitsX / 2 < 0;

        return outOfBoundsUp || outOfBoundsDown || outOfBoundsRight || outOfBoundsLeft;
    }

    private bool ShootRayToPosition(out RaycastHit hit, Vector3 checkPosition, int layer) {
        checkPosition.y = transform.position.y;
        Ray ray = new(checkPosition + 1000 * Vector3.up * raycastDistance / 2, Vector3.down);
        return Physics.Raycast(ray, out hit, 1000 * raycastDistance, layer);
    }

    private float CorrectPositionHeight(Vector3 checkPosition) {
        ShootRayToPosition(out RaycastHit hit, checkPosition, spawnOnLayer);
        if (hit.transform == null) {
            Debug.LogWarning("Please make sure that the box collider has a valid ground below it.");
            return Mathf.PI;
        }
        else {
            return hit.point.y;
        }
    }

    private void SetObjectValues(GameObject childObj, GameObject parentObj, Vector3 position, Vector2 scaleRange, Vector2 distanceRange) {

        childObj.transform.parent = parentObj.transform;
        childObj.transform.position = position;
        childObj.transform.localScale = childObj.transform.localScale.x * Random.Range(scaleRange.x, scaleRange.y) * Vector3.one;
        SphereCollider childSphere = childObj.AddComponent<SphereCollider>();
        childSphere.radius = Random.Range(distanceRange.x, distanceRange.y);
        childObj.transform.tag = foliageTag;
        childObj.layer = LayerMask.NameToLayer(foliageLayer);
    }

    private Vector3 RandomVectorPointInCircle(Vector3 center, float radius) {

        Vector3 direction = transform.forward;
        var vector2 = Random.insideUnitCircle * radius;

        float signedAngle = Vector3.SignedAngle(direction, vector2, Vector3.up);
        if (signedAngle > 90 || signedAngle < -90) {
            vector2 = -vector2;
        }

        return new Vector3(vector2.x, 0, vector2.y) + center;
    }
}
#endif
