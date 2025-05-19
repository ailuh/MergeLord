using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Infrastructure.Configs
{
    [CreateAssetMenu(menuName = "Game/LevelStartupConfig")]
    public class LevelStartupConfig : ScriptableObject
    {
        [SerializeField] private Vector2Int _gridSize; 
        [SerializeField] private List<TileObjectData> _tiles = new();
        public IEnumerable<TileObjectData> Tiles => _tiles;
        public Vector2Int GridSize => _gridSize;
        
#if UNITY_EDITOR
        [ContextMenu("Generate Grid")]
        private void GenerateGrid()
        {
            _tiles.Clear();

            for (int y = 0; y < _gridSize.y; y++)
            {
                for (int x = 0; x < _gridSize.x; x++)
                {
                    _tiles.Add(new TileObjectData
                    {
                        Position = new Vector2Int(x, y),
                        ObjectId = ""
                    });
                }
            }

            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log("Grid generated in config.");
        }
#endif
        
    }
    [Serializable]
    public class TileObjectData
    {
        [HideInInspector] public Vector2Int Position;
        public string ObjectId = string.Empty;
    }
    
    
}
