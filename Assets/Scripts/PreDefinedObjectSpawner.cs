using System.Collections;
using Google.XR.ARCoreExtensions;
using UnityEngine;
using Zenject;

public class PreDefinedObjectSpawner : MonoBehaviour
{
    [SerializeField] private AnchorListSO anchorListSO;
    [SerializeField] private GameObject spawnPrefab;

    [Inject] private GeospatialObjectManager _geospatialManager;

    private IEnumerator Start()
    {
        while (!_geospatialManager.Initialized)
        {
            yield return new WaitForSeconds(1f);
        }

        Logger.Log("Spawning predefined objects");

        foreach (var anchor in anchorListSO.Anchors)
        {
            _geospatialManager.SpawnObject(new GeospatialPose()
            {
                Latitude = anchor.Latitude,
                Longitude = anchor.Longitude,
                Altitude = anchor.Altitude,
                EunRotation = anchor.EunRotation,
            }, spawnPrefab);
        }

        Destroy(gameObject); // We don't need this object to exist anymore
    }
}
