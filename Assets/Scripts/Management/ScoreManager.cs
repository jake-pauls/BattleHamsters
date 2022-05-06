using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    public ScoreArea[] ScoreAreas;

    public List<KeyValuePair<int, int>> GetSortedPlayerInformation() {
        Dictionary<int, int> playerInformation = new Dictionary<int, int>(); 

        // Create dict with [playerId -> playerScore]
        for (int i = 0; i < ScoreAreas.Length; i++) {
            playerInformation[i] = ScoreAreas[i].CurrentScore;
        }

        return playerInformation.OrderByDescending(x => x.Value).ToList();
    }

    public bool DoesWinnerExist() {
        List<KeyValuePair<int, int>> playerInformation = GetSortedPlayerInformation();

        // Check if the top value has the same value as any other score in the list 
        for (int i = 1; i < playerInformation.Count; i++) {
            if (playerInformation[0].Value == playerInformation[i].Value) 
                return false;
        }

        return true;
    }
}
