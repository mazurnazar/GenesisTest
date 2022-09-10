using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Scrolling : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Manager manager;
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }
	public void OnPointerDown(PointerEventData eventData)
	{
        manager.CanScroll = false;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
        StartCoroutine(WaitForScroll());
        
	}
    IEnumerator WaitForScroll()
    {
        yield return new WaitForSeconds(1f);
        manager.CanScroll = true;

    }
	

}
