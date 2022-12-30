using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceGrid : MonoBehaviour
{
    [SerializeField]
    private Place placePrefab;


    [Header("Size")]
    [SerializeField]
    private Vector2Int gridSize;
    [SerializeField]
    private float placeSize;


    private Place[,] board;

    //private Dictionary<Vector2Int, Place> board = new Dictionary<Vector2Int, Place>();

    private void Awake()
    {
        board = new Place[gridSize.x, gridSize.y];
    }
    private void Start()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                //Place placePrefab = ((x + y) % 2 == 0) ? placePrefabA : placePrefabB;

                Vector3 center = new Vector3(x, 0, y) * placeSize + transform.position;
                Vector3 size = new Vector3(placeSize, 0, placeSize);
                Place instance = Instantiate(placePrefab, center, Quaternion.identity);

                instance.type = ((x + y) % 2 == 0) ? Place.PlaceType.A : Place.PlaceType.B;

                instance.gameObject.name = "Place" + x + y;

                board[x, y] = instance;
                //board.Add(new Vector2Int(y, x), instance);
            }
        }

        PlaceManager.Instance.places = board;   // 나는 주소 복사를 원하는데 배열에 대해서는 값 복사가 일어나고 있을지도 모른다.
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
