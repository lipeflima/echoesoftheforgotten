using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    private ActionData actionData;
    private enum ActionStates { ManageHand, SelectCards, SelectTarget, SelectAttack, Confirm, None }
    private ActionStates currentState = ActionStates.None;
    private TurnManager turnManager;
    [SerializeField] private PlayerDeckManager playerDeckManager;
    public GeneralUI generalUI;
    public ManageHandUI manageHandUI;
    public ConfirmActionUI confirmActionUI; 
    public SelectCardsUI selectCardsUI;
    public SelectTargetUI selectTargetUI;
    public SelectAttackUI selectAttackUI;
   

    public CardUI cardUI;

    private void Start()
    {
        actionData = new ActionData();
        turnManager = FindObjectOfType<TurnManager>();
        playerDeckManager = GetComponent<PlayerDeckManager>();
        List<Card> currentHand = playerDeckManager.GetPlayerHand();
        cardUI.InitializeHand(currentHand);
        actionData.hand = currentHand;
        actionData.deckCards = playerDeckManager.GetPlayerDeckCards();
    }

    public void StartAction()
    {
        StartManageHand();
        generalUI.Initialize();
    }

    private void StartManageHand()
    {
        currentState = ActionStates.ManageHand;
        generalUI.SetCurrentActionState("Manage Hand");
        cardUI.DisplayCardUI(true);
        manageHandUI.Initialize(actionData, () => GoToNextState(ActionStates.SelectCards));
    }

    private void StartSelectCards()
    {
        currentState = ActionStates.SelectCards;
        generalUI.SetCurrentActionState("Select Cards");
        selectCardsUI.Initialize(actionData, () => GoToNextState(ActionStates.SelectTarget));
    }

    private void StartSelectTarget()
    {
        currentState = ActionStates.SelectTarget;
        generalUI.SetCurrentActionState("Select Target");
        selectTargetUI.Initialize(() => GoToNextState(ActionStates.SelectAttack));
    }

    private void StartSelectAttack()
    {
        currentState = ActionStates.SelectAttack;
        generalUI.SetCurrentActionState("Select Attack");
        selectAttackUI.Initialize(actionData, () => GoToNextState(ActionStates.Confirm));
    }

    private void StartConfirmAction()
    {
        currentState = ActionStates.Confirm;
        generalUI.SetCurrentActionState("Confirm Actions");
        confirmActionUI.Initialize(ConfirmAction, CancelAction);
    }

    private void GoToNextState(ActionStates state)
    {
        switch (state)
        {
            case ActionStates.SelectCards: StartSelectCards(); break;
            case ActionStates.SelectTarget: StartSelectTarget(); break;
            case ActionStates.SelectAttack: StartSelectAttack(); break;
            case ActionStates.Confirm: StartConfirmAction(); break;
        }
    }

    private void ConfirmAction()
    {
        Debug.Log("Action Confirmed!");
        generalUI.gameObject.SetActive(false);
        // Aqui você aplica as ações do jogador.
        turnManager.SetPlayerActionCompleted();
    }

    private void CancelAction()
    {
        Debug.Log("Action Canceled!");
        // Reinicia ou volta para o estado inicial.
        StartManageHand();
    }
}

