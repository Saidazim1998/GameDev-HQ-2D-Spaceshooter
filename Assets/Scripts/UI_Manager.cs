using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private TextMeshProUGUI _gameOverText;
    [SerializeField]
    private TextMeshProUGUI _restartText;
    [SerializeField]
    private Slider _speedChargeSlider;

    [SerializeField] private float _speedChargeValue = 3f;

    private GameManager _gameManager; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FillingSlider();
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager==null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        if(currentLives<0 || currentLives >=_livesSprites.Length)
        {
            return;
        }
        _livesImg.sprite = _livesSprites[currentLives];

        if (currentLives==0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverTextFlickerRoutine());
    }

    IEnumerator GameOverTextFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void FillingSlider()
    {
        StopCoroutine(EmptyingSliderCoroutine());

        StartCoroutine(FillingSliderCoroutine());
    }

    IEnumerator FillingSliderCoroutine()
    {
        while (_speedChargeSlider.normalizedValue <1)
        {
            _speedChargeSlider.normalizedValue +=_speedChargeValue;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void EmptyingSlider()
    {
        StopCoroutine(FillingSliderCoroutine());

        StartCoroutine(EmptyingSliderCoroutine());
    }

    IEnumerator EmptyingSliderCoroutine()
    {
        float emptyValue = _speedChargeValue / 4;
        while (_speedChargeSlider.normalizedValue>0)
        {
            _speedChargeSlider.normalizedValue -= 0.025f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    
}
