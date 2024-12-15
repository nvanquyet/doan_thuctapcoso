using ShootingGame;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUI : Frame
{
    [Header("Input Field")]
    [SerializeField] private InputText usernameInput;
    [SerializeField] private InputText passwordInput;
    [SerializeField] private InputText confirmPassInput;

    [Space(10)]
    [Header("Button")]
    [SerializeField] private Button mainBtn;
    [SerializeField] private Button secondBtn;
    [SerializeField] private Button btnForgotPassword;

    [Space(10)]
    [Header("Text")]
    [SerializeField] private Text titleText;
    [SerializeField] private Text mainBtnText;
    [SerializeField] private Text secondBtnText;
    [SerializeField] private GameObject alreadyHaveAccountText;
    [SerializeField] private SceneLoader sceneLoader;

    private void Start()
    {
        if (UserData.IsLogin)
        {
            OnLogin();
        }
        else
        {
            InitLogin();
            btnForgotPassword.onClick.AddListener(InitResetPassword);
        }        
    }

    private void InitResetPassword()
    {
        passwordInput.gameObject.SetActive(false);
        mainBtn.onClick.RemoveAllListeners();
        secondBtn.onClick.RemoveAllListeners();

        titleText.text = "Forgot Password";
        mainBtnText.text = "Reset Password";
        secondBtnText.text = "Login";
        mainBtn.onClick.AddListener(OnForgotPassword);
        secondBtn.onClick.AddListener(() => InitLogin(true));

        btnForgotPassword.gameObject.SetActive(false);
        alreadyHaveAccountText.gameObject.SetActive(true);
    }

    

    private void InitLogin(bool login = true)
    {
        mainBtn.onClick.RemoveAllListeners();
        secondBtn.onClick.RemoveAllListeners();
        if (login)
        {
            mainBtn.onClick.AddListener(OnLogin);
            secondBtn.onClick.AddListener(() => InitLogin(false));
            SetTextButton("Login", "Register");
        }
        else
        {
            mainBtn.onClick.AddListener(OnRegister);
            secondBtn.onClick.AddListener(() => InitLogin(true));
            SetTextButton("Register", "Login");
        }

        titleText.text = login ? "Login" : "Register";

        alreadyHaveAccountText.gameObject.SetActive(!login);
        confirmPassInput.gameObject.SetActive(!login);
        btnForgotPassword.gameObject.SetActive(login);
    }


    private void SetTextButton(string mainBtnText, string secondBtnText)
    {
        this.mainBtnText.text = mainBtnText;
        this.secondBtnText.text = secondBtnText;
    }

    private void OnRegister()
    {
        GameService.LogColor("Register");
    }

    private void OnLogin()
    {
        UserData.IsLogin = true;
        Hide(false, () =>
        {
            sceneLoader.LoadScene(1);
        });
    }

    private void OnForgotPassword()
    {
        //Send email to user to reset password
        GameService.LogColor("Forgot Password");
    }
}
