using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenStarmapTrigger : MonoBehaviour, ITrigger {

    public void OnEventFinished()
    {
        throw new System.NotImplementedException();
    }

    public void StartTrigger()
    {
        Manager.OpenStarmap();
    }

}
