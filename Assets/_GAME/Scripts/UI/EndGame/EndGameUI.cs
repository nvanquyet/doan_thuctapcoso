using ShootingGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : Frame
{
    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("UI Elements - Win")]
    public Image[] stars;

    [Header("UI Elements - Button")]
    public Button btnHome;
    public Button btnReplay;
    [Header("UI Elements - Text")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI textReward;
    public TextMeshProUGUI textCoinReward;

    [Header("UI Elements - Reward")]
    public GameObject rewardPanel;

    [Header("UI Elements - Color")]
    public Color colorWinTitleText;
    public Color colorLoseTitleText;


    private int coinClaimed;

    private void Start()
    {
        this.AddListener<GameEvent.OnEndGame>(OnEndGame, false);
        btnHome.onClick.AddListener(OnHomeBtnClicked);
        btnReplay.onClick.AddListener(OnReplayBtnClicked);
        gameObject.SetActive(false);
    }

    private void OnEndGame(GameEvent.OnEndGame param)
    {
        ShowEndgame(param);
        UICtrl.Instance.Show<EndGameUI>();
    }

    public void ShowEndgame(GameEvent.OnEndGame param)
    {
        int score = GameService.CalculateScore(param.enemiesDefeated, param.timeLeft);
        int maxScore = GameService.CalculateScore(param.totalEnemies, param.timeLeft);
        int stars = param.isWin ? GameService.CalculateStars(score, maxScore) : 0;
        if (score > UserData.BestScore) UserData.BestScore = score;
        coinClaimed = param.enemiesDefeated;

        titleText.text = param.isWin ? "VICTORY !!!" : "DEFEAT ...";
        titleText.color = param.isWin ? colorWinTitleText : colorLoseTitleText;
        scoreText.text = score.ToString();
        bestScoreText.text = $"Best Score: {UserData.BestScore}";
        if (coinClaimed > 0)
        {
            textCoinReward.text = coinClaimed.ToString();
            textCoinReward.transform.parent.gameObject.SetActive(true);
        }
        else textCoinReward.transform.parent.gameObject.SetActive(false);
        textReward.text ="No Rewards.... :(";

        winPanel.SetActive(param.isWin);
        losePanel.SetActive(!param.isWin);

        if (param.isWin)
        {
            OnShowStarAnimation(stars);
        }
    }

    private void OnShowStarAnimation(int starClaim)
    {
        for (int i = 0; i < this.stars.Length; i++)
        {
            this.stars[i].gameObject.SetActive(i < starClaim);
        }
    }

    private void OnClaimReward()
    {
        if (coinClaimed > 0)
        {
            UserData.CurrentCoin += coinClaimed;
        }
    }

    private void OnReplayBtnClicked()
    {
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        OnClaimReward();
        UIPopUpCtrl.Instance.Get<LoadScene>().LoadSceneAsync((int)SceneIndex.InGame);
    }

    private void OnHomeBtnClicked()
    {
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        OnClaimReward();
        UIPopUpCtrl.Instance.Get<LoadScene>().LoadSceneAsync((int)SceneIndex.Home);
    }
}
