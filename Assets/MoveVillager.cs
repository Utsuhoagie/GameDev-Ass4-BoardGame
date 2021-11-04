using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVillager : MonoBehaviour
{
    bool isChosen = false;
    Collider2D col;

    // Start is called before the first frame update
    void Start() {
        col = GetComponent<Collider2D>();
    }

    public void HandleInput() {
        if ((Input.touchCount > 0) && (Input.touches[0].phase == TouchPhase.Began)) {
            Touch touch = Input.touches[0];
            Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(touch.position);
            // Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            // RaycastHit2D raycastInfo;

            if (col == Physics2D.OverlapPoint(touchPos)) {
                isChosen = true;
            }
            else {
                isChosen = false;
            }

            Debug.Log($"Is chosen? {isChosen}");
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
