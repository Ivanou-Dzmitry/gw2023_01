
using UnityEngine;

public class ControlHeroButton : MonoBehaviour
{
    //GameManager GM;
    public SpriteRenderer spriteRenderer;
    public SoundManager SM;

    [SerializeField] private int ButtonDirection;

    public Sprite regular;
    public Sprite mouseClicked;

    public void OnMouseDown()
    {
        spriteRenderer.sprite = mouseClicked;

        if (!GameManager.GameState)
            SM.ButtonClick();

        if (GameManager.GameState)
        {
            GameManager.HeroDirection = ButtonDirection;
        }
        
    }

    public void OnMouseUp()
    {
        spriteRenderer.sprite = regular;
    }

}
