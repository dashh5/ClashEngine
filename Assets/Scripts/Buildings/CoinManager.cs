using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoinManager : MonoBehaviour
{

        private float timeSinceLastCoinAddition = 0f; 
        private const float secondsInADay = 1f; //change to 86400 since (24 hr times 60 minutes times 60 seconds )

        private void Start()
        {
            StartCoroutine(AddCoinsOverTime());
        }


    private IEnumerator AddCoinsOverTime()
   {
       while (true)
       {
           timeSinceLastCoinAddition += Time.deltaTime;


           if (timeSinceLastCoinAddition >= secondsInADay)
           {
               PlayerProperty.coins += 10; // Add 10 coins every day
               timeSinceLastCoinAddition -= secondsInADay;
           }


           yield return null;
       }

    }

}
