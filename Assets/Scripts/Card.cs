using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int row;
    [SerializeField] private int col;
    [SerializeField] private Vector3 position;
    [SerializeField] private bool isActive = true;
    public bool IsActive { get => isActive; set { isActive = value; } }
    public Vector3 Position { get => position;private set { } }
    private Image image;
    private Manager manager;
    [SerializeField] ParticleSystem particles;
    [SerializeField] Material material;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        particles = GameObject.Find("Particles").GetComponent<ParticleSystem>();
    }
    public void Initialize(int Row,int Col,Vector3 Pos, bool Active)
    {
        row = Row;
        col = Col;
        position = Pos;
        isActive = Active;
    }

	// Update is called once per frame
	void Update()
    {
        
    }

    // when click the card
	public void OnPointerClick(PointerEventData eventData)
	{
        PlayParticles();
        StartCoroutine(MoveCardsUp());
        isActive = false;
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }
    // moviing cards under this to up
    IEnumerator MoveCardsUp()
    {
        int posToMove = 265;
        int curPos = 0;
        yield return new WaitForSeconds(1f);
        while (curPos < posToMove)
        {
            for (int i = row + 1; i < manager.HeightNumber; i++)
            {
                manager.Cards[col, i].transform.localPosition += new Vector3(0, 5f, 0);
            }
            curPos += 5;
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
        SetNewPosition();
        
    }

    void SetNewPosition()
    {
        for (int i = row + 1; i < manager.HeightNumber; i++)
        {
            manager.Cards[col, i].GetComponent<Card>().position = manager.Cards[col, i].transform.localPosition;
        }
    }
    // play particles of destroying a card
    void PlayParticles()
    {
        material.SetTexture("_MainTex", manager.Sprites[row].texture);
        particles.transform.position = transform.position;
        particles.Play();
    }

}
