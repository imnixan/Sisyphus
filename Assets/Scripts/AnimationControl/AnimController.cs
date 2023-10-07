using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class AnimController : MonoBehaviour
{
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

    private Animator animator;

    [SerializeField]
    private GameEnum.GameState _gameState;

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
        SetFirstLeg(GameEnum.GameState.LeftLegPushin);
        if (GameState == GameEnum.GameState.LeftLegPushin)
        {
            MoveLeg(forwardStep);
        }
        else
        {
            MoveLeg(backwardStep);
        }
    }

    public void PushRightLeg()
    {
        SetFirstLeg(GameEnum.GameState.RightLegPushin);
        if (GameState == GameEnum.GameState.RightLegPushin)
        {
            MoveLeg(forwardStep);
        }
        else
        {
            MoveLeg(-forwardStep);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0;
        GameState = GameEnum.GameState.PreStart;
        guiManager.StartInit(failPoint);
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
                Mathf.Lerp(
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime,
                    animPoint,
                    Time.fixedDeltaTime
                )
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
    }

    private void ChangeLeg()
    {
        GameState = oppositeLegs[GameState];
        animator.Play(animStates[GameState], 0, 0);
        guiManager.PointToBtn(GameState);
    }

    private void SetFirstLeg(GameEnum.GameState firstLeg)
    {
        if (GameState == GameEnum.GameState.PreStart)
        {
            GameState = firstLeg;
            guiManager.PointToBtn(GameState);
            animPoint = 0;
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
                Debug.Log("EndGame");
                break;
        }
    }
}
