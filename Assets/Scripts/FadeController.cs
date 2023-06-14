using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Coroutine _fadeCoroutine;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FadeIn(float duration)
    {
        StopFadeCoroutine();
        _fadeCoroutine = StartCoroutine(FadeCoroutine(0f, 1f, duration));
    }

    public void FadeOut(float duration)
    {
        StopFadeCoroutine();
        _fadeCoroutine = StartCoroutine(FadeCoroutine(1f, 0f, duration));
    }

    private void StopFadeCoroutine()
    {
        if (_fadeCoroutine == null)
            return;
        
        StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = null;
    }

    private IEnumerator FadeCoroutine(float startAlpha, float targetAlpha, float duration)
    {
        var objectColor = _spriteRenderer.color;
        var timer = 0f;
        while (timer < duration)
        {
            var alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, alpha);
            _spriteRenderer.color = objectColor;
            timer += Time.deltaTime;
            yield return null;
        }

        _fadeCoroutine = null;
    }

    public IEnumerator WaitForFadeCoroutine()
    {
        while (_fadeCoroutine != null) 
        {
            yield return null;
        }
    }
}
