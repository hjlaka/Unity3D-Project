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
    private IAIStrategy aiStrategy;




    private Coroutine turnGoing;


    private void Awake()
    {
        pieceList = new List<Piece>();
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

        float maxWill = 0f;
        Piece maxWillingPiece = null;
        Place maxWillingPlace = null;

        Debug.Log("����ϴ� �⹰ ��: " + pieceList.Count);
        for(int i = 0; i < pieceList.Count; i++)
        {
            Debug.Log(string.Format("==============={0}�� ������ ����մϴ�.===============", pieceList[i]));
            Piece piece = pieceList[i];
            float will = 0f;
            ScoreNode scoreSet;
            Place targetPlace = piece.DesireToPlace(ref will, out scoreSet);

            //aiStrategy.AddPossibility(scoreSet, piece, targetPlace);

            Debug.Log(string.Format("{0}�� ���� {1}", pieceList[i], will));

            if(maxWill <= will)
            {
                maxWill = will;
                maxWillingPiece = pieceList[i];
                maxWillingPlace = targetPlace;
            }
        }

        Debug.Log(string.Format("{0}�� ���� {1}", maxWillingPiece, maxWill));

        if (null != maxWillingPiece)
        {
            PlaceManager.Instance.SelectPiece(maxWillingPiece);
            Debug.Log(maxWillingPiece + "�� ����. ������ " + maxWill);

            yield return new WaitForSeconds(showSelectedTime);

            maxWillingPiece.PlaceToDesire(maxWillingPlace);
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

            yield return new WaitForSeconds(showSelectedTime);

            float will = 0f;
            ScoreNode scoreSet;
            Place targetPlace = pieceList[i].DesireToPlace(ref will, out scoreSet);
            pieceList[i].PlaceToDesire(targetPlace);

            //yield return new WaitForSeconds(turnChangeTime);
            while (GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE)
            {
                yield return null;
            }
        }

        Debug.Log("AI �� ����");
        turnGoing = null;
        
    }





}
