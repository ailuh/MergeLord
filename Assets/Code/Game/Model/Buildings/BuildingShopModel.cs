using System.Collections.Generic;
using System.Linq;
using Code.Game.Configs.Buildings;
using Code.Game.Configs.DataModels;
using Code.Game.State;
using Code.Game.Systems.Managers;

namespace Code.Game.Model.Buildings
{
    public class BuildingShopModel
    {
        private readonly GameState _state;
        private readonly BuildingCatalog _buildingCatalog;
        private readonly GridManager _gridManager;

        public BuildingShopModel(
            GameState state,
            BuildingCatalog buildingCatalog,
            GridManager gridManager)
        {
            _state = state;
            _buildingCatalog = buildingCatalog;
            _gridManager = gridManager;
        }

        public void UpdateAvailableBuildingsByLevel(int playerLevel)
        {
            var unlocked = _buildingCatalog.AllBuildings
                .Where(b => b.RequiredLevel <= playerLevel)
                .Select(b => b.Id)
                .ToList();

            _state.BuildingShop.AvailableBuildingIds.Value = unlocked;
        }
        
        public List<BuildingConfig> GetAvailableBuildings()
        {
            return _buildingCatalog.AllBuildings
                .Where(b => _state.BuildingShop.AvailableBuildingIds.Value.Contains(b.Id))
                .ToList();
        }

        public bool CanAfford(BuildingConfig config)
        {
            foreach (var cost in config.Costs)
            {
                if (!_state.Currency.TryGetValue(cost.Currency, out var value) || value.Value < cost.Amount)
                {
                    return false;
                }
            }

            return true;
        }

        private bool HasFreeTile()
        {
            return _gridManager.HasFreeTile();
        }

        public bool CanBuy(BuildingConfig config)
        {
            return CanAfford(config) && HasFreeTile();
        }

        public bool ConfirmPurchase(BuildingConfig config)
        {
            if (!CanBuy(config)) return false;

            foreach (var cost in config.Costs)
            {
                if (_state.Currency.TryGetValue(cost.Currency, out var value))
                {
                    value.Value -= cost.Amount;
                }
            }

            var prefab = _buildingCatalog.GetPrefab(config.Id);
            if (prefab == null)
            {
                return false;
            }
            var success = _gridManager.TryPlaceObject(config);
            _state.BuildingShop.SelectedBuildingId.Value = null;

            return success;
        }

        public void CancelPurchase()
        {
            _state.BuildingShop.SelectedBuildingId.Value = null;
        }
    }
}