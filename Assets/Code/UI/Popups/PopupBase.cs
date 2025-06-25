using System;
using System.Collections.Generic;
using Code.Common.EditorUtils;
using Code.Game.Configs.Buildings;
using Code.Game.Configs.Monsters;
using Code.UI.Animations;
using Code.UI.ViewModels;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Popups
{
    public abstract class PopupBase : MonoBehaviour
    {
        [SerializeField, CantBeNull] private CanvasGroup _canvasGroup;
        [SerializeField, CantBeNull] private Button _closeButton;
        [SerializeField] private Button _playAnimation;
        [SerializeField] private float _interElementDelay = 0.05f;
        [SerializeField] private float _fadeDuration = 0.3f;
        public CanvasGroup CanvasGroup => _canvasGroup;
        public abstract PopupType Type { get; }
        protected abstract List<PopupElementAnimation> GetAnimatedElements();

        private void Start()
        {
            _closeButton.onClick.AddListener(ClosePopup);
            if (_playAnimation != null)
            {
                _playAnimation.onClick.AddListener(AnimateElementsIn);
            }
        }

        public virtual void SetData(IPopupData popupData) { }
        
        public virtual async UniTask ShowAsync()
        {
            gameObject.SetActive(true);
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, _fadeDuration).SetEase(Ease.OutQuad);

            AnimateElementsIn();
            await UniTask.Delay(TimeSpan.FromSeconds(_fadeDuration));
        }
        
        public virtual async UniTask HideAsync()
        {
            _canvasGroup.DOFade(0, _fadeDuration).SetEase(Ease.InQuad)
                .OnComplete(() => gameObject.SetActive(false));

            await UniTask.Delay(TimeSpan.FromSeconds(_fadeDuration));
        }
        
        protected void AnimateElementsIn()
        {
            var accumulatedDelay = 0f;
            foreach (var anim in GetAnimatedElements())
            {
                anim.Prepare();
                anim.Animate(accumulatedDelay);
                accumulatedDelay += _interElementDelay;
            }
        }

        private void ClosePopup()
        {
            HideAsync().Forget();
        }
    }
}