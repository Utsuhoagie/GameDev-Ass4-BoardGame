using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMove : MonoBehaviour
{
    float   minX = -0.75f, 
            maxX = 0.75f, 
            minY = -2.25f, 
            maxY = 2.25f;
          
    Vector2 startPos;
    Vector2 targetPos;
    float lerpFactor = 0.05f;
    bool flag = false;

    float dt;
    
    [SerializeField] float speed;

    int timer = 0;


    Vector2 randomizePos() {
        float randX = Random.Range(minX, maxX);
        float randY = Random.Range(minY, maxY);

        return new Vector2(randX, randY);
    }

    // called once, before 1st frame
    void Start() {
        //targetPos = randomizePos();
        startPos = (Vector2)transform.position;
        timer = timer*1;
    }

    // called once per frame
    void Update() {

        // if ((Vector2)transform.position != targetPos) {
        //     timer++;
        //     Debug.Log($"Frame {timer} ! Delta time = {Time.deltaTime}");

        //     transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        // }
        // else {
        //     Debug.Log($"Position {transform.position} reached! ");
            
        //     targetPos = randomizePos();
        // }


        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);


            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) {
                targetPos = Camera.main.ScreenToWorldPoint(touch.position);
            }

            if (touch.phase == TouchPhase.Ended) {
                targetPos = (Vector2)transform.position;
            }

            Debug.Log($"Touch pos = {touch.position} | Current pos = {(Vector2)transform.position}");
        }

        if ((Vector2)transform.position != targetPos) {
            transform.position = Vector2.Lerp(startPos, targetPos, lerpFactor);
            lerpFactor += 0.05f;
        }
        else {
            startPos = (Vector2)transform.position;
            lerpFactor = 0.05f;
        }
    }

}
