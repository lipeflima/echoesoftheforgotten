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
    public SelectDefenseUI selectDefenseUI;
    public CardUI cardUI;

    public void Awake()
    {
        turnManager = FindObjectOfType<TurnManager>();
        playerDeckManager = gameObject.GetComponent<PlayerDeckManager>();
    }

    public void Start()
    {
        cardUI.InitializeData(actionData);
    }

    public void StartAction(ActionData data)
    {
        actionData = data;
        List<Card> currentHand = actionData.CurrentTurnAction == ActionManager.CurrentTurnAction.Attack 
            ? playerDeckManager.GetAttackHand() 
            : playerDeckManager.GetDefenseHand();

        cardUI.InitializeHand(currentHand);
        StartManageHand();
        generalUI.Initialize(actionData);
        cardUI.InitializeGeneralUI();
    }

    private void StartManageHand()
    {
        currentState = ActionStates.ManageHand;
        cardUI.DisplayCardUI(true);
        manageHandUI.Initialize(actionData, () => {
            if (actionData.CurrentTurnAction == ActionManager.CurrentTurnAction.Attack)
            {
                GoToNextAttackState(ActionStates.SelectAttackCards);
            } else {
                GoToNextDefenseState(ActionStates.SelectDefenseCards);
            }
        });
    }

    //---------------- ATACK STATES----------------//
    private void StartSelectAttackCards()
    {
        currentState = ActionStates.SelectAttackCards;
        selectCardsUI.Initialize(actionData, () => GoToNextAttackState(ActionStates.SelectTarget));
    }

    private void StartSelectTarget()
    {
        currentState = ActionStates.SelectTarget;
        selectTargetUI.Initialize(() => GoToNextAttackState(ActionStates.SelectAttack));
    }

    private void StartSelectAttack()
    {
        currentState = ActionStates.SelectAttack;
        selectAttackUI.Initialize(actionData, () => GoToNextAttackState(ActionStates.Confirm));
    }

    //---------------- DEFENSE STATES----------------//

    private void StartSelectDefenseCards()
    {
        currentState = ActionStates.SelectDefenseCards;
        selectCardsUI.Initialize(actionData, () => GoToNextDefenseState(ActionStates.SelectDefense));
    }

    private void StartSelectDefense()
    {
        currentState = ActionStates.SelectDefense;
        selectDefenseUI.Initialize(actionData, () => GoToNextDefenseState(ActionStates.Confirm));
    }

    //---------------- CONFIRM STATES----------------//

    private void StartConfirmAction()
    {
        currentState = ActionStates.Confirm;
        confirmActionUI.Initialize(ConfirmAction, CancelAction);
    }

    //---------------- STATE FUNCTIONS ----------------//

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
        generalUI.gameObject.SetActive(false);
        cardUI.ClearHand();
        // Aqui você aplica as ações do jogador.
        turnManager.SetPlayerActionCompleted();
    }

    private void CancelAction()
    {
        // Reinicia ou volta para o estado inicial.
        StartManageHand();
    }
}

