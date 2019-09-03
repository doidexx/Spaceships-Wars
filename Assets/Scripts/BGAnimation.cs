using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAnimation : MonoBehaviour
{
    [Range(0f, 1f)][SerializeField] float scrollingSpeed = 0;

    Material mat;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        mat.mainTextureOffset += Vector2.up * scrollingSpeed * Time.deltaTime;
    }
}
