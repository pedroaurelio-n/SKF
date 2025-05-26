using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ScreenTransitionManager : MonoBehaviour
{
    public static ScreenTransitionManager Instance { get; private set; }

    [Header("Fade")]
    [SerializeField] CanvasGroup fadeCanvas; // Canvas pai
    [SerializeField] CanvasGroup fadeImage;   // Imagem preta
    [SerializeField] GraphicRaycaster graphicRaycaster;
    [SerializeField] float fadeDuration = 1f;

    [Header("UI de Loading")]
    [SerializeField] GameObject loadingUI;
    [SerializeField] Slider loadingSlider;

    [Header("Frase Desafiadora")]
    [SerializeField] TextMeshProUGUI loadingMessage;
    [SerializeField] float wordDelay = 0.4f;
    [SerializeField] string frase = "A BATALHA VAI COMEÇAR, VOCÊ ESTÁ PREPARADO?";

    bool _isFading;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            fadeCanvas.gameObject.SetActive(false);
            graphicRaycaster.enabled = false;
            return;
        }
        Destroy(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reatribui referências, se existirem na nova cena
        loadingMessage = GameObject.FindWithTag("LoadingText")?.GetComponent<TextMeshProUGUI>();
        loadingSlider = GameObject.FindWithTag("LoadingSlider")?.GetComponent<Slider>();
        loadingUI = GameObject.FindWithTag("LoadingUI");
    }

    public void ChangeScene(string sceneName)
    {
        if (_isFading)
            return;

        _isFading = true;
        StartCoroutine(StartSceneTransition(sceneName));
    }

    IEnumerator StartSceneTransition(string sceneName)
    {
        // Ativa canvas de transição
        fadeCanvas.gameObject.SetActive(true);
        fadeCanvas.alpha = 0f;
        graphicRaycaster.enabled = true;

        // Fade-in
        yield return fadeCanvas.DOFade(1f, fadeDuration).WaitForCompletion();

        // Mostra loading
        if (loadingUI != null)
            loadingUI.SetActive(true);

        if (loadingMessage != null)
            loadingMessage.text = "";

        // Frase aparecendo palavra por palavra
        if (loadingMessage != null)
        {
            string[] words = frase.Split(' ');
            foreach (string word in words)
            {
                loadingMessage.text += word + " ";
                loadingMessage.DOFade(0f, 0f);
                loadingMessage.DOFade(1f, 0.3f);
                yield return new WaitForSeconds(wordDelay);
            }
        }

        // Inicia carregamento da cena
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            if (loadingSlider != null)
                loadingSlider.value = progress;

            yield return null;
        }

        yield return new WaitForSeconds(1f); // pausa para impacto

        // Fade-out
        yield return fadeCanvas.DOFade(0f, fadeDuration).WaitForCompletion();

        graphicRaycaster.enabled = false;
        fadeCanvas.gameObject.SetActive(false);
        _isFading = false;
        op.allowSceneActivation = true;
    }
}
