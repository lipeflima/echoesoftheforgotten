using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.EditorTools;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    public List<Battler> battlers = new List<Battler>();
    private int currentTurnIndex = 0;
    public Battler currentDefender { get; set; }
    public Battler currentAttacker { get; set; }
    private TargetManager targetManager;
    private TurnResolver turnResolver;
    private ActionData actionData;
    private int turnCount = 0;
    private int cycleMana = 10;
    private bool IsPlayerActionCompleted = false;
    public bool IsCombatRunning = true;
    public MenuMain menuMain; 

    void Start() 
    {
        InitializeTurnManager();
        InitializeActionData();
        InitializeBattlers();
        StartTurnCycle();
    }

    private void StartTurnCycle()
    {
        StartCoroutine(TurnCycle());
    }

    private void InitializeBattlers()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            CharacterStats playerStats = playerObject.GetComponent<CharacterStats>();
            if (playerStats != null)
            {
                PlayerCombat player = new(
                    playerStats.Name,
                    playerStats.Initiative,
                    true,
                    playerStats.Health,
                    playerStats.Mana,
                    playerStats.Attack,
                    playerStats.Defense,
                    playerStats.Dexterity,
                    playerStats.Resistance,
                    playerStats.Mentality,
                    playerStats.Luck,
                    playerStats.CriticalDamage,
                    playerStats.CriticalChance,
                    playerStats.ArmourPenetration,
                    playerStats.Recovery,
                    playerStats.Absorsion,
                    playerStats.Accuracy
                );
                player.battlerGameobject = playerObject;
                var playerComponent = playerObject.AddComponent<BattlerComponent>();
                playerComponent.battler = player;
                battlers.Add(player);
                actionData.PlayerStats = player;
            }
        }
        
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObject in enemyObjects)
        {
            CharacterStats enemyStats = enemyObject.GetComponent<CharacterStats>();
            if (enemyStats != null)
            {
                Enemy enemy = new(
                    enemyStats.Name,
                    enemyStats.Initiative,
                    false,
                    enemyStats.Health,
                    enemyStats.Mana,
                    enemyStats.Attack,
                    enemyStats.Defense,
                    enemyStats.Dexterity,
                    enemyStats.Resistance,
                    enemyStats.Mentality,
                    enemyStats.Luck,
                    enemyStats.CriticalDamage,
                    enemyStats.CriticalChance,
                    enemyStats.ArmourPenetration,
                    enemyStats.Recovery,
                    enemyStats.Absorsion,
                    enemyStats.Accuracy
                );
                enemy.battlerGameobject = enemyObject;
                var enemyComponent = enemyObject.AddComponent<BattlerComponent>();
                enemyComponent.battler = enemy;
                battlers.Add(enemy);
                actionData.EnemiesStats.Add(enemy);
            }
        }

        battlers = battlers.OrderByDescending(b => b.Initiative).ToList();
        currentAttacker = battlers[0];
        currentDefender = battlers.FirstOrDefault(b => b != currentAttacker);
        InitializeTargets();
    }

    private IEnumerator TurnCycle()
    {
        while (IsCombatRunning)
        {
            currentAttacker = battlers[currentTurnIndex];
            if (!currentAttacker.IsPlayer)
            {
                currentDefender = battlers.Find(battler => battler.IsPlayer);
            } else {
                currentDefender = battlers.Where(b => !b.IsPlayer).OrderByDescending(b => b.Initiative).FirstOrDefault();
            }

            targetManager.HighlightAttacker(currentAttacker.battlerGameobject);
            targetManager.HighlightDefender(currentDefender.battlerGameobject);

            actionData.Attacker = currentAttacker;
            actionData.Defender = currentDefender;

            if (currentAttacker.IsPlayer)
            {
                // Espera a interação do jogador
                yield return StartCoroutine(WaitForPlayerAction("attack"));
            }
            else
            {
                // Ação automática para NPCs
                currentAttacker.TakeAction(actionData);
            }

            if (currentDefender.IsPlayer)
            {
                yield return StartCoroutine(WaitForPlayerAction("defend"));
            }
            else
            {
                yield return StartCoroutine(WaitForDefenderActionComplete());
            }

            turnResolver.ResolveTurn(actionData);
            NextTurn();
        }

        menuMain.GoToLevel("Acampamento");
    }

    private IEnumerator WaitForPlayerAction(string action)
    {
        IsPlayerActionCompleted = false;
        if (action == "attack")
        {
            actionData.CurrentTurnAction = ActionManager.CurrentTurnAction.Attack;
            currentAttacker.TakeAction(actionData);
            yield return new WaitUntil(() => IsPlayerActionCompleted);
        }
        else
        {
            actionData.CurrentTurnAction = ActionManager.CurrentTurnAction.Defense;
            currentDefender.Defend(actionData);
            yield return new WaitUntil(() => IsPlayerActionCompleted);
        }
    }

    IEnumerator WaitForDefenderActionComplete()
    {
        yield return new WaitUntil(() => IsPlayerActionCompleted);
        currentDefender.Defend(actionData);
    }

    private void NextTurn()
    {
        turnCount++;
        // AddCycleMana(currentAttacker, currentDefender);
        currentTurnIndex = (currentTurnIndex + 1) % battlers.Count;
    }

    public void InitializeTargets()
    {
        targetManager = FindObjectOfType<TargetManager>();

        if (targetManager != null)
        {
            List<GameObject> battlerObjects = battlers.Select(b => b.battlerGameobject).ToList();
            targetManager.InitializeTargets(battlers, battlerObjects, actionData);
        }
    }

    private void InitializeTurnManager()
    {
        turnResolver = FindObjectOfType<TurnResolver>();
    }

    public void InitializeActionData()
    {
        actionData = new ActionData();
    }

    public void SetPlayerActionCompleted()
    {
        IsPlayerActionCompleted = true;
    }

    private void AddCycleMana(Battler attacker, Battler defender)
    {
        attacker.Mana += cycleMana;
        defender.Mana += cycleMana;
    }
}
