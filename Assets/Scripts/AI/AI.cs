using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player
{
    // AI���� �⹰�� �����Ѵ�.


    // �⹰���� �� �� �ִ� ��� ����� ���� Ȯ���Ѵ�.
    // <�⹰, �̵��� ��ǥ> ������ ����?

    // �ּ��� ���� �����Ѵ�.
    // ��� �������? ������ ������ �ο��ؾ� �ϳ�?
    // �̵� ���� ����� ������ �����ؾ� �ϳ�?

    // �� �⹰�� ���� �� �ִ� �ּ��� �� ����

    // ���� �⹰�� ���ÿ� �̵��� �� �ִ��� �������� �𸣰ڴ�.


    // 1. �⹰�� ���ʰ� �Ǿ��� �� �⹰�� �˾Ƽ� �ൿ�ϴ� ���?
    // 2. ��ü �����ڰ� �־, � �⹰�� �������� �����ϴ� ���? - �⹰�� ���� ���� �ൿ�� ���� �� �ϰ� �Ƿ���?


    // ������ �⹰�� ��� ���� ������
    // ��� ���ʴ�� �������� �����ϵ��� �ϱ�?

    // �⹰ ���� ���ʴ�?


    [SerializeField]
    [Range(0f, 3f)]
    private float turnChangeTime;

    [SerializeField]
    [Range(0f, 3f)]
    private float showSelectedTime;

    [SerializeField]
    private AIStrategy aiStrategy;




    private Coroutine turnGoing;


    private void Awake()
    {
        pieceList = new List<Piece>();
    }

    public void SetStrategy(AIStrategy.AIStrategyType type)
    {
        switch (type)
        {
            case AIStrategy.AIStrategyType.AGGRESIVE:
                aiStrategy = gameObject.AddComponent<AggresiveStrategy>();
                break;
            case AIStrategy.AIStrategyType.RANDOM:
                aiStrategy = gameObject.AddComponent<RandomAIStrategy>();
                break;
            case AIStrategy.AIStrategyType.WILLFIRST:
                aiStrategy = gameObject.AddComponent<WillFirstStrategy>();
                break;
        }
    }

    public override void AddPiece(Piece piece)
    {
        pieceList.Add(piece);
        Debug.Log(piece + "�⹰�� AI�� �߰��ߴ�.");
    }

    public override void DoTurn()
    {
        if (GameManager.Instance.turnState != GameManager.TurnState.TOP_TURN)
        {
            Debug.Log("AI ���� �ܰ谡 �ƴ�");
            return;
        }
        if (turnGoing != null)
        {
            Debug.Log("�̹� ����Ǵ� ���� ����");
            return;
        }
            

        Debug.Log("AI �� ����");


        // �ϳ��� �⹰�� ������
        turnGoing = StartCoroutine(RunOnePiece());
        // ���� �⹰ ���������� ������
        //turnGoing = StartCoroutine(RunPieces());

    }

    private IEnumerator RunOnePiece()
    {
        // �⹰ ����

        // �⹰���� ������ �ִ�.
        // �����̰��� �ϴ� ������ �ִ�(?)
        // ������ ���� ���� ���̰� �����δ�?



        // �⹰ ������

        Debug.Log("����ϴ� �⹰ ��: " + pieceList.Count);
        for(int i = 0; i < pieceList.Count; i++)
        {
            Debug.Log(string.Format("==============={0}�� ������ ����մϴ�.===============", pieceList[i]));
            Piece piece = pieceList[i];
            float will = 0f;
            ScoreNode scoreSet;
            Placement placement = piece.DesireToPlace(ref will, out scoreSet);
            
            if (placement != null)
            {
                Debug.Log(string.Format("�⹰ {0} �ڸ� {1}, �ڸ� �� {2} �ڸ� �� {3}", piece, placement, placement.PrevPosition, placement.NextPosition));
                aiStrategy.AddPossibility(scoreSet, piece, placement.NextPosition);
            }
        }

        Placement strategySelection = aiStrategy.GetBestInOwnWay();

        if (null != strategySelection)
        {
            Piece bestPiece = strategySelection.Piece;
            PlaceManager.Instance.SelectPiece(bestPiece);
            Debug.Log(bestPiece + "�� ����.");

            yield return new WaitForSeconds(showSelectedTime);

            bestPiece.PlaceToDesire(strategySelection.NextPosition);
        }
        else
        {
            Debug.Log("������ �⹰ ����");
            // ���º� ����
        }

        turnGoing = null;

    }


    private IEnumerator RunPieces()
    {
        Debug.Log("AI �� ���� ����: " + pieceList.Count);

        for (int i = 0; i < pieceList.Count; i++)
        {

            if (null == pieceList[i]) continue;

            PlaceManager.Instance.SelectPiece(pieceList[i]);
            Debug.Log(pieceList[i] + "�� ����");

            yield return new WaitForSeconds(0f);

            float will = 0f;
            ScoreNode scoreSet;
            Placement placement = pieceList[i].DesireToPlace(ref will, out scoreSet);
            Place targetPlace = placement.NextPosition;
            pieceList[i].PlaceToDesire(targetPlace);
            PlaceManager.Instance.SelectedPieceInit();

            //yield return new WaitForSeconds(turnChangeTime);
        }

        Debug.Log("AI �� ����");
        turnGoing = null;
        
    }





}
