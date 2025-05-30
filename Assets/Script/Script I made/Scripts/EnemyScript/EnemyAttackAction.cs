using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{

[CreateAssetMenu(menuName = "A.I./EnemyActions/Attack Action")]
public class EnemyAttackAction : EnemyAction
{
    public bool canCombo;
    public EnemyAttackAction comboAction;

    
    public int attackScore = 3;
    public float recoveryTime = 2;

    public float maximumAttackAngle = 35;
    public float minimumAttackAngle = -35;

    public float maximumDistanceNeededToAttack = 3;
    public float minimumDistanceNeededToAttack = 0;





}//class
}//Nay