using System;
using DG.Tweening;
using UnityEngine;

public class PauseButton : InteractableMonoBehavior
{
    public Game game;

    [GetComponent] public TransitionElement transitionElement;
    [GetComponent] public InteractableMonoBehavior interactableMonoBehavior;
    public CanvasGroup canvasGroup;

    public float normalOpacity = 0.3f;
    public float highlightedOpacity = 0.7f;
    public float animationDuration = 0.4f;
    
    private bool highlighted = false;
    private float willUnhighlightTimestamp = 0;
    
    protected void Awake()
    {
        canvasGroup.alpha = normalOpacity;
        game.onGameCompleted.AddListener(_ =>
        {
            transitionElement.leaveTo = Transition.Default;
            transitionElement.Leave();
        });
        game.onGamePaused.AddListener(_ =>
        {
            transitionElement.leaveTo = Transition.Default;
            transitionElement.Leave();
        });
        game.onGameUnpaused.AddListener(_ =>
        {
            canvasGroup.alpha = normalOpacity;
            transitionElement.enterFrom = Transition.Default;
            transitionElement.Enter();
        });
        interactableMonoBehavior.onPointerClick.AddListener(_ =>
        {
            if (!highlighted)
            {
                Highlight();
            }
            else
            {
                Unhighlight();
                game.Pause();
                Context.ScreenManager.ChangeScreen(PausedScreen.Id, ScreenTransition.None);
            }
        });
    }

    protected void Update()
    {
        if (highlighted && Time.realtimeSinceStartup > willUnhighlightTimestamp)
        {
            Unhighlight();
        }
    }

    private void Highlight()
    {
        highlighted = true;
        canvasGroup.DOFade(highlightedOpacity, animationDuration);
        willUnhighlightTimestamp = Time.realtimeSinceStartup + 2;
    }

    private void Unhighlight()
    {
        highlighted = false;
        canvasGroup.DOFade(normalOpacity, animationDuration);
    }
}