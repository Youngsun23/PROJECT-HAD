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
        private AnimationCurve currentCurve;
        private bool isFading = false;
        private float fadeStartTime;

        private void Awake()
        {
            Instance = this;
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

                canvasGroup.alpha = currentCurve.Evaluate(t);

                if (t >= 1.0f)
                {
                    isFading = false;
                }
            }
        }

        [Button("Fade In")]
        public void FadeIn()
        {
            StartFade(fadeInCurve);
        }

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