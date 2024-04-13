using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISpriteAnimation : MonoBehaviour
{
    public Image imageComponent;       
    public Sprite[] sprites;             
    public float frameRate = 0.1f;      

    private int currentFrame;           
    private Coroutine animationCoroutine; 

    void Start()
    {
        StartAnimation();
    }

    public void StartAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimateSprite());
    }

    IEnumerator AnimateSprite()
    {
        currentFrame = 0;
        while (true)
        {
            imageComponent.sprite = sprites[currentFrame];
            currentFrame = (currentFrame + 1) % sprites.Length;
            yield return new WaitForSeconds(frameRate); 
        }
    }
}
