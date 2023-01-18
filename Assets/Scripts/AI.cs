using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
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
    private List<Piece> aiPieceList;

    private Coroutine turnGoing;


    private void Awake()
    {
        aiPieceList = new List<Piece>();
    }

    public void AddAIPiece(Piece piece)
    {
        Debug.Log("���޵� ��: " + piece);
        aiPieceList.Add(piece);
        Debug.Log(piece + "�⹰�� AI�� �߰��ߴ�.");
    }

    public void AITurn()
    {
        if (GameManager.Instance.turnState != GameManager.TurnState.OPPONENT_TURN)
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


        if (null != aiPieceList[0])
        {
            PlaceManager.Instance.SelectPiece(aiPieceList[0]);
            Debug.Log(aiPieceList[0] + "�� ����");

            yield return new WaitForSeconds(showSelectedTime);

            aiPieceList[0].PlaceToDesire();

        }

        turnGoing = null;

    }

    private IEnumerator RunPieces()
    {
        Debug.Log("AI �� ���� ����: " + aiPieceList.Count);

        for (int i = 0; i < aiPieceList.Count; i++)
        {

            if (null == aiPieceList[i]) continue;

            PlaceManager.Instance.SelectPiece(aiPieceList[i]);
            Debug.Log(aiPieceList[i] + "�� ����");

            yield return new WaitForSeconds(showSelectedTime);

            aiPieceList[i].PlaceToDesire();

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
