using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMap
{
    private ScoreNode[,] scoreNodes = new ScoreNode[8, 8];

    
    public void ClearMap()
    {
        for(int i = 0; i < scoreNodes.GetLength(0); i++)
        {
            for(int j = 0; j < scoreNodes.GetLength(1); j++)
            {
                scoreNodes[i, j].Clear();
            }
        }
    }

    public ScoreNode GetNode(Vector2Int location)
    {
        if (location.x < 0 || location.y < 0) return null;
        if (location.x >= 8 || location.y >= 8) return null;

        return scoreNodes[location.x, location.y];
    }

    public void SetNode(Vector2Int location, ScoreNode scoreSet)
    {
        // 오류 상황
/*        if (location.x < 0 || location.y < 0) return;
        if (location.x >= 8 || location.y >= 8) return;*/

        scoreNodes[location.x, location.y] = scoreSet;
    }

    public void PrintMap()
    {
        string mapInfo = "";

        for (int i = 0; i < scoreNodes.GetLength(0); i++)
        {
            for (int j = 0; j < scoreNodes.GetLength(1); j++)
            {
                mapInfo += scoreNodes[i, j].WillPoint + " ";
            }
            mapInfo += "\n";
        }

        Debug.Log(mapInfo);
    }
}
