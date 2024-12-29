using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectAttackUI : MonoBehaviour
{
    private Action onComplete;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button BasicAttackButton;
    [SerializeField] private Button FakeAttackButton;
    [SerializeField] private Button CardAttackButton;
    [SerializeField] private int minFakeAttackManaAmount = 3;
    private ActionData actionData;

    public void Initialize(ActionData data, Action onCompleteCallback)
    {
        actionData = data;
        gameObject.SetActive(true);
        onComplete = onCompleteCallback;

        CardAttackButton.interactable = HasAttackCard();
        FakeAttackButton.interactable = HasManaToFakeAttack();

        nextButton.interactable = false; 
        nextButton.onClick.AddListener(CompleteStep);

        BasicAttackButton.onClick.RemoveListener(() => {
            OnBasicAttack();
        });
        BasicAttackButton.onClick.AddListener(() => {
            OnBasicAttack();
        });

        FakeAttackButton.onClick.RemoveListener(() => {
            OnFakeAttack();
        });
        FakeAttackButton.onClick.AddListener(() => {
            OnFakeAttack();
        });
        CardAttackButton.onClick.RemoveListener(() => {
            OnCardAttack();
        });
        CardAttackButton.onClick.AddListener(() => {
            OnCardAttack();
        });
    }

    private bool HasAttackCard()
    {
        return actionData.CardData.AttackerSelectedCards.Count > 0;
    }

    private bool HasManaToFakeAttack()
    {
        return actionData.Attacker.Mana >= minFakeAttackManaAmount;
    }

    private void CompleteStep()
    {
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    private void OnBasicAttack()
    {
        actionData.CombatAction.AttackerAction.AttackType = ActionManager.AttackType.Basic;
        nextButton.interactable = true;
    }
    
    private void OnFakeAttack()
    {
        actionData.CombatAction.AttackerAction.AttackType = ActionManager.AttackType.FakeAttack;
        nextButton.interactable = true;
        
    }

    private void OnCardAttack()
    {
        actionData.CombatAction.AttackerAction.AttackType = ActionManager.AttackType.CardAttack;
        nextButton.interactable = true;
    }
}
