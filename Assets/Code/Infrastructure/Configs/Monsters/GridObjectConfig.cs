using System;
using Code.Presentation.Views;
using Code.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Infrastructure.Configs.Monsters
{
    [CreateAssetMenu(menuName = "Unit/GridObjectConfig")]
    public class GridObjectConfig : ScriptableObject 
    {
        [SerializeField, CantBeNull] private ObjectRef _objectRef;
        [SerializeField, CantBeNull] private DraggableObject _objectPrefab = null!;
        [SerializeField, CantBeNull] private int _baseLevel;
        [SerializeField] private GridObjectConfig? _nextLevelObjectLevelObject;
        
        public ObjectRef ObjectRef => _objectRef;
        public DraggableObject ObjectPrefab => _objectPrefab;
        public int BaseLevel => _baseLevel;
        public GridObjectConfig? NextLevelObject => _nextLevelObjectLevelObject;
    }
    
    [Serializable]
    public struct ObjectRef
    {
        public string Id;
        [TextArea] public string Description;

        public ObjectRef(string id, string description)
        {
            Id = id;
            Description = description;
        }

        public override string ToString() => Description;
    }
}