using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMaterialController : MonoBehaviour
{
    public Transform[] keys;
    public Dictionary<Transform, Vector3> keyStartingLocalPositions;
    public Dictionary<KeyCode, Transform> keyKeyCodes;
    private KeyCode[] allKeyCodes;
    public TerrainUpdater terrainUpdater;
    public Material baseMat;
    public Material keyUpMat;
    public Material keyDownMat;
    public AudioClip keyUpSound;
    public AudioClip keyDownSound;
    [SerializeField] AudioSource audioSource;

    IEnumerator Start()
    {
        allKeyCodes = System.Enum.GetValues(typeof(KeyCode)) as KeyCode[];
        keyKeyCodes = new Dictionary<KeyCode, Transform>();
        keyStartingLocalPositions = new Dictionary<Transform, Vector3>();

        for (int i = 0; i < keys.Length; i++)
        {
            keyStartingLocalPositions.Add(keys[i], keys[i].localPosition);
            keys[i].GetComponent<Renderer>().material = keyUpMat;
        }

        //build a matching array of trimmed names to compare. assume that the key names are Key_<some keycode>
        string[] trimmedKeyNames = new string[keys.Length];
        for (int i = 0; i < trimmedKeyNames.Length; i++)
        {
            var spl = keys[i].name.Split('_');
            if (spl.Length == 2)
            {
                trimmedKeyNames[i] = spl[1];
            }
            else
            {
                Debug.LogError("Malformed key name. Expected Key_<name>. Using entire name " + keys[i].name + "as fallback.");
                trimmedKeyNames[i] = keys[i].name;
            }
        }

        //build the dictionary so we can quickly match keycode to transform in the update
        string[] n = System.Enum.GetNames(typeof(KeyCode));
        int e = n.Length;
        for (int i = 0; i < e; i++)
        {
            for (int k = 0; k < trimmedKeyNames.Length; k++)
            {
                if (trimmedKeyNames[k] == n[i])
                {
                    keyKeyCodes.Add(allKeyCodes[i], keys[k]);
                }
            }
        }

        yield return null;

        terrainUpdater.UpdateHeightMap();
    }

    private Transform tmpKey;
    void Update()
    {
        bool keyHit = false;

        for (int i=0; i < allKeyCodes.Length; i++)
        {
            if (Input.GetKeyDown(allKeyCodes[i]))
            {
                if (keyKeyCodes.TryGetValue(allKeyCodes[i], out tmpKey))
                {
                    tmpKey.GetComponent<MeshRenderer>().material = keyDownMat;
                    audioSource.PlayOneShot(keyDownSound);
                    keyHit = true;
                }
            }
            if (Input.GetKeyUp(allKeyCodes[i]))
            {
                if (keyKeyCodes.TryGetValue(allKeyCodes[i], out tmpKey))
                {
                    tmpKey.GetComponent<MeshRenderer>().material = keyUpMat;
                    audioSource.PlayOneShot(keyUpSound);
                    keyHit = true;
                }
            }
        }

        if (keyHit){ terrainUpdater.UpdateHeightMap(); }
    }
}