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


    public void AddAIPiece(Piece piece)
    {
        Debug.Log("���޵� ��: " + piece);
        aiPieceList.Add(piece);
        Debug.Log(piece + "�⹰�� AI�� �߰��ߴ�.");
    }

    public void AITurn()
    {
        if (!GameManager.Instance.isPlayerTurn)
        {
            Debug.Log("�÷��̾��� ���ʰ� �ƴ�");
            return;
        }
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE)
        {
            Debug.Log("�⹰ ���� �ܰ谡 �ƴ�");
            return;
        }
        if (turnGoing != null)
        {
            Debug.Log("�̹� ����Ǵ� ���� ����");
            return;
        }
            

        Debug.Log("AI �� ����");

        turnGoing = StartCoroutine(RunPieces());

    }

    private IEnumerator RunPieces()
    {
        GameManager.Instance.isPlayerTurn = false;
        //GameManager.Instance.state = GameManager.GameState.AI_TURN;
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
        //GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;
        GameManager.Instance.isPlayerTurn = true;
        turnGoing = null;
        
    }

    /*private IEnumerator WaitForReadyNext()
    {
        while(GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE)
        {
            yield return null;
        }
    }*/




}