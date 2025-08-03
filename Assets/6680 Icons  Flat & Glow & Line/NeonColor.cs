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
            // ��ȡImage���
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
                // ���������������ɫ
                Color newColor = GenerateBrightColor();

                // Ӧ�õ�Image���
                imageComponent.color = newColor;

                // �ȴ�ָ��ʱ��
                yield return waitTime;
            }
        }

        private Color GenerateBrightColor()
        {
            // ʹ��HSV��ɫ�ռ���ȷ����ɫ����
            float hue = Random.Range(0f, 1f);
            float saturation = Random.Range(minSaturation, 1f);
            float brightness = Random.Range(minBrightness, 1f);

            // ת��HSV��RGB
            return Color.HSVToRGB(hue, saturation, brightness);
        }

        // ��Inspector�е��������ķ���
        public void SetChangeInterval(float interval)
        {
            changeInterval = Mathf.Max(0.1f, interval);
            waitTime = new WaitForSeconds(changeInterval);
        }
    }
}