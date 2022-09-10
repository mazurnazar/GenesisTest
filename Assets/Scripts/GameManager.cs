using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Text text;
    [SerializeField] private Image circle;

    public bool[] states; // array of bool states of card (is active or not)
    public Vector3[] positions; // array of positions of cards
    private const int cardsNumber = 15; // total number of cards

    private AssetBundle assetBundle;
    private Sprite[] sprites; // array of sprites
    private string path = @"C:\Users\NM\Рабочий стол\AssetBundles\cards";

    private void Awake()
	{
        states = new bool[cardsNumber];
        positions = new Vector3[cardsNumber];
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
	// Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Load());
    }
    // loading assets from assetbundle and displaying it on first scene
    IEnumerator Load()
    {
        float time = Time.time;
        var bundleload = AssetBundle.LoadFromFileAsync(path);
        do
        {
            text.text =  bundleload.progress * 100 + "%";
            yield return null;
        } while (!bundleload.isDone);
        while(Time.time - time<3f)
        {
            string percentage = string.Format("{0:0}% ",(Time.time - time) / 3 * 100f );
            circle.fillAmount = (Time.time - time) / 3;
            text.text = percentage;
            yield return null;
        }
        assetBundle = bundleload.assetBundle;
        SetSpritesFromBundle();
        SceneManager.LoadScene(1);
    }
    // initializing sprites array from assets from assetbundle
    public Sprite[] SetSpritesFromBundle()
    {
        var names = assetBundle.GetAllAssetNames();
        sprites = new Sprite[names.Length];
        
        for (int i = 0; i < names.Length; i++)
        {
            sprites[i] = assetBundle.LoadAsset<Sprite>(names[i]);
        }
        return sprites;
    }

    // saving info to file
    public void SaveInfo(bool [] firstStates, Vector3[] startPos)
    {
        SaveData data = new SaveData();
        for (int i = 0; i < cardsNumber; i++)
        {
            data.activeState[i] = firstStates[i];
            data.startPositions[i] = startPos[i];
        }
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log(Application.persistentDataPath);
    }
   
    // loading info from file
    public void LoadInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            for (int i = 0; i < cardsNumber; i++)
            {
                positions[i] = data.startPositions[i];
                states[i] = data.activeState[i];                
            }
        }
    }
    

    [System.Serializable]
    class SaveData
    {
        public bool[] activeState = new bool[cardsNumber];
        public Vector3[] startPositions = new Vector3[cardsNumber];
    }

}
