using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceByValue : MonoBehaviour
{
    public static ReferenceByValue instance;

    [Space]
    [Header("Project References")]
    public AudioClip[] possibleClip;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Return Material At given Value
    /// </summary>
    /// <param name="i_value"><given value/param>
    /// <returns></returns>
    public T ReturnValue<T>(T[] possibleValue, float i_percentValue)
    {
        float localIndex = Remap(i_percentValue, 0f, 1f, 0f, (float)possibleClip.Length);
        
        Debug.Log(Mathf.RoundToInt(localIndex));

        return possibleValue[Mathf.RoundToInt(localIndex)];
    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
