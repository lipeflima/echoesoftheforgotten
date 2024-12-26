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
    public int maxResetHandCount = 2;
    public int maxShuffleCount = 3;
    public int maxShuffleCountPerTurn, maxPickCardCountPerTurn = 1;
    public int resetHandCount, pickCardCount, shuffleCount, totalShuffleCount = 0;

    private void Update()
    {
        int handCount = actionData.CurrentTurnAction == ActionManager.CurrentTurnAction.Attack ? playerDeckManager.GetAttackHand().Count : playerDeckManager.GetDefenseHand().Count;

        resetHandButton.interactable = resetHandCount < maxResetHandCount;
        shuffleButton.interactable = shuffleCount < maxShuffleCount && shuffleCount < maxShuffleCountPerTurn;
        pickCardButton.interactable = pickCardCount < maxPickCardCountPerTurn && handCount < playerDeckManager.GetMaxHandSize();
    }

    public void Initialize(ActionData data, Action onCompleteCallback)
    {
        pickCardCount = 0;
        shuffleCount = 0;
        actionData = data;
        gameObject.SetActive(true);
        onComplete = onCompleteCallback;
        cardUI.ActivateSelectCard(true);
        nextButton.interactable = true; 
        nextButton.onClick.AddListener(CompleteStep);

        foreach (var cardObject in cardUI.GetCardObjects())
        {
            cardObject.GetComponent<CardInteraction>().SetClickable(true);
        }

        resetHandButton.onClick.AddListener(() => {
            OnResetHand();
        });
        shuffleButton.onClick.AddListener(() => {
            OnShuffle();
        });
        pickCardButton.onClick.AddListener(() => {
            OnPickCard();
        });
    }

    private void CompleteStep()
    {
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    private void OnResetHand()
    {
        Debug.Log("Reset Hand");
        _ = new List<Card>();
        var maxHandSize = playerDeckManager.GetMaxHandSize();
        cardUI.ClearHand();
        List<Card> discardHand;
        List<Card> newHand;
        if (actionData.CurrentTurnAction == ActionManager.CurrentTurnAction.Attack)
        {
            Debug.Log("Resetando hand de ataque");
            discardHand = playerDeckManager.GetAttackHand();
            playerDeckManager.DiscardFromAttackHand(discardHand);
            playerDeckManager.DrawToAttackHand(maxHandSize);
            newHand = playerDeckManager.GetAttackHand();
        }
        else
        {
            Debug.Log("Resetando hand de defesa");
            discardHand = playerDeckManager.GetDefenseHand();
            playerDeckManager.DiscardFromDefenseHand(discardHand);
            // playerDeckManager.DrawToDefenseHand(maxHandSize);
            newHand = playerDeckManager.GetDefenseHand();
        }

        cardUI.InitializeHand(newHand);
        
        resetHandCount++;
    }

    private void OnShuffle()
    {
        Debug.Log("Shuffle Deck");
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
        Debug.Log("Pick Card");
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
