using UnityEngine;
using TMPro;

public class HeightCounter : MonoBehaviour
{
    private TextMeshProUGUI heightText;
    private float height;

    private void Start()
    {
        heightText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateHeight(float step)
    {
        height += step;
        if (height < 0)
        {
            height = 0;
        }
        heightText.text = height.ToString("F2");
    }
}
