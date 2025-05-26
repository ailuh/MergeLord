using System.Collections.Generic;
using Code.Common.EditorUtils;
using Code.Game.Configs.Monsters;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.Popups
{
    public class PopupManager : MonoBehaviour
    {
        [SerializeField, CantBeNull] private Transform _popupRoot;
        [SerializeField, CantBeNull] private List<PopupBase> _popupPrefabs;

        private readonly Dictionary<PopupType, PopupBase> _cache = new();
        
        public async UniTask ShowAsync(PopupType type)
        {
            var popup = GetOrCreatePopup(type);
            if (popup == null) return;

            await popup.ShowAsync();
        }

        public async UniTask ShowAsync(PopupType type, ObjectRef data)
        {
            var popup = GetOrCreatePopup(type);
            if (popup == null)
            {
                return;
            }

            popup.SetData(data);
            await popup.ShowAsync();
        }

        public async UniTask HideAsync(PopupType type)
        {
            if (_cache.TryGetValue(type, out var popup))
            {
                await popup.HideAsync();
            }
        }

        private PopupBase? GetOrCreatePopup(PopupType type)
        {
            if (_cache.TryGetValue(type, out var cachedPopup))
                return cachedPopup;

            var prefab = _popupPrefabs.Find(p => p.Type == type);
            if (prefab == null)
            {
                Debug.LogError($"[PopupManager] Popup prefab with type {type} not found.");
                return null;
            }

            var instance = Instantiate(prefab, _popupRoot);
            _cache[type] = instance;
            return instance;
        }
    }
}