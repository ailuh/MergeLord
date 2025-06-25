using System;
using System.Collections.Generic;
using System.Linq;
using Code.Common.Reactive;
using Code.Game.Configs.Buildings;
using Code.Game.Model.Buildings;
using Code.Game.State;

namespace Code.UI.ViewModels
{
    public class BuildingShopViewModel : IDisposable
    {
        public IReadOnlyReactiveProperty<List<BuildingEntry>> Buildings => _buildings;
        private readonly ReactiveProperty<List<BuildingEntry>> _buildings = new(new List<BuildingEntry>());

        private readonly GameState _state;
        private readonly BuildingCatalog _catalog;
        private readonly BuildingShopModel _model;

        private IDisposable _availableBuildingsSubscription;

        public BuildingShopViewModel(
            GameState state,
            BuildingCatalog catalog,
            BuildingShopModel model)
        {
            _state = state;
            _catalog = catalog;
            _model = model;

            _availableBuildingsSubscription = _state.BuildingShop.AvailableBuildingIds
                .Subscribe(_ => RebuildBuildingEntries());

            RebuildBuildingEntries();
        }

        private void RebuildBuildingEntries()
        {
            var entries = _model.GetAvailableBuildings()
                .Select(config => new BuildingEntry
                {
                    Config = config,
                    CanAfford = _model.CanAfford(config),
                    CanBuy = _model.CanBuy(config)
                }).ToList();

            _buildings.Value = entries;
        }

        public bool CanBuy(BuildingConfig config)
        {
            return _model.CanBuy(config);
        }

        public void ConfirmPurchase(BuildingConfig config)
        {
            _model.ConfirmPurchase(config);
        }

        public void Dispose()
        {
            _availableBuildingsSubscription?.Dispose();
        }
    }

    public class BuildingEntry
    {
        public BuildingConfig Config;
        public bool CanAfford;
        public bool CanBuy;
    }
}