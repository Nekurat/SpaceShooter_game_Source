using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Text _GameOver;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _highScoreText;
    private int _lastScore;
    [SerializeField] private Text _newHighScore;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
        _lastScore = playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];
    }

    public void GameOver()
    {
        _GameOver.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        ShowHighScore(_lastScore);
        StartCoroutine(GameOverFlickerRoutine());
    }

    public void ShowHighScore(int playerScore)
    {
        int highScore = PlayerPrefs.GetInt("High Score:", 0);
        bool isNew = false;
        if (playerScore > highScore)
        {
            highScore = playerScore;
            PlayerPrefs.SetInt("High Score:", highScore);
            isNew = true;
        }
        _highScoreText.text = "High Score: " + highScore;
        _highScoreText.gameObject.SetActive(true);

        if (_newHighScore != null)
            _newHighScore.gameObject.SetActive(isNew);
    }

    private IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _GameOver.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _GameOver.enabled = false;
            yield return new WaitForSeconds(0.5f);

            if (_newHighScore != null && _newHighScore.gameObject.activeSelf)
            {
                _newHighScore.enabled = true;
                yield return new WaitForSeconds(0.1f);
                _newHighScore.enabled = false;
                yield return new WaitForSeconds(0.1f);
            }

        }
    }

    void Update()
    {
        if (_restartText.gameObject.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
