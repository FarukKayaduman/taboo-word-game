using UnityEngine;

public class PanelAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform uiPanel;
    private const float ScaleSpeed = 1.25f;
    private const float MinScale = 0.95f;
    private const float MaxScale = 1.1f;

    private void Update()
    {
        float scale = Mathf.Lerp(MinScale, MaxScale, Mathf.PingPong(Time.time * ScaleSpeed, 1));
        uiPanel.localScale = new Vector3(scale, scale, 1);
    }
}
