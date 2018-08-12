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

        yield return new WaitForSeconds(2f);

        transform.DOMove(playerPos, pickupTime);
        transform.DOScale(0.0f, pickupTime);

        yield return new WaitForSeconds(pickupTime * 1.05f);

        Instantiate(Resources.Load<GameObject>("McMuffinGet"), player.transform.position, Quaternion.identity, player.transform);

        Manager.GetPlayer.disableInput = false;
        if (asset)
        {
            Manager.GetDialogue.StartDialogue(0, asset);
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
