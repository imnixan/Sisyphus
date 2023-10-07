using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private Image powerScaleFill;

    [SerializeField]
    private RectTransform leftBtn,
        rightBtn,
        fingerPoint;

    [SerializeField]
    private float animLength;

    private float failPoint;
    private float scaleLength;
    private float currentAnimPoint;
    float fillPercent;

    public void Init(float failPoint)
    {
        this.failPoint = Mathf.Abs(failPoint);
        this.scaleLength = 1 + this.failPoint;
    }

    public void UpdateScale(float animPoint)
    {
        currentAnimPoint = animPoint + failPoint;
        fillPercent = currentAnimPoint / scaleLength;
        powerScaleFill.fillAmount = fillPercent;
        powerScaleFill.color = new Color(fillPercent, 1 - fillPercent, 0, 1);
    }

    public void PointToBtn(GameEnum.GameState gameState)
    {
        switch (gameState)
        {
            case GameEnum.GameState.LeftLegPushin:
                fingerPoint.DOAnchorPosX(leftBtn.anchoredPosition.x, animLength).PlayForward();
                break;

            case GameEnum.GameState.RightLegPushin:
                fingerPoint.DOAnchorPosX(rightBtn.anchoredPosition.x, animLength).PlayForward();
                break;
        }
    }
}
