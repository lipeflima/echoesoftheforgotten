using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectCardsUI : MonoBehaviour
{
    [SerializeField] private Button nextButton; // Botão de avançar
    [SerializeField] private TMP_Text energyText; // Texto para exibir a energia disponível

    private Action onComplete;
    private ActionData actionData;
    [SerializeField] private CardUI cardUI;
    private int availableEnergy;
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

        availableEnergy = GetCurrentEnergy();
        UpdateEnergyText();

        gameObject.SetActive(true);

        nextButton.interactable = false;
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
            gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        } else {
            gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
    }

    private void CompleteStep()
    {
        foreach(var selectedCard in cardUI.GetSelectedCards())
        {
            cardUI.HighlightCard(selectedCard, false);

            foreach(var effect in selectedCard.effects)
            {
                if (effect.effectType == Card.CardType.Attack || effect.effectType == Card.CardType.Debuff)
                {
                    actionData.CombatAction.AttackerAction.CardEffects.Add(effect); 
                } else {
                    actionData.CombatAction.DefenderAction.CardEffects.Add(effect);
                }
            }
        }

        cardUI.ClearSelectedCards();
        gameObject.SetActive(false);
        cardUI.DisplayCardUI(false);
        isSelectCardsActive = false;
        onComplete?.Invoke();
    }

    private int GetCurrentEnergy()
    {
        // Simule a energia atual do jogador (adapte para o sistema de energia)
        return 10;
    }

    private void UpdateEnergyText()
    {
        energyText.text = $"Energy: {availableEnergy}";
    }
}
