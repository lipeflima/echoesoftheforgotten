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
        UpdateNextButtonState();
    }

    public void Initialize(ActionData data, Action onCompleteCallback)
    {
        cardUI.ActivateSelectCard(true);
        actionData = data;
        onComplete = onCompleteCallback;
        gameObject.SetActive(true);
        nextButton.interactable = false;
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
        List<Card> attackCardsToDiscard = new();
        List<Card> defenseCardsToDiscard = new();
        foreach(var selectedCard in cardUI.GetSelectedCards())
        {
            cardUI.HighlightCard(selectedCard, false);

            foreach(var effect in selectedCard.effects)
            {
                if (actionData.CurrentTurnAction == ActionManager.CurrentTurnAction.Attack)
                {
                    attackCardsToDiscard.Add(selectedCard);
                    actionData.CombatAction.AttackerAction.CardEffects.Add(effect); 
                } else {
                    defenseCardsToDiscard.Add(selectedCard);
                    actionData.CombatAction.DefenderAction.CardEffects.Add(effect);
                }
            }

            if (actionData.CurrentTurnAction == ActionManager.CurrentTurnAction.Attack)
            {
                playerDeckManager.DiscardFromAttackHand(attackCardsToDiscard);
                actionData.CombatAction.AttackerAction.EnergyCost += selectedCard.EnergyCost;
                actionData.CardData.AttackerSelectedCards.Add(selectedCard);
            } else {
                playerDeckManager.DiscardFromDefenseHand(defenseCardsToDiscard);
                actionData.CombatAction.DefenderAction.EnergyCost += selectedCard.EnergyCost;
                actionData.CardData.DefenderSelectedCards.Add(selectedCard);
            }
        }

        cardUI.ClearSelectedCards();
        gameObject.SetActive(false);
        cardUI.DisplayCardUI(false);
        isSelectCardsActive = false;
        onComplete?.Invoke();
    }
}
