using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    // AI편의 기물을 보유한다.


    // 기물들이 갈 수 있는 모든 경우의 수를 확보한다.
    // <기물, 이동할 좌표> 식으로 저장?

    // 최선의 수를 도출한다.
    // 어떻게 계산하지? 수마다 점수를 부여해야 하나?
    // 이동 후의 결과도 따져서 결정해야 하나?

    // 각 기물이 취할 수 있는 최선의 수 도출

    // 여러 기물이 동시에 이동할 수 있는지 없는지는 모르겠다.


    // 1. 기물의 차례가 되었을 때 기물이 알아서 행동하는 방식?
    // 2. 전체 관리자가 있어서, 어떤 기물을 움직일지 선택하는 방식? - 기물이 팀을 위한 행동을 조금 더 하게 되려나?


    // 움직일 기물을 모두 모은 다음에
    // 모두 차례대로 움직임을 선택하도록 하기?

    // 기물 간의 차례는?


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
        aiPieceList.Add(piece);
        Debug.Log(piece + "기물을 AI에 추가했다.");
    }

    public void AITurn()
    {
        if (GameManager.Instance.turnState != GameManager.TurnState.OPPONENT_TURN)
        {
            Debug.Log("AI 차례 단계가 아님");
            return;
        }
        if (turnGoing != null)
        {
            Debug.Log("이미 진행되는 턴이 있음");
            return;
        }
            

        Debug.Log("AI 턴 실행");


        // 하나의 기물이 움직임
        turnGoing = StartCoroutine(RunOnePiece());
        // 여러 기물 연쇄적으로 움직임
        //turnGoing = StartCoroutine(RunPieces());

    }

    private IEnumerator RunOnePiece()
    {
        // 기물 선정

        // 기물마다 점수가 있다.
        // 움직이고자 하는 의지가 있다(?)
        // 의지가 가장 높은 아이가 움직인다?



        // 기물 움직임

        float maxWill = 0f;
        Piece maxWillingPiece = null;
        Place maxWillingPlace = null;

        for(int i = 0; i < aiPieceList.Count; i++)
        {
            float will = 0f;
            Place targetPlace = aiPieceList[i].DesireToPlace(ref will);

            Debug.Log(string.Format("{0}의 의지 {1}", aiPieceList[i], will));

            if(maxWill <= will)
            {
                maxWill = will;
                maxWillingPiece = aiPieceList[i];
                maxWillingPlace = targetPlace;
            }
        }

        Debug.Log(string.Format("{0}의 의지 {1}", maxWillingPiece, maxWill));

        if (null != maxWillingPiece)
        {
            PlaceManager.Instance.SelectPiece(maxWillingPiece);
            Debug.Log(maxWillingPiece + "턴 시작. 의지는 " + maxWill);

            yield return new WaitForSeconds(showSelectedTime);

            maxWillingPiece.PlaceToDesire(maxWillingPlace);
        }
        else
        {
            Debug.Log("선정된 기물 없음");
            // 무승부 판정
        }

        turnGoing = null;

    }


    private IEnumerator RunPieces()
    {
        Debug.Log("AI 턴 실행 개수: " + aiPieceList.Count);

        for (int i = 0; i < aiPieceList.Count; i++)
        {

            if (null == aiPieceList[i]) continue;

            PlaceManager.Instance.SelectPiece(aiPieceList[i]);
            Debug.Log(aiPieceList[i] + "턴 시작");

            yield return new WaitForSeconds(showSelectedTime);

            float will = 0f;
            Place targetPlace = aiPieceList[i].DesireToPlace(ref will);
            aiPieceList[i].PlaceToDesire(targetPlace);

            //yield return new WaitForSeconds(turnChangeTime);
            while (GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE)
            {
                yield return null;
            }
        }

        Debug.Log("AI 턴 종료");
        turnGoing = null;
        
    }





}
