using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationTrigger : MonoBehaviour, ITrigger {

    public Animator target;

    [Header("Animation")]
    public string animationName;

    public bool setParameter = false;

    [Header("Parameter")]
    public string parameter;
    public bool parameterValue;

    public void OnEventFinished()
    {
    }

    public void StartTrigger()
    {
        if (!target)
        {
            Manager.GetPlayer.PlayAnimation(animationName);

            if (setParameter)
            {
                Manager.GetPlayer.SetAnimatorBoolParameter(parameter, parameterValue);
            }
        }

        else
        {
            target.Play(animationName);
        }
    }
}
