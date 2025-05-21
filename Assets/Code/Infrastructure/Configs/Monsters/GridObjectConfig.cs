using System;
using Code.Presentation.Views;
using Code.Utils;
using UnityEngine;

namespace Code.Infrastructure.Configs.Monsters
{
    [CreateAssetMenu(menuName = "Unit/GridObjectConfig")]
    public class GridObjectConfig : ScriptableObject 
    {
        [SerializeField, CantBeNull] private ObjectRef _objectRef;
        [SerializeField, CantBeNull] private DraggableObject _objectPrefab = null!;
        [SerializeField, CantBeNull] private GridObjectConfig? _rootLevelObject;
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
        public int GeneratedMana;
        public int BaseLevel;
        public ObjectRef(
            string id, 
            string description, 
            GridObjectConfig rootLevelObject, 
            Sprite sprite,
            int generatedMana,
            int baseLevel)
        {
            Id = id;
            Description = description;
            RootLevelObject = rootLevelObject;
            Sprite = sprite;
            GeneratedMana = generatedMana;
            BaseLevel = baseLevel;
        }

        public override string ToString() => Description;
    }
}