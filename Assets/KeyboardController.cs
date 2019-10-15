using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    public Transform[] keys;
    public Dictionary<Transform, Vector3> keyStartingLocalPositions;
    public Dictionary<KeyCode, Transform> keyKeyCodes;
    private KeyCode[] allKeyCodes;
    public Vector3 keyOffset = new Vector3(0f, 0f, 0.09f);
    public TerrainUpdater terrainUpdater;

    void Start()
    {
        allKeyCodes = System.Enum.GetValues(typeof(KeyCode)) as KeyCode[];
        keyKeyCodes = new Dictionary<KeyCode, Transform>();
        keyStartingLocalPositions = new Dictionary<Transform, Vector3>();

        for (int i = 0; i < keys.Length; i++)
        {
            keyStartingLocalPositions.Add(keys[i], keys[i].localPosition);
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
                    tmpKey.localPosition = keyStartingLocalPositions[tmpKey] - keyOffset;
                    keyHit = true;
                }
            }
            if (Input.GetKeyUp(allKeyCodes[i]))
            {
                if (keyKeyCodes.TryGetValue(allKeyCodes[i], out tmpKey))
                {
                    tmpKey.localPosition = keyStartingLocalPositions[tmpKey];
                    keyHit = true;
                }
            }
        }

        if (keyHit){ terrainUpdater.UpdateHeightMap(); }
    }

    KeyCode FetchKey()
    {
        var e = System.Enum.GetNames(typeof(KeyCode)).Length;
        for (int i = 0; i < e; i++)
        {
            if (Input.GetKey((KeyCode)i))
            {
                return (KeyCode)i;
            }
        }

        return KeyCode.None;
    }
}