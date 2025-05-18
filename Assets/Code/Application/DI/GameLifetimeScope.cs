using Code.Application.Factories;
using Code.Application.GameLoop;
using Code.Application.Managers;
using Code.Infrastructure.Configs;
using Code.Infrastructure.Configs.Monsters;
using Code.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Application.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("References")]
        [SerializeField, CantBeNull] private Canvas _canvas;
        [SerializeField, CantBeNull] private GridManager _gridManager;

        [Header("Prefabs")]
        [SerializeField, CantBeNull] private GridObjectCatalog _draggablePrefab;

        [Header("Config")]
        [SerializeField, CantBeNull] private LevelStartupConfig _startupConfig;
    
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_canvas);
            builder.RegisterInstance(_gridManager);
            builder.RegisterInstance(_startupConfig);
            builder.RegisterInstance(_draggablePrefab);
            
            builder.Register<DraggableFactory>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<GameInitializer>();
        }
    }
}
