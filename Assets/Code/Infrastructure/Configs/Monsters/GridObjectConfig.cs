using Code.Presentation.Views;
using Code.Utils;
using UnityEngine;

namespace Code.Infrastructure.Configs.Monsters
{
    [CreateAssetMenu(menuName = "Unit/GridObjectConfig")]
    public class GridObjectConfig : ScriptableObject 
    {
        [SerializeField, CantBeNull] private string _id;
        [SerializeField, CantBeNull] private DraggableObject _objectPrefab;
        [SerializeField, CantBeNull] private int _baseLevel;
        [SerializeField] private GridObjectConfig _nextLevelUnit;
        
        public string Id => _id;
        public DraggableObject ObjectPrefab => _objectPrefab;
        public int BaseLevel => _baseLevel;
        public GridObjectConfig Next => _nextLevelUnit;
    }
}