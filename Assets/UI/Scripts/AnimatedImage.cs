using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AnimatedImage : MonoBehaviour
{
  [SerializeField] private Sprite[] frames;
  [SerializeField] private float timePerFrame = 1f;
  private Image image;
  private int currentFrameIndex = 0;
  private float timer = 0f;

  private void Awake()
  {
    image = GetComponent<Image>();
  }

  private void Start()
  {
    image.sprite = frames[0];
  }

  private void Update()
  {
    if (frames == null || frames.Length == 0) return;

    timer += Time.deltaTime;

    if (timer >= timePerFrame)
    {
      timer -= timePerFrame; 
      
      currentFrameIndex = (currentFrameIndex + 1) % frames.Length;
      image.sprite = frames[currentFrameIndex];
    }
  }
}
