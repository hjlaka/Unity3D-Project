using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageSetter : MonoBehaviour
{
    [SerializeField]
    private Board village;

    private void Start()
    {
        Debug.Log("보드를 찾는 중");
        village = GameObject.Find("VillageBoard").GetComponent<Board>();

        SetPlayerPieces();
    }
    private void SetPlayerPieces()
    {

        List<Piece> pieces = PlayerDataManager.Instance.PlayerPieces;
        Debug.Log("기물을 배치한다. 기물 수: " + pieces.Count);

        for (int i = 0; i < pieces.Count; i++)
        {
            Piece piece = pieces[i];

            Debug.Log("배치중인 기물: " + piece);

            Piece instance = Instantiate(piece);
            instance.IsFree = true;

            int randX = Random.Range(0, village.Size.x);
            int randY = Random.Range(0, village.Size.y);

            Debug.Log("랜덤 위치: " + randX + ", " + randY);
            Place targetPlace = village.GetPlace(new Vector2Int(randX, randY));
            instance.transform.position = targetPlace.transform.position;

        }
    }
}
