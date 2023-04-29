using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope {


    protected override void Configure( IContainerBuilder builder ) {
        builder.Register<IGameEvents, GameEvents>( Lifetime.Singleton );
        builder.Register<ISaveManager, PlayerPrefsSaveManager>( Lifetime.Singleton );
        builder.RegisterComponentInHierarchy<GameManager>(  );
        builder.Register<LayerMasks>( Lifetime.Singleton ).AsSelf();
    }

}