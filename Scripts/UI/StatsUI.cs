using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private GameObject EnemyContainer; // Container onde os stats serão exibidos
    [SerializeField] private GameObject PlayerContainer;
    [SerializeField] private GameObject statPrefab; // Prefab para cada linha de estatística

    void Update()
    {
        // Verifica se a tecla "E" foi pressionada
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Alterna o estado ativo/inativo do painel
            PlayerContainer.SetActive(!PlayerContainer.activeSelf);
            EnemyContainer.SetActive(!EnemyContainer.activeSelf);
        }
    }
    public void CreateStatsUI(Battler battler)
    {
        GameObject container = battler.IsPlayer ? PlayerContainer : EnemyContainer;
        // Limpar UI existente
        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }

        // Adicionar estatísticas
        AddStat("Name", battler.Name, container);
        AddStat("Initiative", battler.Initiative.ToString(), container);
        AddStat("Is Player", battler.IsPlayer ? "Yes" : "No", container);
        AddStat("Health", battler.Health.ToString(), container);
        AddStat("Mana", battler.Mana.ToString(), container);
        AddStat("Attack", battler.Attack.ToString(), container);
        AddStat("Defense", battler.Defense.ToString(), container);
        AddStat("Dexterity", battler.Dexterity.ToString(), container);
        AddStat("Resistance", battler.Resistance.ToString(), container);
        AddStat("Mentality", battler.Mentality.ToString(), container);
        AddStat("Luck", battler.Luck.ToString(), container);
        AddStat("Critical Damage", battler.CriticalDamage.ToString("F2"), container);
        AddStat("Critical Chance", battler.CriticalChance.ToString("F2"), container);
        AddStat("Armour Penetration", battler.ArmourPenetration.ToString("F2"), container);
        AddStat("Recovery", battler.Recovery.ToString("F2"), container);
        AddStat("Absorption", battler.Absorsion.ToString("F2"), container);
        AddStat("Accuracy", battler.Accuracy.ToString("F2"), container);
    }

    private void AddStat(string label, string value, GameObject container)
    {
        GameObject statElement = Instantiate(statPrefab, container.transform);

        TMP_Text[] texts = statElement.GetComponentsInChildren<TMP_Text>();

        if (texts.Length >= 2)
        {
            texts[0].text = label;
            texts[1].text = value;
        }

        statElement.SetActive(true); // Certifique-se de ativar o prefab, se estiver desativado
    }
}
