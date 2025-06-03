using Code.Common.EditorUtils;
using Code.Game.Configs;
using Code.Game.Configs.Monsters;
using Code.Game.Logic;
using Code.Game.Model;
using Code.Game.Model.Quests;
using Code.Game.Model.Rewards;
using Code.Game.State;
using Code.Game.Systems.Interfaces;
using Code.Game.Systems.Managers;
using Code.Game.Systems.Services;
using Code.UI.Popups;
using Code.UI.ViewModels;
using Code.UI.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Installers
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("References")]
        [SerializeField, CantBeNull] private Canvas _canvas = null!;
        [SerializeField, CantBeNull] private EnergyView[] _energyViews;
        [SerializeField, CantBeNull] private MainScreenView _mainScreenView;
        [SerializeField, CantBeNull] private RewardBufferView _rewardBufferView;

        [Header("Managers")]
        [SerializeField, CantBeNull] private GridManager _gridManager = null!;
        [SerializeField, CantBeNull] private PopupManager _popupManager = null!;

        [Header("Services")]
        [SerializeField, CantBeNull] private GameMessageService _messageService = null!;

        [Header("Prefabs")]
        [SerializeField, CantBeNull] private GridObjectCatalog _draggablePrefab = null!;

        [Header("Config")]
        [SerializeField, CantBeNull] private LevelStartupConfig _startupConfig = null!;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterUnityObjects(builder);
            RegisterServices(builder);
            RegisterModels(builder);
            RegisterFactories(builder);
            RegisterViewModels(builder);
            RegisterStartup(builder);
        }
        
        private void RegisterUnityObjects(IContainerBuilder builder)
        {
            builder.RegisterInstance(_canvas);
            builder.RegisterInstance(_gridManager);
            builder.RegisterInstance(_startupConfig);
            builder.RegisterInstance(_draggablePrefab);
            builder.RegisterInstance(_energyViews);
            builder.RegisterInstance(_mainScreenView);

            builder.RegisterComponent<IGameMessageService>(_messageService);
            builder.RegisterComponent(_popupManager);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<DragDropService>(Lifetime.Singleton);
            builder.Register<MergeService>(Lifetime.Singleton);
            builder.Register<SaveLoadService>(Lifetime.Singleton);
            builder.Register<TileService>(Lifetime.Singleton);
            builder.Register<AutoSaveService>(Lifetime.Singleton).As<IStartable>();
            builder.Register<EnergyTickService>(Lifetime.Singleton).As<IStartable>();
            builder.Register<QuestEventBus>(Lifetime.Singleton).As<IQuestEventBus>();
        }

        private void RegisterModels(IContainerBuilder builder)
        {
            builder.Register<GameState>(Lifetime.Singleton);
            builder.Register<EnergyModel>(Lifetime.Singleton);
            builder.Register<CurrencyModel>(Lifetime.Singleton);
            builder.Register<QuestModel>(Lifetime.Singleton);
            builder.Register<RewardBufferModel>(Lifetime.Singleton);
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<EnergyViewModelFactory>(Lifetime.Singleton);
            builder.Register<DraggableFactory>(Lifetime.Singleton);
        }
        
        private void RegisterViewModels(IContainerBuilder builder)
        {
            builder.Register<CurrencyViewModel>(Lifetime.Singleton);
            builder.Register<EnergyEntryViewModel>(Lifetime.Singleton);
            builder.Register<LevelViewModel>(Lifetime.Singleton);
            builder.Register<MainScreenViewModel>(Lifetime.Singleton);
            builder.RegisterComponent(_rewardBufferView);
        }

        private void RegisterStartup(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameInitializer>();
        }
    }
}
