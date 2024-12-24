using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private List<Battler> battlers = new List<Battler>(); // Referência lógica
    [SerializeField] private List<GameObject> battlerObjects = new List<GameObject>(); // Representações visuais
    private int currentDefenderIndex = 0;

    // Prefabs ou objetos para indicar destaque
    public GameObject attackerHighlightPrefab;
    public GameObject defenderHighlightPrefab;

    private GameObject attackerHighlight;
    private GameObject defenderHighlight;
    private TurnManager turnManager;

    public void Awake()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    // Inicializa os alvos com base nos Battlers
    public void InitializeTargets(List<Battler> battlers, List<GameObject> battlerObjects)
    {
        this.battlers = battlers;
        this.battlerObjects = battlerObjects;

        if (defenderHighlightPrefab != null && attackerHighlightPrefab)
        {
            attackerHighlight = Instantiate(attackerHighlightPrefab);
            attackerHighlight.SetActive(false);
            defenderHighlight = Instantiate(defenderHighlightPrefab);
            defenderHighlight.SetActive(false);
        }
    }

    // Troca o alvo defensor
    public void ChangeTarget(int direction)
    {
        if (battlers.Count == 0) return;

        RemoveHighlight(defenderHighlight);

        currentDefenderIndex = (currentDefenderIndex + direction + battlers.Count) % battlers.Count;

        var defender = battlerObjects[currentDefenderIndex];

        BattlerComponent targetComponent = defender.GetComponent<BattlerComponent>();
        Battler logicalDefender = targetComponent.battler;

        turnManager.currentDefender = logicalDefender;

        HighlightDefender(defender);
    }

    // Destaca o defensor
    public void HighlightDefender(GameObject targetObject)
    {
        SetHighlight(defenderHighlight, targetObject);
    }

    // Destaca o atacante
    public void HighlightAttacker(GameObject targetObject)
    {
        SetHighlight(attackerHighlight, targetObject);
    }

    // Configura o destaque em um alvo
    private void SetHighlight(GameObject highlight, GameObject target)
    {
        if (highlight == null || target == null) return;

        highlight.SetActive(true);
        highlight.transform.position = target.transform.position + Vector3.up * 1.5f; // Eleva visualmente
    }

    // Remove destaque
    private void RemoveHighlight(GameObject highlight)
    {
        if (highlight != null)
        {
            highlight.SetActive(false);
        }
    }
}
