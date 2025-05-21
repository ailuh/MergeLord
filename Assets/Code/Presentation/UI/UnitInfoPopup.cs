using System.Collections.Generic;
using System.Linq;
using Code.Infrastructure.Configs.Monsters;
using Code.Presentation.Animations;
using Code.Utils;
using TMPro;
using UnityEngine;

namespace Code.Presentation.UI
{
    public class UnitInfoPopup : PopupBase
    {
        [Header("Content References")]
        [SerializeField, CantBeNull] private TextMeshProUGUI _titleText;
        [SerializeField, CantBeNull] private TextMeshProUGUI _descText;
        [SerializeField, CantBeNull] private Transform _objectInfoListParent;
        [SerializeField, CantBeNull] private ObjectInfoElement _elementPrefab;
        [SerializeField] private List<PopupElementAnimation> _animatedElements;
        
        protected override List<PopupElementAnimation> GetAnimatedElements() => _animatedElements;
        private readonly List<ObjectInfoElement> _cache = new();
        
        public override void SetData(ObjectRef data)
        {
            _animatedElements.Clear();
            _titleText.text = data.Id;
            _descText.text = data.Description;
            ShowChain(data);
        }

        private void ShowChain(ObjectRef startConfig)
        {
            ClearChain();
            var current = startConfig.RootLevelObject;

            while (current != null)
            {
                var objRef = current.ObjectRef;

                var element = GetOrCreateElement();
                element.SetData(objRef);
                element.gameObject.SetActive(true);
                element.transform.SetParent(_objectInfoListParent, false);
                current = current.NextLevelObject;
            }
        }
        
        private ObjectInfoElement GetOrCreateElement()
        {
            foreach (var cached in _cache.Where(cached => !cached.gameObject.activeSelf))
            {
                _animatedElements.Add(cached);
                return cached;
            }

            var newElement = Instantiate(_elementPrefab, _objectInfoListParent);
            _animatedElements.Add(newElement);
            _cache.Add(newElement);
            return newElement;
        }

        private void ClearChain()
        {
            foreach (var element in _cache)
            {
                element.gameObject.SetActive(false);
            }
        }
        
        public override PopupType Type { get; }
    }
}