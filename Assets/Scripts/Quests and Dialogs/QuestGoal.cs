using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoalType
{
    Kill,
    Talk,
    Find,
}

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;

    public int requiredAmount;
    public int currentAmount;

    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }

    public void EnemyKilled()
    {
        if (goalType == GoalType.Kill)
        {
            currentAmount++;
        }
    }
}
