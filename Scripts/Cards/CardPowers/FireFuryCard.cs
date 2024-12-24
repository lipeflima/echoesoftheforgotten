using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFuryBehavior : CardBehavior
{
    public override void ExecuteAction(CharacterStats user, CharacterStats target)
    {
        int damage = 20; // Dano fixo
        Debug.Log($"{user.name} usou Fúria Ígnea em {target.name}, causando {damage} de dano!");
    }
}
