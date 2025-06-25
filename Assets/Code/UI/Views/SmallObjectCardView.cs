using System.Collections.Generic;
using Code.Common.EditorUtils;
using Code.UI.Animations;
using Configs.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Views
{
    public class SmallObjectCardView : PopupElementAnimation
    {
        [Header("UI References")]
        [SerializeField, CantBeNull] private Image _mainIcon;
        [SerializeField, CantBeNull] private TextMeshProUGUI _levelText;
        [SerializeField, CantBeNull] private Transform _statsContainer;
        [SerializeField, CantBeNull] private StatIconView _statPrefab;

        private readonly List<StatIconView> _spawnedStats = new();

        public void SetData(GridObjectConfigBase config)
        {
            _mainIcon.sprite = config.Sprite;
            _levelText.text = $"Lvl {config.Level}";

            ClearStats();

            var stats = config.GetCompactStats();
            foreach (var stat in stats)
            {
                var statView = Instantiate(_statPrefab, _statsContainer);
                statView.Set(stat.Icon, stat.Value);
                statView.gameObject.SetActive(true);
                _spawnedStats.Add(statView);
            }
        }

        private void ClearStats()
        {
            foreach (var statView in _spawnedStats)
            {
                Destroy(statView.gameObject);
            }
            _spawnedStats.Clear();
        }
    }
}