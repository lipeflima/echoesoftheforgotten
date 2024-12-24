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
    public void Initialize(ActionData data, Action onCompleteCallback)
    {
        actionData = data;
        gameObject.SetActive(true);
        onComplete = onCompleteCallback;

        nextButton.interactable = false; 
        nextButton.onClick.AddListener(CompleteStep);

        basicAttackButton.onClick.AddListener(() => {
            OnBasicAttack();
        });
        fakeAttackButton.onClick.AddListener(() => {
            OnFakeAttack();
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
        Debug.Log($"Player irá usar um ataque basico");
        // actionData.attack = ActionData.AttackType.Basic;
        nextButton.interactable = true;
        basicAttackButton.interactable = false; 
        fakeAttackButton.interactable = true;
        CardAttackButton.interactable = true;
    }
    
    private void OnFakeAttack()
    {
        Debug.Log($"Player irá fingir um ataque");
        // actionData.attack =ActionData.AttackType.Fake;
        nextButton.interactable = true;
        basicAttackButton.interactable = true; 
        fakeAttackButton.interactable = false;
        CardAttackButton.interactable = true;
        
    }

    private void OnCardAttack()
    {
        Debug.Log($"Player irá usar um ataque especial de carta");
        // actionData.attack = ActionData.AttackType.CardAttack;
        nextButton.interactable = true;
        basicAttackButton.interactable = true; 
        fakeAttackButton.interactable = true;
        CardAttackButton.interactable = false;
    }
}
