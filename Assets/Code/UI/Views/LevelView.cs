using System;
using Code.Common.EditorUtils;
using Code.UI.ViewModels;
using TMPro;
using UnityEngine;
using VContainer;

namespace Code.UI.Views
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField, CantBeNull] private TextMeshProUGUI _levelLabel = null!;
        [Inject] private LevelViewModel _viewModel = null!;
        private IDisposable _subscription = null!;
        
        private void Start()
        {
            _subscription = _viewModel.Level.Subscribe(UpdateView);
        }

        private void UpdateView(int level)
        {
            _levelLabel.text = $"Level: {level}";
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }
    }
}
