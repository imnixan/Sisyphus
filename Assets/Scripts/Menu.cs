using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Image blackScreen,
        soundBtn,
        vibroBtn;

    [SerializeField]
    private RectTransform menuScreen,
        settingsScreen,
        statue;

    [SerializeField]
    private float animLength;

    private float hidePosX = 1000;
    private Sequence goGame,
        goSettings,
        backMenu;

    private void OnEnable()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Application.targetFrameRate = 300;
    }

    private void Start()
    {
        ColorizeBtns();
        InitAnims();
        BackMenu();
    }

    private void ColorizeBtns()
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            soundBtn.DOColor(Color.green, animLength).Play();
        }
        else
        {
            soundBtn.DOColor(Color.red, animLength).Play();
        }

        if (PlayerPrefs.GetInt("Vibro", 1) == 1)
        {
            vibroBtn.DOColor(Color.green, animLength).Play();
        }
        else
        {
            vibroBtn.DOColor(Color.red, animLength).Play();
        }
    }

    public void ChangeParameter(string param)
    {
        PlayerPrefs.SetInt(param, PlayerPrefs.GetInt(param, 1) == 1 ? 0 : 1);
        PlayerPrefs.Save();
        ColorizeBtns();
    }

    public void StartGame()
    {
        goGame.Restart();
    }

    public void OpenSettings()
    {
        goSettings.Restart();
    }

    public void BackMenu()
    {
        backMenu.Restart();
    }

    private void InitAnims()
    {
        goGame = DOTween
            .Sequence()
            .Append(blackScreen.DOColor(Color.black, animLength))
            .AppendCallback(() =>
            {
                SceneManager.LoadSceneAsync("GameScene");
            });

        goSettings = DOTween
            .Sequence()
            .Append(menuScreen.DOAnchorPosX(hidePosX, animLength))
            .Append(settingsScreen.DOAnchorPosX(0, animLength))
            .Append(statue.DOAnchorPosX(0, animLength));

        backMenu = DOTween
            .Sequence()
            .Append(statue.DOAnchorPosX(-hidePosX, animLength))
            .Append(settingsScreen.DOAnchorPosX(-hidePosX, animLength))
            .Join(menuScreen.DOAnchorPosX(0, animLength));
    }
}
