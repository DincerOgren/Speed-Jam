using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractWithObject : MonoBehaviour
{
    private Slider slider;
    private GameObject sliderObject;
    public int increaseAmount = 15;
    public int currentValue = 0;
    private GameObject interactingObject;

    

    public bool done;

    private bool interacting;

    private GameObject player;

    public bool interactable;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        slider = GameObject.FindGameObjectWithTag("PressEUi").GetComponent<Slider>();
        sliderObject = GameObject.FindGameObjectWithTag("PressEUi");
    }

    private void Start()
    {
        interactingObject = gameObject;
        
        sliderObject.SetActive(false);
    }
    private void Update()
    {
        Vector3 diff = interactingObject.transform.position - player.transform.position;
        
        if(diff.magnitude < 10)
        {
            interactable = true;
            sliderObject.SetActive(true);
        }
        else
        {
            interactable = false;
            sliderObject.SetActive(false);
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
                sliderObject.SetActive(false);
                slider.value = 0;
                Destroy(gameObject);
                
                StopAllCoroutines();
                
                
            }

            yield return new WaitForSecondsRealtime(1);
        }

        currentValue = 0;
        slider.value = 0;
        sliderObject.SetActive(false);
        print(currentValue);
        interacting = false;
        StopAllCoroutines();

    }
}
