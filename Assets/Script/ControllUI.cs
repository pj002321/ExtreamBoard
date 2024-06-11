using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Player;
public class ControllUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private PlayerController playerController;
    private Coroutine holdCoroutine;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    #region EventMethods
    private IEnumerator HoldButtonCoroutine()
    {
        while (true)
        {
            if (gameObject.name == "LeftButton")
            {
                playerController.RotatePlayer(1);
            }
            else if (gameObject.name == "RightButton")
            {
                playerController.RotatePlayer(-1);
            }
            else if(gameObject.name == "UpButton")
            {
                playerController.RespondToBoost(true);
            }
            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        holdCoroutine = StartCoroutine(HoldButtonCoroutine());
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        playerController.RespondToBoost(false);
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
        }
    }
    #endregion EventMethods
}
