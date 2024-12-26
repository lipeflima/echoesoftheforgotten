using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.EditorTools;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    public List<Battler> battlers = new List<Battler>();
    private int currentTurnIndex = 0;
    private int energyPool = 10;
    public Battler currentDefender { get; set; }
    public Battler currentAttacker { get; set; }
    private TargetManager targetManager;
    private int turnCount = 0;
    private bool IsPlayerActionCompleted = false;

    void Start() 
    {
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
                    playerStats.Health,
                    playerStats.Mana
                );
                player.battlerGameobject = playerObject;
                var playerComponent = playerObject.AddComponent<BattlerComponent>();
                playerComponent.battler = player;
                battlers.Add(player);
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
                    enemyStats.Health,
                    enemyStats.Mana
                );
                enemy.battlerGameobject = enemyObject;
                var enemyComponent = enemyObject.AddComponent<BattlerComponent>();
                enemyComponent.battler = enemy;
                battlers.Add(enemy);
            }
        }

        battlers = battlers.OrderByDescending(b => b.Initiative).ToList();
        currentAttacker = battlers[0];
        currentDefender = battlers.FirstOrDefault(b => b != currentAttacker);
        InitializeTargets();
    }

    private IEnumerator TurnCycle()
    {
        while (turnCount <= 10)
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

            if (currentAttacker.IsPlayer)
            {
                // Espera a interação do jogador
                yield return StartCoroutine(WaitForPlayerAction("attack"));
            }
            else
            {
                // Ação automática para NPCs
                currentAttacker.TakeAction();
                yield return new WaitForSeconds(2f);
            }

            if (currentDefender.IsPlayer)
            {
                yield return StartCoroutine(WaitForPlayerAction("defend"));
            }
            else
            {
                currentDefender.Defend(currentAttacker);
                yield return new WaitForSeconds(2f);
            }

            // TurnResolver resolver = new TurnResolver();
            // resolver.ResolveTurn(attackerData, defenderData);


            NextTurn();
        }
    }

    private IEnumerator WaitForPlayerAction(string action)
    {
        IsPlayerActionCompleted = false;
        if (action == "attack")
        {
            currentAttacker.TakeAction();
            yield return new WaitUntil(() => IsPlayerActionCompleted);
        }
        else
        {
            currentDefender.Defend(currentAttacker);
            yield return new WaitUntil(() => IsPlayerActionCompleted);
        }
    }

    private void NextTurn()
    {
        turnCount++;
        currentTurnIndex = (currentTurnIndex + 1) % battlers.Count;
    }

    public void InitializeTargets()
    {
        targetManager = FindObjectOfType<TargetManager>();

        if (targetManager != null)
        {
            List<GameObject> battlerObjects = battlers.Select(b => b.battlerGameobject).ToList();
            targetManager.InitializeTargets(battlers, battlerObjects);
        }
    }

    public void SetPlayerActionCompleted()
    {
        IsPlayerActionCompleted = true;
    }

    public int GetAvailableEnergy()
    {
        return energyPool;
    }
}
