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
    public T ReturnValue<T>(T[] possibleValue, int i_index)
    {
        int localIndex = i_index > possibleValue.Length - 1 ? possibleValue.Length - 1 : i_index;

        return possibleValue[localIndex];
    }
}
