using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    public ScoreArea[] ScoreAreas;

    public Dictionary<int, int> GetPlayerInformation() {
        Dictionary<int, int> playerInformation = new Dictionary<int, int>(); 

        // Create dict with [playerId -> playerScore]
        for (int i = 0; i < ScoreAreas.Length; i++) {
            Debug.Log($"Creating id -> {i} with score {ScoreAreas[i].CurrentScore}");
            playerInformation[i] = ScoreAreas[i].CurrentScore;
        }

        return playerInformation;
    }

    public bool DoesWinnerExist() {
        Dictionary<int, int> playerInformation = GetPlayerInformation();
        
        Debug.Log($"Distinct count {playerInformation.Values.Distinct().Count()}");

        return playerInformation.Values.Distinct().Count() == playerInformation.Count();
    }
}
