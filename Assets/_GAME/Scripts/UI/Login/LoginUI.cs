using ShootingGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : Frame
{
    [Header("Input Field")] 
    [SerializeField] private InputText emailInput;
    [SerializeField] private InputText usernameInput;
    [SerializeField] private InputText passwordInput;
    [SerializeField] private InputText confirmPassInput;

    [Space(10)] [Header("Button")] [SerializeField]
    private Button mainBtn;

    [SerializeField] private Button secondBtn;
    [SerializeField] private Button btnForgotPassword;

    [Space(10)] [Header("Text")] [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField] private TextMeshProUGUI mainBtnText;
    [SerializeField] private TextMeshProUGUI secondBtnText;
    [SerializeField] private GameObject alreadyHaveAccountText;


    private void Awake()
    {
        Session.gI().Connect(GameConfig.Instance.ServerAddress, GameConfig.Instance.ServerPort);
        Session.gI().SetHandler(Controller.gI());
    }

    private void Start()
    {
        this.AddListener<GameEvent.OnLogin>(OnLoginCallBack, false);
        this.AddListener<GameEvent.OnResgister>(OnRegisterCallBack, false);
        if (UserData.IsLogin) ForceLogin();
        else
        {
            InitLogin();
            btnForgotPassword.onClick.AddListener(InitResetPassword);
        }
    }

    private void OnLoginCallBack(GameEvent.OnLogin login)
    {
        GameService.LogColor($"Login : {login.success}");
        UserData.IsLogin = login.success;
        if (!login.success)
        {
            if (!string.IsNullOrEmpty(login.message)) UIPopUpCtrl.Instance.Get<UINotice>().SetNotice("Login Error", login.message);
        }
        else
        {
            //Load UserData
            if(!string.IsNullOrEmpty(emailInput.Text) && !string.IsNullOrEmpty(passwordInput.Text)) UserData.EmailPassword = (emailInput.Text, passwordInput.Text);
            Hide(false, () => { UIPopUpCtrl.Instance.Get<LoadScene>().LoadSceneAsync((int)SceneIndex.Home); });
        }
    }

    private void ForceLogin()
    {
        var (email, password) = UserData.EmailPassword;
        GameService.LogColor($"Login {email} {password}");
        Service.gI().login(email, password, "0.0.1");
    }

    /// <summary>
    /// Connect to server and get client info
    /// </summary>
    private void LoadClientInfo()
    {
        //var data = Service.gI().GetClientInfo();
        //How to load client info
        //UserData.UserName = data.username;
        //UserData.CurrentEnergy = data.energy;
        //UserData.LastTimePlayed = data.lastTimePlayed;
        //UserData.CurrentCoin = data.CurrentCoin;
        //Save all character buyed
    }

    private void OnRegisterCallBack(GameEvent.OnResgister resgister)
    {
        if (resgister.success)
        {
            UIPopUpCtrl.Instance.Get<UINotice>().SetNotice($"Register Success", "Please login to continue", () => InitLogin(true));
            UserData.UserName = usernameInput.Text;
        }
        else UIPopUpCtrl.Instance.Get<UINotice>().SetNotice("Register Error", resgister.message);
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
        passwordInput.gameObject.SetActive(true);
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

        titleText.text = login ? "LOGIN TO CONTINUE" : "REGISTER NEW ACCOUNT";

        alreadyHaveAccountText.gameObject.SetActive(!login);
        usernameInput.gameObject.SetActive(!login);
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
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        var email = emailInput.Text;
        var password = passwordInput.Text;
        var confirmPass = confirmPassInput.Text;
        var userName = usernameInput.Text;
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(confirmPass))
        {
            UIPopUpCtrl.Instance.Get<UINotice>().SetNotice($"Register Error", "Email or Password is empty");
            return;
        }
        // else if (!GameService.IsValidEmail(email))
        // {
        //     UIPopUpCtrl.Instance.Get<UINotice>().SetNotice($"Register Error", "Email is invalid");
        //     return;
        // }
        else if (password.Length < 6)
        {
            UIPopUpCtrl.Instance.Get<UINotice>().SetNotice($"Register Error", "Password must be at least 6 characters");
            return;
        }
        else if (mainBtnText.text == "Register")
        {
            if (string.IsNullOrEmpty(confirmPass))
            {
                UIPopUpCtrl.Instance.Get<UINotice>().SetNotice($"Register Error", "Confirm Password is empty");
                return;
            }
            else if (confirmPass != password)
            {
                UIPopUpCtrl.Instance.Get<UINotice>().SetNotice($"Register Error", "Confirm Password is not match");
                return;
            }
        }

        GameService.LogColor("Register");
        Service.gI().register(email, password);
    }

    private void OnLogin()
    {
       // UIPopUpCtrl.Instance.Get<LoadScene>().LoadSceneAsync((int)SceneIndex.Home);
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        var email = emailInput.Text;
        var password = passwordInput.Text;
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            UIPopUpCtrl.Instance.Get<UINotice>().SetNotice($"Login Error", "Email or Password is empty");
            return;
        }
        // else if (!GameService.IsValidEmail(email))
        // {
        //     UIPopUpCtrl.Instance.Get<UINotice>().SetNotice($"Login Error", "Email is invalid");
        //     return;
        // }
        else if (password.Length < 6)
        {
            UIPopUpCtrl.Instance.Get<UINotice>().SetNotice($"Login Error", "Password must be at least 6 characters");
            return;
        }

        GameService.LogColor($"Login {email} {password}");
        Service.gI().login(email, password, "0.0.1");
    }

    private void OnForgotPassword()
    {
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        GameService.LogColor("Forgot Password");
        //Send email to user to reset password
    }
}