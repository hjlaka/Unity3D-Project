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
                default:
                    return null;
            }
        }
    }

    public string threatening;
    public string defending;
    public string attacking;

    public List<string> normalTalks;


}
