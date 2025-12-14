using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;

    [Header("Placeable Prefabs (must match image names)")]
    [SerializeField] private GameObject[] placeablePrefabs;

    [Header("Adjust Position Offset (meters, same order as prefabs)")]
    [SerializeField] private Vector3[] spawnPositions;

    [Header("Adjust Direction (Euler, same order as prefabs)")]
    [SerializeField] private Vector3[] spawnRotations;

    [Header("Adjust Size (same order as prefabs)")]
    [SerializeField] private Vector3[] spawnScales;

    // ✅ TrackableId -> spawned instance (one object per tracked physical image)
    private readonly Dictionary<TrackableId, GameObject> spawnedById = new Dictionary<TrackableId, GameObject>();

    // imageName -> prefab index
    private readonly Dictionary<string, int> indexByImageName = new Dictionary<string, int>();

    private void Awake()
    {
        if (trackedImageManager == null)
            trackedImageManager = GetComponent<ARTrackedImageManager>();

        indexByImageName.Clear();
        for (int i = 0; i < placeablePrefabs.Length; i++)
        {
            if (placeablePrefabs[i] == null) continue;
            // Prefab name MUST match reference image name
            indexByImageName[placeablePrefabs[i].name] = i;
        }
    }

    private void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnImagesChanged;
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnImagesChanged;
    }

    private void OnImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var img in eventArgs.added)
            TrySpawn(img);

        foreach (var img in eventArgs.updated)
            TrySpawn(img);

        // removed -> optional: 清理字典记录（不删除生成的物体）
        foreach (var img in eventArgs.removed)
            spawnedById.Remove(img.trackableId);
    }

    private void TrySpawn(ARTrackedImage trackedImage)
    {
        if (trackedImage == null) return;
        if (trackedImage.trackingState != TrackingState.Tracking) return;

        // ✅ 同一张图的不同实体会有不同 trackableId
        TrackableId id = trackedImage.trackableId;

        // Already spawned for this physical image -> don't spawn again
        if (spawnedById.ContainsKey(id)) return;

        string imageName = trackedImage.referenceImage.name;

        if (!indexByImageName.TryGetValue(imageName, out int index))
        {
            Debug.LogWarning($"No prefab found for image '{imageName}'. Prefab name must match image name.");
            return;
        }

        GameObject prefab = placeablePrefabs[index];

        // Base pose = image pose
        Vector3 pos = trackedImage.transform.position;
        Quaternion rot = trackedImage.transform.rotation;

        // Position offset (relative to image orientation)
        if (index < spawnPositions.Length)
            pos += trackedImage.transform.TransformDirection(spawnPositions[index]);

        // Rotation offset
        if (index < spawnRotations.Length)
            rot *= Quaternion.Euler(spawnRotations[index]);

        GameObject instance = Instantiate(prefab, pos, rot);
        instance.name = prefab.name + "_Spawned_" + id;

        // Scale
        if (index < spawnScales.Length)
            instance.transform.localScale = spawnScales[index];

        spawnedById.Add(id, instance);
    }
}
