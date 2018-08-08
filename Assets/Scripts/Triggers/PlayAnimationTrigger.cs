using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationTrigger : MonoBehaviour, ITrigger {

    public string animationName;

    public void OnEventFinished()
    {
        throw new System.NotImplementedException();
    }

    public void StartTrigger()
    {
        Manager.GetPlayer.PlayAnimation(animationName);
        print("PLAYED ANIM");
    }
}
