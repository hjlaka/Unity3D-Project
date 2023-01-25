using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Drama/Dialogue")]

public class CharacterData : ScriptableObject
{
    [Header("General")]
    public string characterName;
    public Piece piecePrefab;

    public IDecidePlaceStrategy.StategyType decidePlaceStrategyType;

    // ���� �Ҵ� ������ �̰� ���� Piece Ŭ������ �־ �� �� ����.
    public IDecidePlaceStrategy DecidePlaceStrategy
    {
        get
        {
            switch(decidePlaceStrategyType)
            {
                case IDecidePlaceStrategy.StategyType.AttackFirst:
                    return new AttackFirstStrategy();
                case IDecidePlaceStrategy.StategyType.DefenceFirst:
                    return new DefendFirstStategy();
                case IDecidePlaceStrategy.StategyType.SaftyFirst:
                    return new SaftyFirstStrategy();
                case IDecidePlaceStrategy.StategyType.Normal:
                    return new NormalStrategy();
                default:
                    return null;
            }
        }
    }

    public string threating;
    public string defending;
    public string attacking;
    public string called;

    public string beThreated;
    public string beDefended;
    public string beAttacked;


    public List<string> normalTalks;

}
