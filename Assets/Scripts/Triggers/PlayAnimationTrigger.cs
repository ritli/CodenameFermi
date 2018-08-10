using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationTrigger : MonoBehaviour, ITrigger {

    [Header("Animation")]
    public string animationName;

    public bool setParameter = false;

    [Header("Parameter")]
    public string parameter;
    public bool parameterValue;

    public void OnEventFinished()
    {
        throw new System.NotImplementedException();
    }

    public void StartTrigger()
    {
        Manager.GetPlayer.PlayAnimation(animationName);

        if (setParameter)
        {
            Manager.GetPlayer.SetAnimatorBoolParameter(parameter, parameterValue);
        }
    }
}
