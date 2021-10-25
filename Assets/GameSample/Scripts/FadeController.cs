using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public static FadeController instance = null;

    [SerializeField]
    private UnityEngine.UI.Image fadeOverlay = null;
    [SerializeField]
    private UnityEngine.UI.Image flashOverlay = null;
    private UnityEngine.UI.Image activeOverlay = null;

    private int fadeDirection = 0;
    private float fadeOpacity = 0.0f;
    private float fadeDuration = 0.0f;

    private bool waitForInput = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        activeOverlay = fadeOverlay;
        if (activeOverlay == null)
        {
            this.enabled = false;
            return;
        }

        // Start from black
        fadeOpacity = 1.0f;
        SetOverlayOpacity(fadeOverlay, fadeOpacity);
    }

    void Update()
    {
        // Note: You could simplify this greatly with DOTween, if your project uses it.
        if (fadeDirection != 0)
        {
            fadeOpacity += fadeDirection * (Time.smoothDeltaTime / fadeDuration);
            if (fadeOpacity >= 1.0f && fadeDirection == 1)
            {
                fadeDirection = 0;
            }
            else if (fadeOpacity <= 0.0f && fadeDirection == -1)
            {
                fadeDirection = 0;
            }

            SetOverlayOpacity(activeOverlay, fadeOpacity);
        }
        
        if (waitForInput && Input.GetMouseButtonDown(0))
        {
            GabbyDialogueSample.SampleDialogueSystem.instance().AllowAdvancingDialogue = true;
            GabbyDialogueSample.SampleDialogueSystem.instance().NextLine();
            waitForInput = false;
        }
    }

    public void FadeOut(float fadeTime = 0.75f, bool shouldContinueAfterInput = true)
    {
        StartCoroutine(_FadeOut(fadeTime, shouldContinueAfterInput));
    }

    IEnumerator _FadeOut(float fadeTime, bool shouldContinueAfterInput)
    {
        activeOverlay = fadeOverlay;

        if (shouldContinueAfterInput)
        {
            GabbyDialogueSample.SampleDialogueSystem.instance().SetDialogueUIVisible(false);
            GabbyDialogueSample.SampleDialogueSystem.instance().AllowAdvancingDialogue = false;
        }

        // Fade out
        fadeDirection = 1;
        fadeDuration = fadeTime;

        yield return new WaitForSeconds(fadeTime);

        if (shouldContinueAfterInput)
        {
            waitForInput = true;
        }
    }

    public void FadeIn(float fadeTime = 0.75f, bool shouldContinueAfterInput = true)
    {
        StartCoroutine(_FadeIn(fadeTime, shouldContinueAfterInput));
    }

    IEnumerator _FadeIn(float fadeTime, bool shouldContinueAfterInput)
    {
        activeOverlay = fadeOverlay;

        if (shouldContinueAfterInput)
        {
            GabbyDialogueSample.SampleDialogueSystem.instance().AllowAdvancingDialogue = false;
        }

        // Fade in
        fadeDirection = -1;
        fadeDuration = fadeTime;

        yield return new WaitForSeconds(fadeTime);

        if (shouldContinueAfterInput)
        {
            waitForInput = true;
        }
    }

    public void FadeOutAndIn(float fadeTime = 0.75f, float fadeInDelay = 1.5f, bool shouldContinueAfterInput = true)
    {
        StartCoroutine(_FadeOutAndIn(fadeTime, fadeInDelay, shouldContinueAfterInput));
    }

    IEnumerator _FadeOutAndIn(float fadeTime, float delayTime, bool shouldContinueAfterInput)
    {
        activeOverlay = fadeOverlay;

        if (shouldContinueAfterInput)
        {
            GabbyDialogueSample.SampleDialogueSystem.instance().SetDialogueUIVisible(false);
            GabbyDialogueSample.SampleDialogueSystem.instance().AllowAdvancingDialogue = false;
        }

        // Fade out
        fadeDirection = 1;
        fadeDuration = fadeTime;

        yield return new WaitForSeconds(fadeTime + delayTime);

        // Fade in
        fadeDirection = -1;
        fadeDuration = fadeTime;

        yield return new WaitForSeconds(fadeTime);

        if (shouldContinueAfterInput)
        {
            waitForInput = true;
        }
    }

    public void Flash()
    {
        StartCoroutine(_Flash());
    }

    IEnumerator _Flash()
    {
        activeOverlay = flashOverlay;
        GabbyDialogueSample.SampleDialogueSystem.instance().AllowAdvancingDialogue = false;

        // Show flash
        fadeDirection = 1;
        fadeDuration = 0.05f;

        yield return new WaitForSeconds(0.2f);

        // Slowly hide flash
        fadeDirection = -1;
        fadeDuration = 0.5f;

        yield return new WaitForSeconds(0.5f);

        GabbyDialogueSample.SampleDialogueSystem.instance().AllowAdvancingDialogue = true;
    }

    private void SetOverlayOpacity(UnityEngine.UI.Image overlay, float opacity)
    {
        Color overlayColor = overlay.color;
        overlayColor.a = opacity;
        overlay.color = overlayColor;
    }
}
