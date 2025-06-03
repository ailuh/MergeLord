using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.UI.Animations
{
    public class PopupNumPoolService : IDisposable
    {
        private readonly Queue<TextMeshProUGUI> _pool = new();
        private readonly Queue<string> _queue = new();

        private readonly Transform _container;
        private readonly TextMeshProUGUI _prefab;
        private bool _isShowing;
        private CancellationTokenSource _cts = new();

        public PopupNumPoolService(TextMeshProUGUI prefab, Transform container, int initialCount = 10)
        {
            _prefab = prefab;
            _container = container;

            for (var i = 0; i < initialCount; i++)
            {
                var obj = Object.Instantiate(_prefab, _container);
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        public void ShowPopup(string text)
        {
            _queue.Enqueue(text);
            if (!_isShowing)
            {
                ProcessQueueAsync(_cts.Token).Forget();
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }

        private async UniTaskVoid ProcessQueueAsync(CancellationToken token)
        {
            _isShowing = true;

            while (_queue.Count > 0)
            {
                token.ThrowIfCancellationRequested();

                var text = _queue.Dequeue();

                var popup = _pool.Count > 0 ? _pool.Dequeue() : Object.Instantiate(_prefab, _container);
                if (popup == null) continue;

                popup.text = text;

                popup.rectTransform.anchoredPosition = Vector2.zero;
                popup.alpha = 1f;
                popup.gameObject.SetActive(true);

                AnimatePopupAsync(popup, token).Forget();

                await UniTask.Delay(300, cancellationToken: token);
            }

            _isShowing = false;
        }

        private async UniTaskVoid AnimatePopupAsync(TextMeshProUGUI popup, CancellationToken token)
        {
            if (popup == null || popup.gameObject == null) return;

            var rect = popup.rectTransform;
            if (rect == null) return;

            var startPos = rect.anchoredPosition;
            var endPos = startPos + new Vector2(0f, 30f);

            const float duration = 1f;
            float time = 0f;

            try
            {
                while (time < duration)
                {
                    token.ThrowIfCancellationRequested();

                    if (popup == null || popup.gameObject == null || rect == null)
                        return;

                    time += Time.deltaTime;
                    float t = time / duration;

                    rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                    popup.alpha = Mathf.Lerp(1f, 0f, t);
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

                if (popup != null && popup.gameObject != null)
                {
                    popup.gameObject.SetActive(false);
                    _pool.Enqueue(popup);
                }
            }
            catch (MissingReferenceException)
            {
                Debug.Log("Game aborted");
            }
        }
    }
}
