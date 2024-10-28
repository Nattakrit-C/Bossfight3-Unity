using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPSystemScript : MonoBehaviour
{
    // Player stats
    public int currentLevel = 1;
    public int currentXP = 0;
    public int currentGold = 0;

    // Configurable values
    public int xpToNextLevel = 100;
    public int xpIncrementPerLevel = 50; // Increase required XP with each level
    public int goldRewardOnLevelUp = 50;

    // Method to add XP
    public void AddXP(int xp)
    {
        currentXP += xp;
        Debug.Log("Gained " + xp + " XP. Total XP: " + currentXP);

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    // Method to add Gold
    public void AddGold(int gold)
    {
        currentGold += gold;
        Debug.Log("Gained " + gold + " gold. Total Gold: " + currentGold);
    }

    // Level up method
    private void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel; // Carry over excess XP
        xpToNextLevel += xpIncrementPerLevel; // Increase XP requirement for next level
        AddGold(goldRewardOnLevelUp); // Reward player with gold on level up
        Debug.Log("Level up! New Level: " + currentLevel);
    }
}
