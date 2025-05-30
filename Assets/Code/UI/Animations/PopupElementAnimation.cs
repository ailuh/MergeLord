using Code.Common.EditorUtils;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Animations
{
    public class PopupElementAnimation : MonoBehaviour
    {
        [Header("Animation Settings")] 
        [SerializeField, CantBeNull] private CanvasGroup _canvasGroup;
        [SerializeField, CantBeNull] private RectTransform _rect;
        [SerializeField, CantBeNull] private Vector2 _startOffset = new(0, -50);
        [SerializeField, CantBeNull] private float _delay;
        [SerializeField, CantBeNull] private float _duration = 0.2f;
        [SerializeField, CantBeNull] private Ease _ease = Ease.OutQuad;
        
        public virtual void Prepare()
        {
            _rect.anchoredPosition += _startOffset;
            _rect.localScale = new Vector3(0f, 0f, 1f);
            _canvasGroup.alpha = 0;
        }

        public virtual void Animate(float additionalDelay = 0f)
        {
            var totalDelay = _delay + additionalDelay;
            var seq = DOTween.Sequence();
            seq.AppendInterval(totalDelay);
            seq.Append(_rect.DOScaleX(1f, _duration).SetEase(_ease));
            seq.Join(_rect.DOScaleY(1f, _duration).SetEase(_ease));
            seq.Join(_canvasGroup.DOFade(1f, _duration));
            seq.Play();
        }
    }
}