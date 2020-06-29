using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    private bool startPlayAnimation;
    private bool startCreditsAnimation;
    private bool unHinged;
    private bool buttonInCenter;

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject creditsButton;
    [SerializeField] private GameObject instructionsButton;
    [SerializeField] private GameObject exitButton;

    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject instructionsPanel;

    private float animationMoveSpeed;
    private float animationTimer;

    // Start is called before the first frame update
    void Start()
    {
        animationMoveSpeed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Potential improvement, have the buttons like drop down a little bit (unhinge) then accelerate down off the screen
        if(startPlayAnimation)
        {
            PlayButtonAnimation();
        }

        if(startCreditsAnimation)
        {
            CreditsAnimation();
        }
    }

    public void Play()
    {
        startPlayAnimation = true;
        instructionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void Credits()
    {
        //startCreditsAnimation = true;
        creditsPanel.SetActive(!creditsPanel.activeSelf);
        instructionsPanel.SetActive(false);
    }

    public void Instructions()
    {
        instructionsPanel.SetActive(!instructionsPanel.activeSelf);
        creditsPanel.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void PlayButtonAnimation()
    {
        if (unHinged == false)
        {
            if (animationTimer <= 0.1f)
            {
                playButton.transform.Translate(0, 100.0f * Time.deltaTime, 0);
                exitButton.transform.Translate(0, 100.0f * Time.deltaTime, 0);
                creditsButton.transform.Translate(0, 100.0f * Time.deltaTime, 0);
                instructionsButton.transform.Translate(0, 100.0f * Time.deltaTime, 0);
            }
            else if (animationTimer <= 0.2)
            {
                playButton.transform.Translate(0, -500.0f * Time.deltaTime, 0);
                exitButton.transform.Translate(0, -500.0f * Time.deltaTime, 0);
                creditsButton.transform.Translate(0, -500.0f * Time.deltaTime, 0);
                instructionsButton.transform.Translate(0, -500.0f * Time.deltaTime, 0);
            }
            else if (animationTimer >= 0.3)
            {
                unHinged = true;
            }
            animationTimer += Time.deltaTime;
        }
        else
        {
            playButton.transform.Translate(0, animationMoveSpeed * Time.deltaTime, 0);
            instructionsButton.transform.Translate(0, (animationMoveSpeed * 1.1f) * Time.deltaTime, 0);
            creditsButton.transform.Translate(0, (animationMoveSpeed * 1.2f) * Time.deltaTime, 0);
            exitButton.transform.Translate(0, (animationMoveSpeed * 1.3f) * Time.deltaTime, 0);
            if (animationMoveSpeed >= -4000)
            {
                animationMoveSpeed -= 50.0f;
            }

            // Add fade to black here
            GameObject.Find("Canvas").GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
        }

        if (Camera.main.ScreenToWorldPoint(playButton.transform.position).y <= -10)
        {
            SceneManager.LoadScene("Level");
        }
    }

    private float panelHeight = 1400;
    private void CreditsAnimation()
    {
        if (Camera.main.ScreenToWorldPoint(creditsButton.transform.position).x < 0)
        {
            creditsButton.transform.Translate(1000 * Time.deltaTime, 0, 0);
            //creditsPanel.GetComponent<RectTransform>().offsetMin = new Vector2(creditsPanel.GetComponent<RectTransform>().offsetMin.x, 0);
        }
        else
        {
            creditsButton.transform.position = new Vector3(1920, creditsButton.transform.position.y, creditsButton.transform.position.z);
            buttonInCenter = true;
        }

        if(buttonInCenter)
        {
            creditsPanel.GetComponent<RectTransform>().offsetMin = new Vector2(creditsPanel.GetComponent<RectTransform>().offsetMin.x, panelHeight);
            if(panelHeight > 100)
            {
                panelHeight-=10f;
            }
        }
    }
}
