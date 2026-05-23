using UnityEngine;
using UnityEngine.UI;

public class GageUI : MonoBehaviour
{
    public BallMove ball;
    private Slider slider;

    private Image fillImage;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        Transform fillArea = slider.transform.Find("Fill Area");
        Transform fill = fillArea.transform.Find("Fill");

        fillImage = fill.GetComponent<Image>();

        slider.minValue = 0.0f;
        slider.maxValue = ball.MaxHoldGage;
    }

    private void Update()
    {
        ball.HoldGage = Mathf.Clamp(ball.HoldGage, 0.0f, ball.MaxHoldGage);

        float ratio = ball.HoldGage / ball.MaxHoldGage;

        Color fillColor = fillImage.color;

        fillColor.g = 1.0f - ratio;

        fillImage.color = fillColor;

        slider.value = ball.HoldGage;
    }
}