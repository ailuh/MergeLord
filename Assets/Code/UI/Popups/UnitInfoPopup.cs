using System.Collections.Generic;
using System.Linq;
using Code.Common.EditorUtils;
using Code.UI.Animations;
using Code.UI.Views;
using Configs.Objects;
using TMPro;
using UnityEngine;

namespace Code.UI.Popups
{
    public class UnitInfoPopup : PopupBase
    {
        [Header("Content References")]
        [SerializeField, CantBeNull] private TextMeshProUGUI _titleText;
        [SerializeField, CantBeNull] private TextMeshProUGUI _descText;
        [SerializeField, CantBeNull] private Transform _objectInfoListParent;
        [SerializeField, CantBeNull] private SmallObjectCardView _elementPrefab;
        [SerializeField] private List<PopupElementAnimation> _animatedElements;
        
        protected override List<PopupElementAnimation> GetAnimatedElements() => _animatedElements;
        private readonly List<SmallObjectCardView> _cache = new();
        
        public override void SetData(IPopupData popupData)
        {
            if (popupData is not UnitInfoPopupData data || data.Provider == null)
                return;

            var chain = data.Provider.GetUpgradeChain();
            if (chain.Count == 0)
                return;

            _animatedElements.Clear();
            _titleText.text = chain[0].Id;
            _descText.text = chain[0].Description;

            ShowChain(chain);
        }

        private void ShowChain(List<GridObjectConfigBase> chain)
        {
            ClearChain();

            foreach (var config in chain)
            {
                var element = GetOrCreateElement();
                element.SetData(config);
                element.transform.SetParent(_objectInfoListParent, false);
                element.gameObject.SetActive(true);
            }
        }

        private SmallObjectCardView GetOrCreateElement()
        {
            foreach (var cached in _cache.Where(e => !e.gameObject.activeSelf))
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