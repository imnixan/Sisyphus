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
        height = (float)System.Math.Round(height, 2);
        heightText.text = height.ToString();
    }

    public float GetHeight()
    {
        return height;
    }
}
