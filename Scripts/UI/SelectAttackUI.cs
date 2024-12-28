using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectAttackUI : MonoBehaviour
{
    private Action onComplete;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button basicAttackButton;
    [SerializeField] private Button fakeAttackButton;
    [SerializeField] private Button CardAttackButton;
    private ActionData actionData;

    private bool HasAttackCard()
    {
        return actionData.CardData.AttackerSelectedCards.Count > 0;
    }


    public void Initialize(ActionData data, Action onCompleteCallback)
    {
        actionData = data;
        gameObject.SetActive(true);
        onComplete = onCompleteCallback;

        CardAttackButton.interactable = HasAttackCard();

        nextButton.interactable = false; 
        nextButton.onClick.AddListener(CompleteStep);

        basicAttackButton.onClick.RemoveListener(() => {
            OnBasicAttack();
        });
        basicAttackButton.onClick.AddListener(() => {
            OnBasicAttack();
        });

        fakeAttackButton.onClick.RemoveListener(() => {
            OnFakeAttack();
        });
        fakeAttackButton.onClick.AddListener(() => {
            OnFakeAttack();
        });
        CardAttackButton.onClick.RemoveListener(() => {
            OnCardAttack();
        });
        CardAttackButton.onClick.AddListener(() => {
            OnCardAttack();
        });
    }

    private void CompleteStep()
    {
        gameObject.SetActive(false);
        basicAttackButton.interactable = true; 
        fakeAttackButton.interactable = true;
        CardAttackButton.interactable = true;
        onComplete?.Invoke();
    }

    private void OnBasicAttack()
    {
        actionData.CombatAction.AttackerAction.AttackType = ActionManager.AttackType.Basic;
        nextButton.interactable = true;
        basicAttackButton.interactable = false; 
        fakeAttackButton.interactable = true;
        CardAttackButton.interactable = HasAttackCard();
    }
    
    private void OnFakeAttack()
    {
        actionData.CombatAction.AttackerAction.AttackType = ActionManager.AttackType.FakeAttack;
        nextButton.interactable = true;
        basicAttackButton.interactable = true; 
        fakeAttackButton.interactable = false;
        CardAttackButton.interactable = HasAttackCard();
        
    }

    private void OnCardAttack()
    {
        actionData.CombatAction.AttackerAction.AttackType = ActionManager.AttackType.CardAttack;
        nextButton.interactable = true;
        basicAttackButton.interactable = true; 
        fakeAttackButton.interactable = true;
        CardAttackButton.interactable = false;
    }
}
