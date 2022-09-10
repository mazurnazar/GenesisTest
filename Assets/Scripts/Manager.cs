using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Manager : MonoBehaviour
{
    private int widthNumber = 3, heightNumber = 5; // columns and rows
    public int HeightNumber { get => heightNumber; private set { } }
    private GameObject[,] cards; // array of gameobjects cards
    public GameObject[,] Cards { get => cards; private set { } }

    [SerializeField] private GameObject cardsParent;
    [SerializeField] private GameObject cardPrefab;

    private bool[,] ActiveStates;
    private Vector3[,] StartPosition;

    // coords of start and step between cards
    private float xStart = 135, yStart = -135;
    private float xStep = 265, yStep = -265;

    [SerializeField] private Sprite[] sprites;
    public Sprite[] Sprites { get => sprites; private set { } }

    [SerializeField] private ScrollRect scrollRect;

    private bool canScroll;
    public bool CanScroll { get => canScroll;set { canScroll = value; } }

    void Start()
    {
        cards = new GameObject[widthNumber, heightNumber];
        ActiveStates = new bool[3, 5];
        StartPosition = new Vector3[3, 5];
        sprites = GameManager.Instance.SetSpritesFromBundle();
        GameManager.Instance.LoadInfo();
        SetStates();
        SetPositions();        
        CreateCards();
        canScroll = true;
    }
    // taking values of card state from gamemanager
    void SetStates()
    {
        int index = 0;
        for (int i = 0; i < heightNumber; i++)
        {
            for (int j = 0; j < widthNumber; j++)
            {
                ActiveStates[j, i] = GameManager.Instance.states[index];
                index++;
            }
        }
    }
    // taking values of card position from gamemanager
    void SetPositions()
    {
        int index = 0;
        for (int i = 0; i < heightNumber; i++)
        {
            for (int j = 0; j < widthNumber; j++)
            {
                StartPosition[j, i] = GameManager.Instance.positions[index];
                index++;
            }
        }
    }

    // instantiating cards
    void CreateCards()
    {
		for (int i = 0; i < heightNumber; i++)
		{
			for (int j = 0; j < widthNumber; j++)
			{
                cards[j, i] = Instantiate(cardPrefab,new Vector2(0,0),Quaternion.identity);
                cards[j, i].transform.SetParent(cardsParent.transform);

                cards[j, i].transform.localPosition = new Vector3(StartPosition[j,i].x, StartPosition[j,i].y,0);
                cards[j, i].transform.localScale = new Vector3(1, 1, 1);
                
                cards[j, i].transform.GetChild(0).GetComponent<Image>().sprite = sprites[i];
                cards[j, i].gameObject.SetActive(ActiveStates[j,i]);

                cards[j, i].GetComponent<Card>().Initialize(i, j, new Vector3(xStart + j * xStep, yStart + i * yStep, 0), true);
            }
        }
        
    }
    // initial coordinates and state of cards
    public void OnGameLaunch()
    {
        for (int i = 0; i < heightNumber; i++)
        {
            for (int j = 0; j < widthNumber; j++)
            {
                cards[j,i].transform.localPosition = new Vector3(xStart + j * xStep-400, yStart + i * yStep+400,0);
                cards[j, i].GetComponent<Card>().IsActive = true;                
            }
        }
    }

    // writing values of state to game manager for saving into file
    public bool[] ActiveState()
    {
        bool[] states = new bool[15];
       
        int index = 0;
		for (int i = 0; i < heightNumber; i++)
		{
			for (int j = 0; j < widthNumber; j++)
			{
                states[index] = cards[j, i].GetComponent<Card>().IsActive;
                index++;
			}
		}
        return states;
    }
    // writing values of position to game manager for saving into file
    public Vector3[] CurrentPosition()
    {
        Vector3[] positions = new Vector3[15];
        int index = 0;

        for (int i = 0; i < heightNumber; i++)
        {
            for (int j = 0; j < widthNumber; j++)
            {
                positions[index] = cards[j, i].transform.localPosition;
                index++;
            }
        }
        return positions;
    }
    // scrolling second block
	private void Update()
	{
        if (canScroll)
        {
            var pos = new Vector2(Mathf.Sin(Time.time * 1f) * 1000f, 0);
            scrollRect.content.localPosition = pos;
        }
    }
}
