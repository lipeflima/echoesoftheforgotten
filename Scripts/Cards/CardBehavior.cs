using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardBehavior
{
    public abstract void ExecuteAction(CharacterStats user, CharacterStats target);
}
