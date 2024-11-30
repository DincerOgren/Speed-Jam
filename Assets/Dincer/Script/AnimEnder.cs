using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEnder : MonoBehaviour
{
    public ChargeAttack chg;


    public void ResetRelease()
    {
        chg.ResetRelease();
    }

    public void EndFirstCharge()
    {
        chg.EndFirstCharge();
    }

    
}
