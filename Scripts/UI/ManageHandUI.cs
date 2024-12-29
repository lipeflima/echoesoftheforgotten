using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageHandUI : MonoBehaviour
{
    [SerializeField] private Button resetHandButton;
    [SerializeField] private Button shuffleButton;
    [SerializeField] private Button pickCardButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private CardUI cardUI;
    [SerializeField] private PlayerDeckManager playerDeckManager;
    [SerializeField] private RawImage deckIndicator;
    private Action onComplete;
    private ActionData actionData;
    [SerializeField] public int maxResetHandCount = 2;
    [SerializeField] public int maxShuffleCount = 3;
    [SerializeField] public int maxShuffleCountPerTurn, maxPickCardCountPerTurn = 2;
    [SerializeField] public int resetHandCount, pickCardCount, shuffleCount, totalShuffleCount = 0;
    [SerializeField] public int handCount = 0;

    private void Update()
    {
        handCount = actionData.CurrentTurnAction == ActionManager.CurrentTurnAction.Attack ? playerDeckManager.GetAttackHand().Count : playerDeckManager.GetDefenseHand().Count;

        resetHandButton.interactable = resetHandCount < maxResetHandCount;
        shuffleButton.interactable = totalShuffleCount < maxShuffleCount && shuffleCount < maxShuffleCountPerTurn;
        pickCardButton.interactable = pickCardCount < maxPickCardCountPerTurn && handCount < playerDeckManager.GetMaxHandSize();
    }

    public void Initialize(ActionData data, Action onCompleteCallback)
    {
        pickCardCount = 0;
        shuffleCount = 0;
        actionData = data;
        gameObject.SetActive(true);
        onComplete = onCompleteCallback;
        nextButton.interactable = true; 
        nextButton.onClick.AddListener(CompleteStep);

        foreach (var cardObject in cardUI.GetCardObjects())
        {
            cardObject.GetComponent<CardInteraction>().SetClickable(true);
        }

        resetHandButton.onClick.RemoveListener(OnResetHand);
        resetHandButton.onClick.AddListener(OnResetHand);

        shuffleButton.onClick.RemoveListener(OnShuffle);
        shuffleButton.onClick.AddListener(OnShuffle);

        pickCardButton.onClick.RemoveListener(OnPickCard);
        pickCardButton.onClick.AddListener(OnPickCard);
    }

    private void CompleteStep()
    {
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    private void OnResetHand()
    {
        _ = new List<Card>();
        var maxHandSize = playerDeckManager.GetMaxHandSize();
        cardUI.ClearHand();
        List<Card> discardHand;
        List<Card> newHand;
        if (actionData.CurrentTurnAction == ActionManager.CurrentTurnAction.Attack)
        {
            discardHand = playerDeckManager.GetAttackHand();
            playerDeckManager.DiscardFromAttackHand(discardHand);
            playerDeckManager.DrawToAttackHand(maxHandSize);
            newHand = playerDeckManager.GetAttackHand();
        }
        else
        {
            discardHand = playerDeckManager.GetDefenseHand();
            playerDeckManager.DiscardFromDefenseHand(discardHand);
            playerDeckManager.DrawToDefenseHand(maxHandSize);
            newHand = playerDeckManager.GetDefenseHand();
        }

        cardUI.InitializeHand(newHand);
        
        resetHandCount++;
    }

    private void OnShuffle()
    {
        if (actionData.CurrentTurnAction == ActionManager.CurrentTurnAction.Attack)
        {
            playerDeckManager.ShuffleAttackDeck();
        } else {
            playerDeckManager.ShuffleDefenseDeck();
        }
        
        deckIndicator.GetComponent<RotateCard>().Rotate();
        totalShuffleCount++;
        shuffleCount++;
    }

    private void OnPickCard()
    {
        Card newcard = new();
        if (actionData.CurrentTurnAction == ActionManager.CurrentTurnAction.Attack)
        {
            newcard = playerDeckManager.DrawOneToAttackHand();
        } else {
            newcard = playerDeckManager.DrawOneToDefenseHand();
        }
        cardUI.AddCard(newcard);
        pickCardCount++;
    }
}
