using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceGrid : MonoBehaviour
{
    [SerializeField]
    private Place placePrefab;
    [SerializeField]
    private string boardName = "Board";
    [SerializeField]
    private bool followRule = true;
    [SerializeField]
    private bool isMarkable = false;


    [Header("Size")]
    [SerializeField]
    private Vector2Int gridSize;
    [SerializeField]
    private float placeSize;
    [SerializeField]
    private bool isCheck;


    private Place[,] places;

    //private Dictionary<Vector2Int, Place> board = new Dictionary<Vector2Int, Place>();

    private void Awake()
    {
        places = new Place[gridSize.x, gridSize.y];
        CreateBoard();
    }

    private void CreateBoard()
    {
        
        GameObject boardObject = new GameObject();

        if (isMarkable)
            boardObject.AddComponent<MarkableBoard>();
        else
            boardObject.AddComponent<Board>();

        boardObject.gameObject.name = boardName;

        Board board = boardObject.GetComponent<Board>();
        Debug.Log("보드를 만드는 중 " + board);
        board.Size = gridSize;
        board.FollowRule = followRule;

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {

                Vector3 center = new Vector3(x, 0, y) * placeSize + transform.position;
                Vector3 size = new Vector3(placeSize, 0, placeSize);
                Place instance = Instantiate(placePrefab, center, Quaternion.identity, board.transform);

                if (isCheck)
                    instance.type = ((x + y) % 2 == 0) ? Place.PlaceType.A : Place.PlaceType.B;
                else
                    instance.type = Place.PlaceType.V;

                instance.gameObject.name = "Place" + x + y;
                instance.boardIndex = new Vector2Int(x, y);
                instance.board = board;

                places[x, y] = instance;
            }
        }

        // 배열은 클래스다.
        board.places = places;
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
