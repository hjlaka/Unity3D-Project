using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceGrid : MonoBehaviour
{
    [SerializeField]
    private Place placePrefabA;
    [SerializeField]
    private Place placePrefabB;

    [Header("Size")]
    [SerializeField]
    private Vector2Int gridSize;
    [SerializeField]
    private float placeSize;


    //private Place[][] board;

    private Dictionary<Vector2Int, Place> board = new Dictionary<Vector2Int, Place>();


    private void Start()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Place placePrefab = ((x + y) % 2 == 0) ? placePrefabA : placePrefabB;

                Vector3 center = new Vector3(x, 0, y) * placeSize + transform.position;
                Vector3 size = new Vector3(placeSize, 0, placeSize);
                Place instance = Instantiate(placePrefab, center, Quaternion.identity);
                //board[x][y] = instance;
                board.Add(new Vector2Int(y, x), instance);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        for(int y = 0; y < gridSize.y; y++)
        {
            for(int x = 0; x < gridSize.x; x++)
            {
                Vector3 center = new Vector3(x, 0, y) * placeSize + transform.position;
                Vector3 size = new Vector3(placeSize, 0, placeSize);
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}
