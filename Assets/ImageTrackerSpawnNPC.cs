using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackerSpawnNPC : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;

    [Header("NPC Prefab (will spawn once per image)")]
    public GameObject npcPrefab;

    [Header("Spawn Offset / Rotation / Scale")]
    public Vector3 positionOffset = Vector3.zero;
    public Vector3 rotationOffsetEuler = Vector3.zero;
    public Vector3 scale = Vector3.one;

    // TrackableId -> spawned NPC
    private Dictionary<TrackableId, GameObject> spawnedNPCs = new Dictionary<TrackableId, GameObject>();

    private void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackablesChanged.AddListener(OnImagesChanged);
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackablesChanged.RemoveListener(OnImagesChanged);
    }

    private void OnImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var img in args.added)
            TrySpawn(img);

        foreach (var img in args.updated)
            TrySpawn(img);
    }

    private void TrySpawn(ARTrackedImage img)
    {
        if (img == null) return;

        // only spawn when actually tracking
        if (img.trackingState != TrackingState.Tracking)
            return;

        // already spawned for this tracked image instance
        if (spawnedNPCs.ContainsKey(img.trackableId))
            return;

        if (npcPrefab == null)
        {
            Debug.LogError("npcPrefab is NULL! Drag your NPC prefab into ImageTrackerSpawnNPC.");
            return;
        }

        // spawn at image pose (world)
        Vector3 pos = img.transform.position + img.transform.TransformVector(positionOffset);
        Quaternion rot = img.transform.rotation * Quaternion.Euler(rotationOffsetEuler);

        GameObject npc = Instantiate(npcPrefab, pos, rot);
        npc.transform.localScale = scale;

        spawnedNPCs.Add(img.trackableId, npc);

        Debug.Log($"Spawned NPC for image: {img.referenceImage.name}");
    }
}
