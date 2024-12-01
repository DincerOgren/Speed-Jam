using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractWithObject : MonoBehaviour
{
    public Slider slider;
    public int increaseAmount = 15;
    public int currentValue = 0;
    private GameObject interactingObject;

    public bool done;

    private bool interacting;

    private GameObject player;

    public bool interactable;

    private void Start()
    {
        interactingObject = gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        Vector3 diff = interactingObject.transform.position - player.transform.position;
        
        if(diff.magnitude < 10)
        {
            interactable = true;
        }
        else
        {
            interactable = false;
        }

        if (slider.value <= currentValue)
        {
            slider.value += 50 * Time.deltaTime;
        }


        if(Input.GetKeyDown(KeyCode.F) && interactable && !interacting)
        {
            interacting = true;
            Interact();
        }
    }

    private void Interact()
    {
        StartCoroutine(Focus());
    }

    IEnumerator Focus()
    {
        while(interactable)
        {
            currentValue += 20;

            if (currentValue >= 100)
            {
                print(currentValue);
                done = true;
                yield return new WaitForSecondsRealtime(0.5f);
                StopAllCoroutines();
                Destroy(gameObject);
            }

            yield return new WaitForSecondsRealtime(1);
        }

        currentValue = 0;
        slider.value = 0;
        print(currentValue);
        interacting = false;
        StopAllCoroutines();

    }
}
