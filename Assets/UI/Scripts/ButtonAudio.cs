using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
  [SerializeField] private string hoverSoundName = "UI_Hover";
  [SerializeField] private string clickSoundName = "UI_Click";

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (!string.IsNullOrEmpty(hoverSoundName))
      GameEvents.RequestPlaySound(hoverSoundName);
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (!string.IsNullOrEmpty(clickSoundName))
      GameEvents.RequestPlaySound(clickSoundName);
  }
}
