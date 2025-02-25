using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HAD
{
    public class BloodFrameUI : UIBase
    {
        private float durationTime = 2f;
        private float elapsedTime = 0f;
        [SerializeField] private Image image;

        private void OnEnable()
        {
            StartCoroutine(bloodEffect());
        }

        IEnumerator bloodEffect()
        {
            Color originalColor = image.color;
            while (elapsedTime <= durationTime)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(0.4f, 0f, elapsedTime / durationTime);
                image.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
                yield return null;
            }
            Destroy(this.gameObject);
        }
    }
}
