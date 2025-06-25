using Code.Game.Configs.Buildings;
using Code.Game.Configs.Interfaces;
using Code.Game.Configs.Monsters;
using Configs.Objects;

namespace Code.Game.Configs.GridConfig
{
    public class GridObjectResolver : IGridObjectResolver
    {
        private readonly MonsterObjectCatalog _monsterCatalog;
        private readonly BuildingCatalog _buildingCatalog;

        public GridObjectResolver(MonsterObjectCatalog monsterCatalog, BuildingCatalog buildingCatalog)
        {
            _monsterCatalog = monsterCatalog;
            _buildingCatalog = buildingCatalog;
        }

        public GridObjectConfigBase? Resolve(string id)
        {
            var monster = _monsterCatalog.GetConfig(id);
            if (monster != null)
            {
                return monster;
            }

            var building = _buildingCatalog.GetConfig(id);
            if (building != null)
            {
                return building;
            }

            return null;
        }
    }
}