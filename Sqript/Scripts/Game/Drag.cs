using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public static Drag hurufSedangDrag;
    [SerializeField] TMPro.TextMeshProUGUI hurufDisplay;
    private bool petunjuk, terisi;
    private Vector3 posisiAwal;
    private Transform parentAwal;

    public string Huruf {get; private set;}

    public void Inisialisasi(Transform parent, string huruf, bool petunjuk){
		Huruf = huruf;
		transform.SetParent(parent);
		hurufDisplay.SetText(Huruf);
		this.petunjuk = petunjuk;
		GetComponent<CanvasGroup>().alpha = petunjuk ? .5f : 1f; 
		hurufDisplay.color = petunjuk ? new Color(1.0f, 1.0f, 1.0f, 0.0f) : new Color(0.0f, 0.7f, 1.0f, 1.0f); 
    }
    
    public void Cocok(Transform parent){
    	transform.SetParent(parent);
    	transform.localPosition = Vector3.zero;
    	petunjuk = true;
    }

    public void OnBeginDrag(PointerEventData eventData){
    	if(petunjuk)
    		return;
    	
    	posisiAwal = transform.position;
    	parentAwal = transform.parent;
    	hurufSedangDrag = this;
    	GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData){
    	if(petunjuk)
    		return;

    	transform.position = Input.mousePosition; 
    }

    public void OnDrop(PointerEventData eventData){
    	if(petunjuk && !terisi){
    		if(hurufSedangDrag.Huruf == Huruf){
    			ManagerKata.Instance.TambahPoin();
    			hurufSedangDrag.Cocok(transform);
    			terisi = true;
    			GetComponent<CanvasGroup>().alpha = 1f; 
    			GetComponent<AudioSource>().Play();
    		}
    	}
    }

    public void OnEndDrag(PointerEventData eventData){
    	if(petunjuk)
    		return;
    	hurufSedangDrag = null;

    	if(transform.parent == parentAwal){
    		transform.position = posisiAwal;
    	}

    	GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
