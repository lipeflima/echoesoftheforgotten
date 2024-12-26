using System.Collections;
using UnityEngine;

public abstract class ActionManager : MonoBehaviour
{
    protected Battler currentBattler;
    protected Coroutine actionCoroutine;
    private ActionData actionData;
    
    public enum AttackStrategy { 
        ArmourBreak, 
        CriticalStrike, 
        DisruptAccuracy, 
        ExploitWeakDefense, 
        FocusedAttack, 
        OportunityStrike, 
        StatBalancer,
        FakeAttack,
        HealthRecovery,
        ManaRecovery,
    }
    public enum AttackType { Basic, FakeAttack, CardAttack }
    public enum CurrentTurnAction { Attack, Defense };
    
    public enum DefenseType { Evade, CounterAttack, CardDefense, Basic }
    public enum DefenseStrategy { BestBuff, BlockStrongAttack, CounterLowCostAttacks, PreserveEnergy, RegenerateHealth }

    public void StartAction(Battler battler)
    {
        currentBattler = battler;
        actionCoroutine = StartCoroutine(ActionFlow());
    }

    protected abstract IEnumerator ActionFlow();
}
