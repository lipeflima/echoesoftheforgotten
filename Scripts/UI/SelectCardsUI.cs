using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectCardsUI : MonoBehaviour
{
    [SerializeField] private Button nextButton; // Botão de avançar
    [SerializeField] private PlayerDeckManager playerDeckManager;

    private Action onComplete;
    private ActionData actionData;
    [SerializeField] private CardUI cardUI;
    public TMP_Text instruction;
    public bool isSelectCardsActive = false;

    private void Update()
    {
        UpdateInstructionState();
        // UpdateNextButtonState();
    }

    public void Initialize(ActionData data, Action onCompleteCallback)
    {
        cardUI.ActivateSelectCard(true);
        actionData = data;
        onComplete = onCompleteCallback;
        gameObject.SetActive(true);
        nextButton.interactable = true;
        nextButton.onClick.RemoveListener(CompleteStep);
        nextButton.onClick.AddListener(CompleteStep);
    }

    private void UpdateNextButtonState()
    {
        nextButton.interactable = cardUI.GetSelectedCards().Count > 0;
    }

    private void UpdateInstructionState()
    {
        if (cardUI.GetSelectedCards().Count <= 0)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        } else {
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void CompleteStep()
    {
        List<Card> cardsToRemove = cardUI.GetSelectedCards();
        List<Card> cardsFromClones = new();
        
        bool isAttack = actionData.CurrentTurnAction == ActionManager.CurrentTurnAction.Attack;
        
        foreach(var selectedCard in cardsToRemove)
        {
            cardUI.HighlightCard(selectedCard, false); 
             
            foreach(var effect in selectedCard.effects)
            {
                if (isAttack)
                {
                    actionData.CombatAction.AttackerAction.CardEffects.Add(effect); 
                } else {
                    actionData.CombatAction.DefenderAction.CardEffects.Add(effect);
                }
            }

            GetCardsFromClones(selectedCard, cardsFromClones);
        }

        if (isAttack)
        {
            playerDeckManager.DiscardFromAttackHand(cardsFromClones);
            actionData.CardData.AttackerSelectedCards = cardsFromClones;
        } else {
            playerDeckManager.DiscardFromDefenseHand(cardsFromClones);
            actionData.CardData.DefenderSelectedCards = cardsFromClones;
        }

        cardUI.ClearSelectedCards();
        cardUI.ActivateSelectCard(false);
        gameObject.SetActive(false);
        cardUI.DisplayCardUI(false);
        isSelectCardsActive = false;
        onComplete?.Invoke();
    }

    private void GetCardsFromClones(Card selectedCard, List<Card> cardsFromClones)
    {
        bool isAttack = actionData.CurrentTurnAction == ActionManager.CurrentTurnAction.Attack;

        if (isAttack)
        {
            foreach (Card card in playerDeckManager.GetAttackHand())
            {
                if (selectedCard.cardName == card.cardName)
                {
                    cardsFromClones.Add(card);
                    break;
                }
            }
        } else {
            foreach (Card card in playerDeckManager.GetDefenseHand())
            {
                if (selectedCard.cardName == card.cardName)
                {
                    cardsFromClones.Add(card);
                    break;
                }
            }
        }
    }
}
