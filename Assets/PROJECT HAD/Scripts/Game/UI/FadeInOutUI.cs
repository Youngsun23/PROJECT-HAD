using Sirenix.OdinInspector;
using UnityEngine;

namespace HAD
{
    public class FadeInOutUI : UIBase
    {
        public static FadeInOutUI Instance { get; private set; }

        public CanvasGroup canvasGroup;

        public float fadeDuration = 1.0f;
        public AnimationCurve fadeInCurve;
        public AnimationCurve fadeOutCurve;

        private bool isFading = false;
        private float fadeStartTime;
        private AnimationCurve currentCurve;

        private void Awake()
        {
            Instance = this;
            //Show();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            if (isFading)
            {
                float elapsedTime = Time.time - fadeStartTime;
                float t = Mathf.Clamp01(elapsedTime / fadeDuration);

                // AnimationCurve의 Evaluate로 t 값을 통해 alpha 값을 설정
                canvasGroup.alpha = currentCurve.Evaluate(t);

                if (t >= 1.0f)
                {
                    isFading = false; // 페이드가 끝났음을 표시
                }
            }
        }

        // 장면 시작
        [Button("Fade In")]
        public void FadeIn()
        {
            StartFade(fadeInCurve);
        }

        // 장면 끝
        [Button("Fade Out")]
        public void FadeOut()
        {
            StartFade(fadeOutCurve);
        }

        private void StartFade(AnimationCurve curve)
        {
            this.currentCurve = curve;
            this.fadeStartTime = Time.time;
            this.isFading = true;
        }
    }
}