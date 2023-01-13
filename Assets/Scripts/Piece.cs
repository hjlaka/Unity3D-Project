using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Piece : LifeUnit
{
    [Header("InGame")]
    public Place place;
    public TeamData team;
    [SerializeField]
    private Color mouseOver;

    public int forwardY;

    protected int pieceScore;
    public int PieceScore { get { return pieceScore; } private set { pieceScore = value; } }


    public IReturnHeat returnHeat;

    [Header("Charecter")]
    [SerializeField]
    public CharacterData character;

    private Renderer render;
    private Color curNormal;

    private IDecidePlaceStrategy decideDesireStrategy;

    protected IPieceMovable movePattern;


    private StateLists recognized;
    public StateLists Recognized { get { return recognized; } private set { recognized = value; } }

    //private List<Piece> defendFor;
    //private List<Piece> threatTo;
    //private List<Place> movableTo;
    //private List<Place> influenceable;

    #region ����Ʈ ������Ƽ
    /*public List<Piece> DefendFor
    {
        get { return defendFor; }
        private set 
        { 
            defendFor = value;
        }
    }
    
    public List<Piece> ThreatTo
    {
        get { return threatTo; }
        private set 
        { 
            threatTo = value;
        }
    }*/

    /*public List<Place> MovableTo
    {
        get { return movableTo; } 
        private set { movableTo = value;  }
    }*/

   /* public List<Place> Influenceable
    {
        get { return influenceable; }
        private set { influenceable = value; }
    }*/

    #endregion

    protected virtual void Awake()
    {
        render = GetComponentInChildren<Renderer>();
        /*defendFor = new List<Piece>();
        threatTo = new List<Piece>();
        movableTo = new List<Place>();*/
        recognized = new StateLists();
        //influenceable = new List<Place>();
    }

    private void Start()
    {
        ApplyTeamInfo();

        if (place != null)
        {
            SetInPlace(place);
        }   
    }

    public void PlaceToDesire()
    {

        //PlaceManager.Instance.SelectPiece(this);

        if(Recognized.movable.Count <= 0)
        {
            Debug.Log("�̵��� �� �ִ� ���� ����");
            PlaceManager.Instance.CancleSelectPiece();
            return;
        }

        Place targetPlace = decideDesireStrategy.DecidePlace(this);
        if(targetPlace != null)
        {
            if(targetPlace.piece != null)
            {
                Debug.Log("��������!");
                //AttackTo(targetPlace);
                PlaceManager.Instance.Attack(this, targetPlace.piece);
            }
            else
            {
                Debug.Log("�̵�����!");
                PlaceManager.Instance.MoveProcess(this, targetPlace);
            }   
        }
        else
        {
            Debug.Log("�����̰� ���� ���� ����");
            PlaceManager.Instance.CancleSelectPiece();
        }
        //Place targetPlace;
        //decideDesireStrategy.DecidePlace(out targetPlace);

    }

    private void ApplyTeamInfo()
    {
        curNormal = team.normal;
        render.material.color = curNormal;

        if (team.direction == TeamData.Direction.UpToDown)
        {
            forwardY = -1;
            returnHeat = new TopTeam();
            transform.Rotate(Vector3.up * 180);
        } 
        else
        {
            forwardY = 1;
            returnHeat = new BottomTeam();
        }

        decideDesireStrategy = character.DecidePlaceStrategy;
    }

    #region ����Ʈ ����
    /*public void ClearMovable()
    {
        //Debug.Log("�̵� Ŭ����" + MovableTo.Count);
        MovableTo.Clear();
    }

    // �ӽ� ���� 
    public void ClearThreat()
    {
        //Debug.Log("���� Ŭ����" + ThreatTo.Count);
        ThreatTo.Clear();
    }

    public void ClearDefence()
    {
        //Debug.Log("��� Ŭ����" + DefendFor.Count);
        DefendFor.Clear();
    }

    public void ClearInfluence()
    {
        //Debug.Log("����� Ŭ����" + Influenceable.Count);
        Influenceable.Clear();
    }*/
    // ������� �ӽ� ����

/*    public void AddMovable(Place place)
    {
        movableTo.Add(place);
    }*/
   /* public void AddInfluence(Place place)
    {
        recognized.AddInfluenceable(place);

        if (team.direction == TeamData.Direction.DownToUp)
            place.HeatPointBottomTeam++;
        else
            place.HeatPointTopTeam++;

    }
    public void AddDefence(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "�� ��ȣ�Ѵ�");
        recognized.AddDefending(piece);
        //DialogueManager.Instance.AddDialogue(ref character.characterName, ref character.defending);
    }*/

    public void EndDefence(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "�� ���̻� ��ȣ���� �ʴ´�.");
    }

    public void BeDefended(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "���� ��ȣ �޴´�");
    }

    public void EndDefended(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "�κ��� ���̻� ��ȣ���� �ʴ´�.");
    }

    /*public void AddThreat(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "�� �����Ѵ�");
        ThreatTo.Add(piece);
        //DialogueManager.Instance.AddDialogue(ref character.characterName, ref character.threatening);
    }*/

    public void EndThreat(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "�� ���̻� �������� �ʴ´�.");
    }

    public void BeThreatened(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "���� ���� ���Ѵ�");
    }

    public void EndThreatened(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "�κ��� ���̻� �������� �ʴ´�.");
    }

    #endregion
    public virtual void SetInPlace(Place place)
    {
        this.place = place;
        place.piece = this;
        Move();
    }

    public void Move()
    {
        transform.position = place.transform.position;
    }


    public virtual void PieceAction() { }
    public virtual void RecognizeRange(Vector2Int location)
    {
        movePattern.RecognizeRange(location);
    }

    public void SetDecidePlaceStrategy(IDecidePlaceStrategy decidePlaceStrategy)
    {
        decideDesireStrategy = decidePlaceStrategy;
    }

    public virtual List<Place> ReturnMovablePlaces(Vector2Int location)
    {
        return null;
    }




    // ----------------------------------------------------------- �� �������� ���� �߰� { 
    
    private void OnMouseOver()
    {
        //Debug.Log(string.Format("{0} ���콺 ����Ŵ", gameObject.name));
        render.material.color = mouseOver;
    }

    private void OnMouseExit()
    {
        //Debug.Log(string.Format("{0} ������ ���콺 ����", gameObject.name));
        render.material.color = curNormal;
    }

    private void OnMouseUpAsButton()
    {
        /*if (!GameManager.Instance.isPlayerTurn)
        {
            Debug.Log("�÷��̾��� ���ʰ� �ƴմϴ�.");
            return;
        }*/
        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
        {
            //Debug.Log(string.Format("{0} Ŭ��", gameObject.name));
            PlaceManager.Instance.SelectPiece(this);
        }
        
        else if(GameManager.Instance.state == GameManager.GameState.SELECTING_PLACE)
        {
            // �ڽ��̶��
            if (PlaceManager.Instance.SelectedPiece == this)
                PlaceManager.Instance.CancleSelectPiece();

            // ���� �� �⹰�̶��
            else if (this.team.TeamId == PlaceManager.Instance.SelectedPiece.team.TeamId)
            {
                PlaceManager.Instance.CancleSelectPiece();
                PlaceManager.Instance.SelectPiece(this);
            }
            // �ٸ� �� �⹰�̶��
            else
            {
                // ������ �� �ִٸ�
                if(place.IsAttackableByCurPiece)
                {
                    //���� 
                    //BeAttackedBy(PlaceManager.Instance.SelectedPiece);
                    PlaceManager.Instance.Attack(PlaceManager.Instance.SelectedPiece, this);
                   
                }
            }

        }
    }
    private void AttackTo(Place place)
    {
        PlaceManager.Instance.Attack(this, place.piece);
        //PlaceManager.Instance.ExpelPiece(place.Piece);
        //PlaceManager.Instance.MoveProcess(this, place);
    }
    private void BeAttackedBy(Piece piece)
    {
        Place attackPlace = this.place;
        PlaceManager.Instance.ExpelPiece(this);
        PlaceManager.Instance.MoveProcess(piece, attackPlace);
    }



    public void ChangeColor()
    {
        curNormal = team.normal;
        render.material.color = curNormal;
    }
    public void ChangeColor(Color color)
    {
        curNormal = color;
        render.material.color = curNormal;

    }
}

