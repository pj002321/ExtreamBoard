using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireBaseAuthUIController : MonoBehaviour
{
    public InputField emailInputField;
    public InputField passwordInputField;
    public TextMeshProUGUI outputText;
    public GameObject mainCanvas;
    bool issigned = false;

    // Start is called before the first frame update
    void Start()
    {
        outputText.text = "";
        FireBaseAuthManager.Instance.OnChangedLoginState += OnChangedLoginState;
        FireBaseAuthManager.Instance.InitializeFirebase();
        
        mainCanvas.SetActive(false);   
    }
 
    public void CreateUser()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;  
        FireBaseAuthManager.Instance.Create(email, password);  
    }

    public void SigedIn()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;
        FireBaseAuthManager.Instance.Login(email, password);
       
    }
    private void Update()
    {
        if (FireBaseAuthManager.Instance.GetSignedState())
        {
            SetActiveEvents();
        }
    }
    void SetActiveEvents()
    {
        mainCanvas.SetActive (true);
    }
    public void SigedOut()
    {
        FireBaseAuthManager.Instance.LogOut();
    }

    private void OnChangedLoginState(bool signedIn)
    {
      
        if (signedIn)
        {
            outputText.text = "성공적으로 로그인 되었습니다";
        }
        else
        {
            outputText.text = "이메일/비밀번호가 일치하지 않습니다.";
        }
    }
}
