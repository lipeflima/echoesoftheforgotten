using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectDefenseUI : MonoBehaviour
{
    private Action onComplete;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button basicDefenseButton;
    [SerializeField] private Button evadeDefenseButton;
    [SerializeField] private Button CardDefenseButton;
    [SerializeField] private Button CounterAttackButton;
    private ActionData actionData;
    public void Initialize(ActionData data, Action onCompleteCallback)
    {
        actionData = data;
        gameObject.SetActive(true);
        onComplete = onCompleteCallback;

        nextButton.interactable = false; 
        nextButton.onClick.RemoveListener(CompleteStep);
        nextButton.onClick.AddListener(CompleteStep);

        basicDefenseButton.onClick.AddListener(() => {
            OnBasicAttack();
        });
        evadeDefenseButton.onClick.AddListener(() => {
            OnEvadeDefense();
        });
        CardDefenseButton.onClick.AddListener(() => {
            OnCardDefense();
        });
        CounterAttackButton.onClick.AddListener(() => {
            OnCounterAttack();
        });
    }

    private void CompleteStep()
    {
        gameObject.SetActive(false);
        basicDefenseButton.interactable = true; 
        evadeDefenseButton.interactable = true;
        CardDefenseButton.interactable = true;
        CounterAttackButton.interactable = true;
        onComplete?.Invoke();
    }

    private void OnBasicAttack()
    {
        Debug.Log($"Player irá usar um ataque basico");
        actionData.CombatAction.DefenderAction.DefenseType = ActionManager.DefenseType.Basic;
        nextButton.interactable = true;
        basicDefenseButton.interactable = false; 
        evadeDefenseButton.interactable = true;
        CardDefenseButton.interactable = true;
        CounterAttackButton.interactable = false;
    }
    
    private void OnEvadeDefense()
    {
        Debug.Log($"Player irá fingir um ataque");
        actionData.CombatAction.DefenderAction.DefenseType = ActionManager.DefenseType.Evade;
        nextButton.interactable = true;
        basicDefenseButton.interactable = true; 
        evadeDefenseButton.interactable = false;
        CardDefenseButton.interactable = true;
        CounterAttackButton.interactable = false;
        
    }

    private void OnCardDefense()
    {
        Debug.Log($"Player irá usar um ataque especial de carta");
        actionData.CombatAction.DefenderAction.DefenseType = ActionManager.DefenseType.CardDefense;
        nextButton.interactable = true;
        basicDefenseButton.interactable = true; 
        evadeDefenseButton.interactable = true;
        CardDefenseButton.interactable = false;
        CounterAttackButton.interactable = true;
    }

    private void OnCounterAttack()
    {
        Debug.Log($"Player irá usar um ataque especial de carta");
        actionData.CombatAction.DefenderAction.DefenseType = ActionManager.DefenseType.CounterAttack;
        nextButton.interactable = true;
        basicDefenseButton.interactable = true; 
        evadeDefenseButton.interactable = true;
        CardDefenseButton.interactable = true;
        CounterAttackButton.interactable = false;
    }
}
