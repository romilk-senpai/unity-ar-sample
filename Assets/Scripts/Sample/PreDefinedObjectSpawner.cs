using System.Collections;
using Google.XR.ARCoreExtensions;
using UnityEngine;
using Zenject;

// we take pre scriptable object with array of predefined anchors and spawn them
public class PreDefinedObjectSpawner : MonoBehaviour
{
    [SerializeField] private AnchorListSO anchorListSO;
    [SerializeField] private GameObject spawnPrefab;

    [Inject] private GeospatialObjectManager _geospatialManager;

    // coroutine start/awake/etc methods have fine syntax 
    // therefore sometimes I use it instead of async
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
