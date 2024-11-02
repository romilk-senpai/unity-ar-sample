using Cysharp.Threading.Tasks;
using Google.XR.ARCoreExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;

public class GeospatialObjectManager : MonoBehaviour
{
    [SerializeField] private double HeadingThreshold = 25;
    [SerializeField] private double HorizontalThreshold = 20;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI outputText;

    [Inject] private ARAnchorManager _anchorManager;
    [Inject] private AREarthManager _earthManager;
    [Inject] private VpsInitializer _initializer;

    private bool _initialized = false;

    public bool Initialized => _initialized;

    private void Update()
    {
        if (!_initializer.IsReady || _earthManager.EarthTrackingState != TrackingState.Tracking)
        {
            Logger.Log($"Earth tracking status: {_earthManager.EarthTrackingState}\nInitializer:{_initializer.IsReady}");
            return;
        }

        string status;
        GeospatialPose pose = _earthManager.CameraGeospatialPose;

        if (pose.OrientationYawAccuracy > HeadingThreshold ||
              pose.HorizontalAccuracy > HorizontalThreshold)
        {
            status = "Low Tracking Accuracy： Please look arround.";
        }
        else
        {
            status = "High Tracking Accuracy";
            if (!_initialized)
            {
                _initialized = true;
            }
        }

        ShowTrackingInfo(status, pose);
    }

    public async void SpawnObject(GeospatialPose pose, GameObject prefab)
    {
        if (!_initialized)
        {
            return;
        }

        ResolveAnchorOnTerrainPromise terrainPromise =
                _anchorManager.ResolveAnchorOnTerrainAsync(pose.Latitude, pose.Longitude, pose.Altitude, pose.EunRotation);

        await terrainPromise;

        var result = terrainPromise.Result;

        if (result.TerrainAnchorState == TerrainAnchorState.Success && result.Anchor != null)
        {
            var spawnedObject = Instantiate(prefab, result.Anchor.gameObject.transform);
            spawnedObject.transform.parent = result.Anchor.gameObject.transform;
        }
    }

    private void ShowTrackingInfo(string status, GeospatialPose pose)
    {
        outputText.text = string.Format(
           $"Latitude: {pose.Latitude:F6}°\n" +
           $"Longitude: {pose.Longitude:F6}°\n" +
           $"Horizontal Accuracy: {pose.HorizontalAccuracy:F6}m\n" +
           $"Altitude: {pose.Altitude:F6}m\n" +
           $"Vertical Accuracy: {pose.VerticalAccuracy:F6}m\n" +
           $"Heading: {pose.EunRotation:F2}\n" +
           $"Heading Accuracy: {pose.EunRotation:F2}\n" +
           $"Status: {status}"
       );
    }
}
