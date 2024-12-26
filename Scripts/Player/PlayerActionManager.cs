using System.Reflection;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    private ActionData actionData;
    private enum ActionStates { ManageHand, SelectAttackCards, SelectDefenseCards, SelectTarget, SelectAttack, SelectDefense, Confirm, None }
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

    public void Awake()
    {
        turnManager = FindObjectOfType<TurnManager>();
        playerDeckManager = gameObject.GetComponent<PlayerDeckManager>();
    }

    public void Start()
    {
        List<Card> currentHand = playerDeckManager.GetPlayerHand();
        cardUI.InitializeHand(currentHand);
    }

    public void StartAction(ActionData data)
    {
        actionData = data;
        StartManageHand();
        generalUI.Initialize();
    }

    private void StartManageHand()
    {
        actionData.CardData.DeckCards = playerDeckManager.GetPlayerDeckCards();
        currentState = ActionStates.ManageHand;
        generalUI.SetCurrentActionState("Manage Hand");
        cardUI.DisplayCardUI(true);
        manageHandUI.Initialize(actionData, () => {
            if (actionData.CurrentTurnAction == ActionManager.CurrentTurnAction.Attack)
            {
                GoToNextAttackState(ActionStates.SelectAttackCards);
            }
            GoToNextDefenseState(ActionStates.SelectDefenseCards);
        });
    }

    //---------------- ATACK STATES----------------//
    private void StartSelectAttackCards()
    {
        currentState = ActionStates.SelectAttackCards;
        generalUI.SetCurrentActionState("Select Attack Cards");
        selectCardsUI.Initialize(actionData, () => GoToNextAttackState(ActionStates.SelectTarget));
    }

    private void StartSelectTarget()
    {
        currentState = ActionStates.SelectTarget;
        generalUI.SetCurrentActionState("Select Target");
        selectTargetUI.Initialize(() => GoToNextAttackState(ActionStates.SelectAttack));
    }

    private void StartSelectAttack()
    {
        currentState = ActionStates.SelectAttack;
        generalUI.SetCurrentActionState("Select Attack");
        selectAttackUI.Initialize(actionData, () => GoToNextAttackState(ActionStates.Confirm));
    }

    //---------------- DEFENSE STATES----------------//

    private void StartSelectDefenseCards()
    {
        currentState = ActionStates.SelectDefenseCards;
        generalUI.SetCurrentActionState("Select Defense Cards");
        selectCardsUI.Initialize(actionData, () => GoToNextDefenseState(ActionStates.SelectDefense));
    }

    private void StartSelectDefense()
    {
        currentState = ActionStates.SelectDefense;
        generalUI.SetCurrentActionState("Select Attack");
        selectAttackUI.Initialize(actionData, () => GoToNextDefenseState(ActionStates.Confirm));
    }

    private void StartConfirmAction()
    {
        currentState = ActionStates.Confirm;
        generalUI.SetCurrentActionState("Confirm Actions");
        confirmActionUI.Initialize(ConfirmAction, CancelAction);
    }

    private void GoToNextAttackState(ActionStates state)
    {
        switch (state)
        {
            case ActionStates.SelectAttackCards: StartSelectAttackCards(); break;
            case ActionStates.SelectTarget: StartSelectTarget(); break;
            case ActionStates.SelectAttack: StartSelectAttack(); break;
            case ActionStates.Confirm: StartConfirmAction(); break;
        }
    }

    private void GoToNextDefenseState(ActionStates state)
    {
        switch (state)
        {
            case ActionStates.SelectDefense: StartSelectDefense(); break;
            case ActionStates.SelectDefenseCards: StartSelectDefenseCards(); break;
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

