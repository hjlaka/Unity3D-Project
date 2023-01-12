using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChangeLayerFunc))]
public class PieceCreateButton : MonoBehaviour
{
    [SerializeField]
    private Piece piecePrefab;
    [SerializeField]
    private TeamData team;
    [SerializeField]
    private CharacterData testCharacter;

    [SerializeField]
    private AI aiManager;

    
    private Transform pieceZone;
    private Place creatingPlace;
    private ChangeLayerFunc changeLayer;
    private void Awake()
    {
        pieceZone = GameObject.Find("PieceZone").transform;
        creatingPlace = GameObject.Find("CreatingPlace").GetComponent<Place>();         // �� ��ȯ�� �ƴ϶� ������Ʈ �������Ⱑ �³�?
        changeLayer = GetComponent<ChangeLayerFunc>();
    }

    public void CreatePiece()
    {
        if (creatingPlace == null || creatingPlace.piece != null) return;
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE) return;

        Debug.Log("�⹰�� �����߽��ϴ�." + piecePrefab.name);
        Piece instance = Instantiate(piecePrefab, pieceZone);
        instance.team = team;
        instance.character = testCharacter;

        changeLayer.ChangeLayerRecursively(instance.transform, pieceZone.gameObject.layer);

        instance.SetInPlace(creatingPlace);
        Debug.Log("������ ��: " + instance);


        if (team.direction == TeamData.Direction.UpToDown)
            aiManager.AddAIPiece(instance);
        else
            PlayerDataManager.Instance.AddPlayerPiece(instance);
        
    }

    

}

    
