using System.Collections;
using Google.XR.ARCoreExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

// taken and reworked from https://github.com/TakashiYoshinaga/GeospatialAPI-Unity-StarterKit/tree/main/SampleProject/Assets/AR_Fukuoka/Scripts
// i just removed all the unnecessary stuff and formated to look like my code :]
public class VpsInitializer : MonoBehaviour
{
    [Inject] private AREarthManager _earthManager;
    [Inject] private InfoPanel _infoPanel;

    private bool _isReturning = false;

    private bool _isReady = false;
    private IEnumerator _startLocationService = null;

    public bool IsReady => _isReady;

    private void Awake()
    {
        // restrict rotation, at least it is recommended by Lightship
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.Portrait;

        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        _isReturning = false;
        _isReady = false;
        _startLocationService = StartLocationService();
        StartCoroutine(_startLocationService);
    }

    // location initialization
    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            string msg = "Location service is disabled by the user.";
            _infoPanel.SetLocationInfoText($"INFO: {msg}");
            Logger.Log(msg);
            yield break;
        }

        if (Input.location.status != LocationServiceStatus.Running)
        {
            string msg = "Starting location service.";
            _infoPanel.SetLocationInfoText($"INFO: {msg}");
            Logger.Log(msg);

            Input.location.Start();

            while (Input.location.status == LocationServiceStatus.Initializing)
            {
                Logger.Log("Initializing...");
                yield return new WaitForSeconds(.5f);
            }
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            string msg = $"Location service ended with {Input.location.status} status.";
            _infoPanel.SetLocationInfoText($"WARN: {msg}");
            Logger.LogWarning(msg);
            Input.location.Stop();
        }
        else
        {
            string msg = "Location service is ready.";
            _infoPanel.SetLocationInfoText($"INFO: {msg}");
            Logger.Log(msg);
            _isReady = true;
        }
    }

    private void OnDisable()
    {
        StopCoroutine(_startLocationService);
        _startLocationService = null;
        Logger.Log("Stop location services.");
        Input.location.Stop();
    }

    private void Update()
    {
        LifecycleUpdate();

        _isReady = ARSession.state == ARSessionState.SessionTracking &&
                   Input.location.status == LocationServiceStatus.Running;

        if (!_isReady)
        {
            return;
        }

        if (_isReturning)
        {
            return;
        }

        if (ARSession.state != ARSessionState.SessionInitializing &&
            ARSession.state != ARSessionState.SessionTracking)
        {
            return;
        }

        var featureSupport = _earthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
        switch (featureSupport)
        {
            case FeatureSupported.Unknown:
                return;
            case FeatureSupported.Unsupported:
                ReturnWithReason("The Geospatial API is not supported by this device.");
                return;
            case FeatureSupported.Supported:
                break;
        }

        var earthState = _earthManager.EarthState;
        if (earthState == EarthState.ErrorEarthNotReady)
        {
            return;
        }
        else if (earthState != EarthState.Enabled)
        {
            string msg = $"Geospatial sample encountered an EarthState error: {earthState}";
            _infoPanel.SetLocationInfoText($"WARN: {msg}");
            Logger.LogWarning(msg);
            return;
        }
    }

    // lifecycle update to check whether everyhing is fine with tracking
    private void LifecycleUpdate()
    {
        if (_isReturning)
        {
            return;
        }

        var sleepTimeout = SleepTimeout.NeverSleep;
        if (ARSession.state != ARSessionState.SessionTracking)
        {
            sleepTimeout = SleepTimeout.SystemSetting;
        }

        Screen.sleepTimeout = sleepTimeout;

        string returningReason = string.Empty;
        if (ARSession.state != ARSessionState.CheckingAvailability &&
            ARSession.state != ARSessionState.Ready &&
            ARSession.state != ARSessionState.SessionInitializing &&
            ARSession.state != ARSessionState.SessionTracking)
        {
            returningReason = string.Format(
                "Geospatial sample encountered an ARSession error state {0}.\n" +
                "Please restart the app.",
                ARSession.state);
        }
        else if (Input.location.status == LocationServiceStatus.Failed)
        {
            returningReason =
                "Geospatial sample failed to start location service.\n" +
                "Please restart the app and grant the fine location permission.";
        }

        ReturnWithReason(returningReason);
    }

    private void ReturnWithReason(string reason)
    {
        if (string.IsNullOrEmpty(reason))
        {
            return;
        }

        _infoPanel.SetLocationInfoText($"ERR: {reason}");

        Logger.LogError(reason);
        _isReturning = true;
        _isReady = false;
    }
}
