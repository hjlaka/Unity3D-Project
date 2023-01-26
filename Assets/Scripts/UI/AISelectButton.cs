using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISelectButton : MonoBehaviour
{
    [SerializeField]
    private AIStrategy.AIStrategyType aiStrategyType;

    public void SetAI()
    {
        Debug.Log("AI ���� ����: " + aiStrategyType);
        
        GameManager.Instance.aiManager.SetStrategy(aiStrategyType);
    }
}
