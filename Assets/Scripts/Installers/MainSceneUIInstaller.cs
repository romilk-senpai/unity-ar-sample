using UnityEngine;
using Zenject;

public class MainSceneUIInstaller : MonoInstaller
{
    [SerializeField] private InfoPanel infoPanel;

    public override void InstallBindings()
    {
        Container.Bind<InfoPanel>()
            .FromInstance(infoPanel)
            .AsSingle();
    }
}
