using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Brown Potion", menuName = "Iventory/Tetris/Potion")]
public class BrownPot : TetrisItem
{
    public Sprite iconAtt1;
    public int heal;

    void OnEnable()
    {
        att1 = heal;
        att1_icon = iconAtt1;
    }

    //if the item is a brownPotion, it will override the use function, implemented in the parent class, to could heal the player
    public override void Use()
    {
        base.Use();

        Debug.Log("Heal:" + heal);
        
    }
}
