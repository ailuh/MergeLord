using System;
using Code.Common.EditorUtils;
using Code.Game.Enums;
using Code.Game.Logic;
using UnityEngine;

namespace Code.Game.Configs.Monsters
{
    [CreateAssetMenu(menuName = "Unit/GridObjectConfig")]
    public class GridObjectConfig : ScriptableObject 
    {
        [SerializeField, CantBeNull] private ObjectRef _objectRef;
        [SerializeField, CantBeNull] private DraggableObject _objectPrefab = null!;
        [SerializeField] private GridObjectConfig? _nextLevelObjectLevelObject;
        
        public ObjectRef ObjectRef => _objectRef;
        public DraggableObject ObjectPrefab => _objectPrefab;
        public GridObjectConfig? NextLevelObject => _nextLevelObjectLevelObject;
    }
    
    [Serializable]
    public struct ObjectRef
    {
        [TextArea] public string Description;
        public string Id;
        public GridObjectConfig RootLevelObject;
        public Sprite Sprite;
        public EnergyGenerationConfig Energy;
        public int BaseLevel;
        public ObjectRef(
            string id, 
            string description, 
            GridObjectConfig rootLevelObject, 
            Sprite sprite,
            EnergyGenerationConfig energy,
            int baseLevel)
        {
            Id = id;
            Description = description;
            RootLevelObject = rootLevelObject;
            Sprite = sprite;
            Energy = energy;
            BaseLevel = baseLevel;
        }

        public override string ToString() => Description;
    }
    
    [Serializable]
    public class EnergyGenerationConfig
    {
        public EnergyType Type;
        public int EnergyPerTick;
        public float TickSeconds;
    }
}