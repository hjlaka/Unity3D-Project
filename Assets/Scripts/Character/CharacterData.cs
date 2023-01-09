using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Drama/Dialogue")]

public class CharacterData : ScriptableObject
{
    public string characterName;

    public IDecidePlaceStrategy.StategyType decidePlaceStrategyType;

    // 동적 할당 과정은 이곳 말고 Piece 클래스에 있어도 될 것 같다.
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

}
