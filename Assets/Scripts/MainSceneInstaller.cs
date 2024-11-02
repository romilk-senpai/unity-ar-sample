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

    public override void InstallBindings()
    {
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
    }
}
