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

    // imageName -> spawned instance (spawn once, stays)
    private readonly Dictionary<string, GameObject> spawnedByImage = new Dictionary<string, GameObject>();

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

        // removed → do nothing (object stays)
    }

    private void TrySpawn(ARTrackedImage trackedImage)
    {
        if (trackedImage == null) return;
        if (trackedImage.trackingState != TrackingState.Tracking) return;

        string imageName = trackedImage.referenceImage.name;

        // Already spawned → don't move it
        if (spawnedByImage.ContainsKey(imageName)) return;

        if (!indexByImageName.TryGetValue(imageName, out int index))
        {
            Debug.LogWarning($"No prefab found for image '{imageName}'. " +
                             $"Prefab name must match image name.");
            return;
        }

        GameObject prefab = placeablePrefabs[index];
        GameObject instance = Instantiate(prefab);
        instance.name = prefab.name + "_Spawned";

        // Base pose = image pose
        instance.transform.position = trackedImage.transform.position;
        instance.transform.rotation = trackedImage.transform.rotation;

        // Position offset (relative to image orientation)
        if (index < spawnPositions.Length)
            instance.transform.position += trackedImage.transform.TransformDirection(spawnPositions[index]);

        // Rotation offset
        if (index < spawnRotations.Length)
            instance.transform.rotation *= Quaternion.Euler(spawnRotations[index]);

        // Scale
        if (index < spawnScales.Length)
            instance.transform.localScale = spawnScales[index];

        spawnedByImage.Add(imageName, instance);
    }
}
