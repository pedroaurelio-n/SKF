using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenTransitionManager : MonoBehaviour
{
    public static ScreenTransitionManager Instance { get; private set; }

    [SerializeField] CanvasGroup fadeImage;
    [SerializeField] GraphicRaycaster graphicRaycaster;
    [SerializeField] float fadeDuration;

    bool _isFading;

    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            graphicRaycaster.enabled = false;
            return;
        }
        Destroy(gameObject);
    }

    public void ChangeScene (string sceneName)
    {
        if (_isFading)
            return;
        
        graphicRaycaster.enabled = true;
        _isFading = true;
        fadeImage.DOFade(1f, fadeDuration).OnComplete(() =>
            {
                SceneManager.LoadScene(sceneName);
                fadeImage.DOFade(0f, fadeDuration);
                graphicRaycaster.enabled = false;
                _isFading = false;
            }
        );
    }
}