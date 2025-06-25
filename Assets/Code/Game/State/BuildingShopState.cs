using System;
using System.Collections.Generic;
using Code.Common.Reactive;

namespace Code.Game.State
{
    [Serializable]
    public class BuildingShopState
    {
        public ReactiveProperty<List<string>> AvailableBuildingIds = new(new List<string>());
        public ReactiveProperty<string> SelectedBuildingId = new(null);
    }
}