using System;
using Code.Game.Enums;
using Code.Game.Logic;
using Code.UI.Views;
using UnityEngine;

namespace Code.Game.Model
{
    public class TileModel
    {
        public Vector2Int Position { get; }
        public bool IsOccupied => _object != null;
        public DraggableObject? ContainedObject => _object;
        public string? ObjectId { get; private set; }
        private DraggableObject? _object;
        private TileView? _view;
        private double _accumulatedTime;

        public TileModel(Vector2Int position)
        {
            Position = position;
        }

        public void BindView(TileView view)
        {
            _view = view;
        }

        public void SetObjectId(string objectId)
        {
            ObjectId = objectId;
        }
        
        public TileView? GetView() => _view;

        public void SetObject(DraggableObject obj)
        {
            _object = obj;
        }

        public void ClearObject()
        {
            _object = null;
            ObjectId = null;
        }
        
        public void Tick(double deltaTime, Action<EnergyType, int> onEnergyGenerated)
        {
            if (ContainedObject == null)
                return;

            var energyInfo = ContainedObject.ObjectRef.Energy;
            if (energyInfo.TickSeconds <= 0)
                return;

            _accumulatedTime += deltaTime;
            var ticks = (int)(_accumulatedTime / energyInfo.TickSeconds);

            if (ticks > 0)
            {
                _accumulatedTime -= ticks * energyInfo.TickSeconds;
                var totalEnergy = energyInfo.EnergyPerTick * ticks;
                onEnergyGenerated?.Invoke(energyInfo.Type, totalEnergy);
            }
        }
    }
}
