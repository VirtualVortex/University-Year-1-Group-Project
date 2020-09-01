using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSoundFix : MonoBehaviour {

    public void PlayAboutToAttack()
    {
        GetComponentInParent<EnemyAudioScript>().PlayAboutToAttack();
    }

    public void PlayAttackSound()
    {
        GetComponentInParent<EnemyAudioScript>().PlayAttack();
    }

    public void HitPlayer()
    {
        GetComponentInParent<TestAI>().hitPlayer();
    }
}
