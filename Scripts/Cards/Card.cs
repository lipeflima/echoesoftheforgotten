using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewCard", menuName = "CardGame/Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public int manaCost;
    public string behaviorClassName; // Nome da classe que implementa o comportamento
    public Sprite artwork;
    public enum cardType { Attack, Defense, InstantAction }
}
