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


    [Header("Size")]
    [SerializeField]
    private Vector2Int gridSize;
    [SerializeField]
    private float placeSize;


    private Place[,] places;

    //private Dictionary<Vector2Int, Place> board = new Dictionary<Vector2Int, Place>();

    private void Awake()
    {
        places = new Place[gridSize.x, gridSize.y];
    }
    private void Start()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        GameObject boardObject = new GameObject();
        boardObject.AddComponent<Board>();
        boardObject.gameObject.name = boardName;

        Board board = boardObject.GetComponent<Board>();
        board.Size = gridSize;
        board.FollowRule = followRule;

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {

                Vector3 center = new Vector3(x, 0, y) * placeSize + transform.position;
                Vector3 size = new Vector3(placeSize, 0, placeSize);
                Place instance = Instantiate(placePrefab, center, Quaternion.identity, board.transform);

                instance.type = ((x + y) % 2 == 0) ? Place.PlaceType.A : Place.PlaceType.B;

                instance.gameObject.name = "Place" + x + y;
                instance.boardIndex = new Vector2Int(x, y);
                instance.board = board;

                places[x, y] = instance;
            }
        }

        // 나는 주소 복사를 원하는데 배열에 대해서는 값 복사가 일어나고 있을지도 모른다.
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
