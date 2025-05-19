using Code.Application.Factories;
using Code.Application.GameLoop;
using Code.Application.Interfaces;
using Code.Application.Managers;
using Code.Domain.Services;
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
        [SerializeField, CantBeNull] private Canvas _canvas = null!;
        [SerializeField, CantBeNull] private GridManager _gridManager = null!;
        [SerializeField, CantBeNull] private GameMessageService _messageService = null!;

        [Header("Prefabs")]
        [SerializeField, CantBeNull] private GridObjectCatalog _draggablePrefab = null!;

        [Header("Config")]
        [SerializeField, CantBeNull] private LevelStartupConfig _startupConfig = null!;
    
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_canvas);
            builder.RegisterInstance(_gridManager);
            builder.RegisterInstance(_startupConfig);
            builder.RegisterInstance(_draggablePrefab);
            
            builder.RegisterComponent<IGameMessageService>(_messageService);

            builder.Register<DragDropManager>(Lifetime.Singleton);
            builder.Register<MergeService>(Lifetime.Singleton);
            builder.Register<DraggableFactory>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<GameInitializer>();
        }
    }
}
