using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Player;
public class ControllUI : MonoBehaviour
{
    private PlayerController playerController;
    private Coroutine holdCoroutine;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    #region EventMethods
    private IEnumerator HoldButtonCoroutine(int reverse)
    {
        while (true)
        {
            playerController.RotatePlayer(reverse);
            yield return null;
        }
    }

    public void OnPointerDown(int reverse)
    {
        holdCoroutine = StartCoroutine(HoldButtonCoroutine(reverse));
    }
    public void OnPointerUp()
    {
        playerController.RespondToBoost(false);
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
        }
    }
    public void OnLeftButtonDown()
    {
        OnPointerDown(-1);
    }
    public void OnUpButtonDown()
    {
        playerController.RespondToBoost(true);
    }
    public void OnRightButtonDown()
    {
        OnPointerDown(1);
    }
    #endregion EventMethods
}
