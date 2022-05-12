using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool uncovered = true;
    public bool active = true;
    public Sprite frontFace;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = GetTargetRotation();

        var frontObject = transform.Find("Front");
        var spriteRenderer = frontObject.transform.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = frontFace;
    }

    // Update is called once per frame
    void Update()
    {
        var targetRotation = GetTargetRotation();
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5);

        if(active == false)
        {
            Destroy(gameObject);
        }
    }

    Quaternion GetTargetRotation()
    {
        var rotation = uncovered ? Vector3.zero : (Vector3.up * 180f);
        return Quaternion.Euler(rotation);
    }

    private void OnMouseDown()
    {
        var board = FindObjectOfType<Board>();

        if (board.canMove == false)
            return;
        else
            uncovered = !uncovered;

        board.CheckPair();
    }
}
