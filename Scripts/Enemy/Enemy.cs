using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class DebugLogger
{
    public static void LogProperties(object obj, int level = 0, int maxDepth = 3)
    {
        if (obj == null)
        {
            Debug.Log($"{new string(' ', level * 2)}Object is null");
            return;
        }

        // Obter o tipo do objeto
        var type = obj.GetType();
        Debug.Log($"{new string(' ', level * 2)}Logging properties of object: {type.Name}");

        // Prevenir loops infinitos
        if (level >= maxDepth)
        {
            Debug.Log($"{new string(' ', (level + 1) * 2)}Max depth reached.");
            return;
        }

        // Obter as propriedades públicas
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            try
            {
                // Obter o nome e valor da propriedade
                var name = property.Name;
                var value = property.GetValue(obj, null);

                // Logar a propriedade
                Debug.Log($"{new string(' ', (level + 1) * 2)}{name}: {value}");

                // Se a propriedade é um objeto (e não um tipo primitivo ou string), logar recursivamente
                if (value != null && !(value is string) && !(value is ValueType) && !(value is IEnumerable))
                {
                    LogProperties(value, level + 1, maxDepth);
                }
            }
            catch
            {
                Debug.LogWarning($"{new string(' ', (level + 1) * 2)}Failed to log property: {property.Name}");
            }
        }
    }
}

public class Enemy : Battler
{
    public EnemyDefenseAction defenseAction;
    public Enemy(string name, int initiative, int health, int mana)
        : base(name, initiative, false, health, mana) { }

    public override void TakeAction(ActionData actionData)
    {
        Debug.Log($"{Name} está executando uma ação de IA...");
    }

    public override void Defend(ActionData actionData)
    {
        defenseAction = battlerGameobject.GetComponent<EnemyDefenseAction>();
        Debug.Log($"{Name} está se defendendo contra {actionData.Attacker.Name}");
        defenseAction.ExecuteDefense(actionData);
    }

    public override void ApplyDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
}
