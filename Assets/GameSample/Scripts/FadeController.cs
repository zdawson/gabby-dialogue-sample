using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public static FadeController instance = null;

    [SerializeField]    
    private UnityEngine.UI.Image overlay = null;

    private int fadeDirection = 0;
    private float fadeOpacity = 0.0f;
    private float fadeDuration = 0.0f;

    private bool tmpInput = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (overlay == null)
        {
            this.enabled = false;
        }
    }

    void Update()
    {
        // Note: You could simplify this greatly with DOTween, if your project uses it.
        if (fadeDirection != 0)
        {
            fadeOpacity += fadeDirection * (Time.smoothDeltaTime / fadeDuration);
            if (fadeOpacity >= 1.0f || fadeOpacity <= 0.0f)
            {
                fadeDirection = 0;
            }

            Color overlayColor = overlay.color;
            overlayColor.a = fadeOpacity;
            overlay.color = overlayColor;
        }
        
        if (tmpInput && Input.GetMouseButtonDown(0))
        {
            GabbyDialogueSample.SampleDialogueSystem.instance().AllowAdvancingDialogue = true;
            GabbyDialogueSample.SampleDialogueSystem.instance().NextLine();
            tmpInput = false;
        }
    }

    public void FadeOut(float fadeTime = 0.75f)
    {
        StartCoroutine(_FadeOut(fadeTime));
    }

    IEnumerator _FadeOut(float fadeTime)
    {
        GabbyDialogueSample.SampleDialogueSystem.instance().SetDialogueUIVisible(false);
        GabbyDialogueSample.SampleDialogueSystem.instance().AllowAdvancingDialogue = false;

        // Fade out
        this.enabled = true;
        fadeDirection = 1;
        fadeDuration = fadeTime;

        yield return new WaitForSeconds(fadeTime);

        tmpInput = true;
    }

    public void FadeIn(float fadeTime = 0.75f)
    {
        StartCoroutine(_FadeIn(fadeTime));
    }

    IEnumerator _FadeIn(float fadeTime)
    {
        GabbyDialogueSample.SampleDialogueSystem.instance().AllowAdvancingDialogue = false;

        // Fade in
        this.enabled = true;
        fadeDirection = -1;
        fadeDuration = fadeTime;

        yield return new WaitForSeconds(fadeTime);

        tmpInput = true;
    }

    public void FadeOutAndIn(float fadeTime = 0.75f, float fadeInDelay = 1.5f)
    {
        this.enabled = true;
        StartCoroutine(_FadeOutAndIn(fadeTime, fadeInDelay));
    }

    IEnumerator _FadeOutAndIn(float fadeTime, float delayTime)
    {
        GabbyDialogueSample.SampleDialogueSystem.instance().SetDialogueUIVisible(false);
        GabbyDialogueSample.SampleDialogueSystem.instance().AllowAdvancingDialogue = false;

        // Fade out
        this.enabled = true;
        fadeDirection = 1;
        fadeDuration = fadeTime;

        yield return new WaitForSeconds(fadeTime + delayTime);

        // Fade in
        this.enabled = true;
        fadeDirection = -1;
        fadeDuration = fadeTime;

        yield return new WaitForSeconds(fadeTime);

        tmpInput = true;
    }
}
