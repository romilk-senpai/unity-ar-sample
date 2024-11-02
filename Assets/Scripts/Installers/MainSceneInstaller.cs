using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [SerializeField] private ARAnchorManager anchorManager;
    [SerializeField] private ARCoreExtensions arCoreExtensions;
    [SerializeField] private AREarthManager earthManager;
    [SerializeField] private VpsInitializer initializer;
    [SerializeField] private GeospatialObjectManager geospatialObjectManager;
    [SerializeField] private BallSample ballSample;

    public override void InstallBindings()
    {
        // some AR bindings, maybe creating and iterface to them is a good idea
        Container.Bind<ARAnchorManager>()
            .FromInstance(anchorManager)
            .AsSingle();

        Container.Bind<ARCoreExtensions>()
            .FromInstance(arCoreExtensions)
            .AsSingle();

        Container.Bind<AREarthManager>()
            .FromInstance(earthManager)
            .AsSingle();

        Container.Bind<VpsInitializer>()
            .FromInstance(initializer)
            .AsSingle();

        Container.Bind<GeospatialObjectManager>()
            .FromInstance(geospatialObjectManager)
            .AsSingle();

        // did this because initially wanted to handle this input with input system :c
        Container.Bind<IInputHandler>()
            .To<BallSample>()
            .FromInstance(ballSample)
            .AsSingle();
    }
}
