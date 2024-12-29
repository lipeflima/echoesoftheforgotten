using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectDefenseUI : MonoBehaviour
{
    private Action onComplete;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button BasicDefenseButton;
    [SerializeField] private Button EvadeDefenseButton;
    [SerializeField] private Button CardDefenseButton;
    [SerializeField] private Button CounterAttackButton;
    private ActionData actionData;
    private int minEvadeDefenseManaCount = 5;
    private int minCounterAttackDefenseManaCount = 8;

    public void Initialize(ActionData data, Action onCompleteCallback)
    {
        actionData = data;
        gameObject.SetActive(true);
        onComplete = onCompleteCallback;

        BasicDefenseButton.interactable = true;
        CardDefenseButton.interactable = HasDefenseCard();
        EvadeDefenseButton.interactable = HasManaToEvade();
        CounterAttackButton.interactable = HasManaToCounterAttack();

        nextButton.interactable = false; 
        nextButton.onClick.RemoveListener(CompleteStep);
        nextButton.onClick.AddListener(CompleteStep);

        BasicDefenseButton.onClick.AddListener(() => {
            OnBasicAttack();
        });
        EvadeDefenseButton.onClick.AddListener(() => {
            OnEvadeDefense();
        });
        CardDefenseButton.onClick.AddListener(() => {
            OnCardDefense();
        });
        CounterAttackButton.onClick.AddListener(() => {
            OnCounterAttack();
        });
    }

    
    private bool HasDefenseCard()
    {
        return actionData.CardData.DefenderSelectedCards.Count > 0;
    }

    private bool HasManaToEvade()
    {
        return actionData.Defender.Mana >= minEvadeDefenseManaCount;
    }

    private bool HasManaToCounterAttack()
    {
        return actionData.Defender.Mana >= minCounterAttackDefenseManaCount;
    }

    private void CompleteStep()
    {
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    private void OnBasicAttack()
    {
        actionData.CombatAction.DefenderAction.DefenseType = ActionManager.DefenseType.Basic;
        nextButton.interactable = true;
    }
    
    private void OnEvadeDefense()
    {
        actionData.CombatAction.DefenderAction.DefenseType = ActionManager.DefenseType.Evade;
        nextButton.interactable = true;
        
    }

    private void OnCardDefense()
    {
        actionData.CombatAction.DefenderAction.DefenseType = ActionManager.DefenseType.CardDefense;
        nextButton.interactable = true;
    }

    private void OnCounterAttack()
    {
        actionData.CombatAction.DefenderAction.DefenseType = ActionManager.DefenseType.CounterAttack;
        nextButton.interactable = true;
    }
}
