using System;
using System.Collections.Generic;
using Code.Infrastructure.Configs.Monsters;
using Code.Presentation.Animations;
using Code.Utils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Presentation.UI
{
    public abstract class PopupBase : MonoBehaviour
    {
        [SerializeField, CantBeNull] private CanvasGroup _canvasGroup;
        [SerializeField, CantBeNull] private Button _closeButton;
        [SerializeField] private float _interElementDelay = 0.05f;
        [SerializeField] private float _fadeDuration = 0.3f;
        
        public abstract PopupType Type { get; }
        protected abstract List<PopupElementAnimation> GetAnimatedElements();

        private void Start()
        {
            _closeButton.onClick.AddListener(ClosePopup);
        }

        public virtual void SetData(ObjectRef data) { }

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
        
        private void AnimateElementsIn()
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