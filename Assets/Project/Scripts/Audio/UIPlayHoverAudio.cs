using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayHoverAudio : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance?.PlayButtonHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SoundManager.Instance?.StopButtonHover();
    }
}