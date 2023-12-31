using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AnimController : MonoBehaviour
{
    public event UnityAction legPicked,
        wrondLegClicked,
        legChanged;

    [SerializeField]
    private SoundManager soundManager;

    [SerializeField]
    private float forwardStep;

    [SerializeField]
    private float backwardStep;

    [SerializeField]
    private float failPoint;

    [SerializeField]
    private float animPoint;

    [SerializeField]
    private GUIManager guiManager;

    [SerializeField]
    private GameEnum.GameState _gameState;

    [SerializeField]
    private Transform stone;

    [SerializeField]
    private HeightCounter heightCounter;

    private Rigidbody stoneRb;

    private Animator animator;
    private GameEnum.GameState GameState
    {
        get { return _gameState; }
        set
        {
            if (_gameState != value)
            {
                _gameState = value;
                ChangeState();
            }
        }
    }

    private Dictionary<GameEnum.GameState, string> animStates = new Dictionary<
        GameEnum.GameState,
        string
    >()
    {
        { GameEnum.GameState.LeftLegPushin, "Base.LeftLegPush" },
        { GameEnum.GameState.RightLegPushin, "Base.RightLegPush" }
    };

    private Dictionary<GameEnum.GameState, GameEnum.GameState> oppositeLegs = new Dictionary<
        GameEnum.GameState,
        GameEnum.GameState
    >()
    {
        { GameEnum.GameState.LeftLegPushin, GameEnum.GameState.RightLegPushin },
        { GameEnum.GameState.RightLegPushin, GameEnum.GameState.LeftLegPushin }
    };

    public void PushLeftLeg()
    {
        if (this.enabled)
        {
            SetFirstLeg(GameEnum.GameState.LeftLegPushin);
            if (GameState == GameEnum.GameState.LeftLegPushin)
            {
                MoveLeg(forwardStep);
            }
            else
            {
                wrondLegClicked?.Invoke();
                MoveLeg(backwardStep);
            }
        }
    }

    public void PushRightLeg()
    {
        if (this.enabled)
        {
            SetFirstLeg(GameEnum.GameState.RightLegPushin);
            if (GameState == GameEnum.GameState.RightLegPushin)
            {
                MoveLeg(forwardStep);
            }
            else
            {
                wrondLegClicked?.Invoke();
                MoveLeg(-forwardStep);
            }
        }
    }

    private void Awake()
    {
        stoneRb = stone.GetComponentInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.speed = 0;
        GameState = GameEnum.GameState.PreStart;
        guiManager.Init(failPoint);
        TurnOffPhysic();
    }

    private void FixedUpdate()
    {
        DecreasePoint();

        SyncAnimWithPoint();
    }

    private void SyncAnimWithPoint()
    {
        if (
            animPoint >= 0
            && GameState != GameEnum.GameState.PreStart
            && GameState != GameEnum.GameState.EndGame
        )
        {
            animator.Play(
                animStates[GameState],
                0,
                Mathf.Lerp(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, animPoint, 0.05f)
            );
        }
    }

    private void DecreasePoint()
    {
        if (GameState > GameEnum.GameState.PreStart && GameState < GameEnum.GameState.EndGame)
        {
            MoveLeg(backwardStep);
        }
    }

    private void MoveLeg(float step)
    {
        animPoint += step;
        heightCounter.UpdateHeight(step);
        guiManager.UpdateScale(animPoint);
        if (animPoint >= 1)
        {
            animPoint = 0;
            ChangeLeg();
        }
        else if (animPoint < failPoint)
        {
            GameState = GameEnum.GameState.EndGame;
        }

        stone.Rotate(step * 5, 0, 0);
    }

    private void ChangeLeg()
    {
        GameState = oppositeLegs[GameState];
        animator.Play(animStates[GameState], 0, 0);
        guiManager.PointToBtn(GameState);
        soundManager.PlayScream();
        legChanged?.Invoke();
    }

    private void SetFirstLeg(GameEnum.GameState firstLeg)
    {
        if (GameState == GameEnum.GameState.PreStart)
        {
            GameState = firstLeg;
            guiManager.PointToBtn(GameState);
            animPoint = 0;
            legPicked?.Invoke();
        }
    }

    private void ChangeState()
    {
        switch (GameState)
        {
            case GameEnum.GameState.LeftLegPushin:
                break;
            case GameEnum.GameState.RightLegPushin:
                break;
            case GameEnum.GameState.EndGame:
                EndGame();
                break;
        }
    }

    private void EndGame()
    {
        TurnOnPhysic();
        guiManager.ShowEndGame(heightCounter.GetHeight());
    }

    private void TurnOnPhysic()
    {
        stoneRb.isKinematic = false;
        animator.enabled = false;
        this.enabled = false;
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = false;
        }
    }

    private void TurnOffPhysic()
    {
        stoneRb.isKinematic = true;
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }
    }
}
