using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootWallBehavior : CardBehavior
{
    public override void ExecuteAction(CharacterStats user, CharacterStats target)
    {
        int shieldAmount = 15; // Quantidade de escudo
        Debug.Log($"{user.name} usou Muralha de Raízes, ganhando {shieldAmount} de escudo!");
    }
}
