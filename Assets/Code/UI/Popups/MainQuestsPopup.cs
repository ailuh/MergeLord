using System.Collections.Generic;
using Code.Common.EditorUtils;
using Code.UI.Animations;
using Code.UI.ViewModels;
using Code.UI.Views;
using UnityEngine;

namespace Code.UI.Popups
{
    public class MainQuestsPopup : PopupBase
    {
        [Header("Content References")]
        [SerializeField, CantBeNull] private List<PopupElementAnimation> _animatedElements = null!;
        [SerializeField, CantBeNull] private QuestView _questView = null!;
        public override PopupType Type => PopupType.QuestInfo;
        protected override List<PopupElementAnimation> GetAnimatedElements() => _animatedElements;
        private QuestViewModel _viewModel;

        public override void SetData(QuestViewModel viewModel)
        {
            _animatedElements.Clear();
            _viewModel = viewModel;
            _animatedElements = _questView.Init(_viewModel, AnimateElementsIn);
        }
    }
}
