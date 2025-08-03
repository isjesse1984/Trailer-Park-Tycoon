using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace ImageFX
{
    public class Neon : MonoBehaviour
    {
        [Header("Color Change Settings")]
        [Tooltip("Time interval between color changes in seconds")]
        [SerializeField] private float changeInterval = 1f;

        [Tooltip("Minimum color saturation (0-1)")]
        [SerializeField] private float minSaturation = 0.7f;

        [Tooltip("Minimum color brightness (0-1)")]
        [SerializeField] private float minBrightness = 0.7f;

        private Image imageComponent;
        private WaitForSeconds waitTime;

        private void Start()
        {
            // 获取Image组件
            imageComponent = GetComponent<Image>();
            if (imageComponent == null)
            {
                Debug.LogError("No Image component found on this object!");
                enabled = false;
                return;
            }

            waitTime = new WaitForSeconds(changeInterval);
            StartCoroutine(ChangeColorRoutine());
        }

        private IEnumerator ChangeColorRoutine()
        {
            while (true)
            {
                // 生成随机明亮的颜色
                Color newColor = GenerateBrightColor();

                // 应用到Image组件
                imageComponent.color = newColor;

                // 等待指定时间
                yield return waitTime;
            }
        }

        private Color GenerateBrightColor()
        {
            // 使用HSV颜色空间来确保颜色明亮
            float hue = Random.Range(0f, 1f);
            float saturation = Random.Range(minSaturation, 1f);
            float brightness = Random.Range(minBrightness, 1f);

            // 转换HSV到RGB
            return Color.HSVToRGB(hue, saturation, brightness);
        }

        // 在Inspector中调整参数的方法
        public void SetChangeInterval(float interval)
        {
            changeInterval = Mathf.Max(0.1f, interval);
            waitTime = new WaitForSeconds(changeInterval);
        }
    }
}