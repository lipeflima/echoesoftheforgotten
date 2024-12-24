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
        resetHandButton.interactable = resetHandCount < maxResetHandCount;
        shuffleButton.interactable = shuffleCount < maxShuffleCount && shuffleCount < maxShuffleCountPerTurn;
        pickCardButton.interactable = pickCardCount < maxPickCardCountPerTurn && playerDeckManager.GetPlayerHand().Count < playerDeckManager.GetMaxHandSize();
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
        
        cardUI.ClearHand();
        playerDeckManager.Discard(playerDeckManager.GetPlayerHand());
        playerDeckManager.DrawCards(playerDeckManager.GetMaxHandSize());
        List<Card> newHand = playerDeckManager.GetPlayerHand();
        cardUI.InitializeHand(newHand);
        
        resetHandCount++;
    }

    private void OnShuffle()
    {
        Debug.Log("Shuffle Deck");
        playerDeckManager.Shuffle();
        deckIndicator.GetComponent<RotateCard>().Rotate();
        totalShuffleCount++;
        shuffleCount++;
    }

    private void OnPickCard()
    {
        Debug.Log("Pick Card");
        Card newcard = playerDeckManager.DrawCards(1);
        cardUI.AddCard(newcard);
        pickCardCount++;
    }
}
