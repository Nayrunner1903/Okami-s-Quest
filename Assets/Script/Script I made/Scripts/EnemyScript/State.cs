using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nay{
    public abstract class State : MonoBehaviour
    {

        public abstract State Tick(EnemyManager enemyManager , EnemyStats enemyStats , EnemyAnimatorManager enemyAnimatorManager);













    }//State
}//Nay