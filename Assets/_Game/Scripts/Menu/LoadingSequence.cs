using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class LoadingSequenceTMP : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loadingMessage;
    [SerializeField] float wordDelay = 0.4f;
    [SerializeField] string sceneToLoad;

    string fullMessage = "A BATALHA VAI COMEÇAR, VOCÊ ESTÁ PREPARADO?";

    public void StartLoadingSequence()
    {
        StartCoroutine(ShowWordsOneByOne());
    }

    IEnumerator ShowWordsOneByOne()
    {
        loadingMessage.text = "";
        string[] words = fullMessage.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            string currentText = loadingMessage.text;
            loadingMessage.text = currentText + words[i] + " ";

            // Animação de destaque com DOTween (opcional)
            loadingMessage.DOFade(0f, 0f); // invisível instantâneo
            loadingMessage.DOFade(1f, 0.3f); // fade in suave

            yield return new WaitForSeconds(wordDelay);
        }

        // Pequeno delay antes da troca de cena
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
    }
}
