using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private Color textColor;

    [SerializeField]
    private Image powerScaleFill;

    [SerializeField]
    private Image leftBtn,
        rightBtn;

    [SerializeField]
    private Image blackScreen;

    [SerializeField]
    private Color correctBtnColor;

    [SerializeField]
    private float animLength;

    [SerializeField]
    private RectTransform endGameWindow,
        leftBtnRt,
        rightBtnRt,
        scale,
        btns;

    [SerializeField]
    private TextMeshProUGUI finalScores,
        record,
        recordText;

    private float failPoint;
    private float scaleLength;
    private float currentAnimPoint;
    float fillPercent;

    private Sequence endAnim;

    public void Init(float failPoint)
    {
        this.failPoint = Mathf.Abs(failPoint);
        this.scaleLength = 1 + this.failPoint;
        blackScreen.DOColor(new Color(0, 0, 0, 0), animLength).Play();
    }

    public void UpdateScale(float animPoint)
    {
        currentAnimPoint = animPoint + failPoint;
        fillPercent = currentAnimPoint / scaleLength;
        powerScaleFill.fillAmount = fillPercent;
        powerScaleFill.color = new Color(1 - fillPercent, fillPercent, 0, 1);
    }

    public void PointToBtn(GameEnum.GameState gameState)
    {
        switch (gameState)
        {
            case GameEnum.GameState.LeftLegPushin:
                leftBtn.DOColor(correctBtnColor, animLength).PlayForward();
                rightBtn.DOColor(Color.white, animLength).PlayForward();
                break;

            case GameEnum.GameState.RightLegPushin:

                rightBtn.DOColor(correctBtnColor, animLength).PlayForward();
                leftBtn.DOColor(Color.white, animLength).PlayForward();
                break;
        }
    }

    public void ShowEndGame(float currentScores)
    {
        float recordScores = PlayerPrefs.GetFloat("Record");
        endAnim = DOTween.Sequence();
        endAnim
            .Append(leftBtnRt.DOAnchorPosY(-1000, animLength))
            .Join(rightBtnRt.DOAnchorPosY(-1000, animLength))
            .Join(scale.DOAnchorPosY(1000, animLength))
            .Append(btns.DOAnchorPosY(15, animLength))
            .Append(endGameWindow.DOAnchorPos(Vector2.zero, animLength))
            .AppendCallback(() =>
            {
                finalScores.text = currentScores.ToString();
                record.text = recordScores.ToString();
            })
            .Append(finalScores.DOColor(textColor, animLength))
            .Join(record.DOColor(textColor, animLength))
            .AppendInterval(animLength)
            .AppendCallback(() =>
            {
                if (currentScores > recordScores)
                {
                    PlayerPrefs.SetFloat("Record", currentScores);
                    PlayerPrefs.Save();
                    recordText.DOColor(textColor, animLength).Play();
                }
            });
        endAnim.Restart();
    }
}
