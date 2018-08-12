using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class McGuffinHandler : MonoBehaviour, ITrigger {

    public DialogueAsset asset;

    Animator animator;
    private Player player;
    public float pickupTime;
    private Vector3 playerPos;

    public DialogueTrigger dialogue;

    public void OnEventFinished()
    {
    }

    public void StartTrigger()
    {
        StartCoroutine(PickupRoutine());
    }

    IEnumerator PickupRoutine()
    {
        animator.Play("Pickup");
        Manager.GetPlayer.disableInput = true;

        Manager.GetCamera.TimedLook(2f, transform.position, 3);

        yield return new WaitForSeconds(2f);

        transform.DOMove(playerPos, pickupTime);
        transform.DOScale(0.0f, pickupTime);
        Manager.GetCamera.TimedLook(2f, Manager.GetPlayer.transform.position, 3);

        yield return new WaitForSeconds(pickupTime * 1.05f);

        Instantiate(Resources.Load<GameObject>("McMuffinGet"), player.transform.position, Quaternion.identity, player.transform);

        Manager.GetPlayer.disableInput = false;
        if (dialogue)
        {
            dialogue.StartTrigger();
        }

        gameObject.SetActive(false);
    }

    void Start () {
        animator = GetComponent<Animator>();

        player = Manager.GetPlayer;
    }
	
	void Update () {
        playerPos = player.transform.position;
	}
}
