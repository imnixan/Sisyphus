using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEdu : MonoBehaviour
{
    [SerializeField]
    private AnimController animController;

    [SerializeField]
    private GameObject hintWindow;

    [SerializeField]
    private TextMeshProUGUI hint;

    private bool changeShowed,
        wrongShowed;

    public void CloseHint()
    {
        hintWindow.SetActive(false);
        animController.enabled = true;
    }

    public void StopShowing()
    {
        PlayerPrefs.SetInt("Edu", 0);
        PlayerPrefs.Save();
        CloseHint();
        Destroy(gameObject);
    }

    private void Start()
    {
        animController.enabled = false;
        if (PlayerPrefs.GetInt("Edu", 1) == 0)
        {
            animController.enabled = true;
            Destroy(gameObject);
        }
    }

    private void OnLegPicked()
    {
        animController.enabled = false;
        hintWindow.SetActive(true);
        hint.text = "Quickly ñlick your active leg to fill the strength bar";
    }

    private void OnLegChanged()
    {
        if (!changeShowed)
        {
            animController.enabled = false;
            hintWindow.SetActive(true);
            hint.text = "After filling the power scale, the active leg changes";
            changeShowed = true;
        }
    }

    private void OnWrongLegClicked()
    {
        if (!wrongShowed)
        {
            animController.enabled = false;
            hintWindow.SetActive(true);
            hint.text = "Pressing the wrong leg decreases the strength bar";
            wrongShowed = true;
        }
    }

    private void OnEnable()
    {
        animController.legPicked += OnLegPicked;
        animController.legChanged += OnLegChanged;
        animController.wrondLegClicked += OnWrongLegClicked;
    }

    private void OnDisable()
    {
        animController.legPicked -= OnLegPicked;
        animController.legChanged -= OnLegChanged;
        animController.wrondLegClicked -= OnWrongLegClicked;
    }
}
