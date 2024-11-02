using System.Collections;
using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

public class VpsInitializer : MonoBehaviour
{
    [Inject] private AREarthManager _earthManager;

    private bool _isReturning = false;

    private bool _isReady = false;
    private IEnumerator _startLocationService = null;

    public bool IsReady => _isReady;

    public void Awake()
    {
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.Portrait;

        Application.targetFrameRate = 60;
    }

    public void OnEnable()
    {
        _isReturning = false;
        _isReady = false;
        _startLocationService = StartLocationService();
        StartCoroutine(_startLocationService);
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Logger.Log("Location service is disabled by the user.");
            yield break;
        }

        if (Input.location.status != LocationServiceStatus.Running)
        {
            Logger.Log("Starting location service.");
            Input.location.Start();

            while (Input.location.status == LocationServiceStatus.Initializing)
            {
                Logger.Log("Initializing...");
                yield return new WaitForSeconds(.5f);
            }
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Logger.LogWarningFormat(
                "Location service ended with {0} status.", Input.location.status);
            Input.location.Stop();
        }
        else
        {
            Logger.Log("Location service is ready.");
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

    public void Update()
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
            string errorMessage =
                "Geospatial sample encountered an EarthState error: " + earthState;
            Logger.LogWarning(errorMessage);
            return;
        }
    }

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

        Logger.LogError(reason);
        _isReturning = true;
        _isReady = false;
    }
}
