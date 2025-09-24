using UnityEngine;

public class Character_Select_Screen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

public int roundsAmout;
public int livesAmount;
public int levelSelect;


public int player1SpriteSelected = 0;
public int player2SpriteSelected = 1;
public int player3SpriteSelected = 2;
public int player4SpriteSelected = 3;
// run code that increases the sprite data for the relevant player

public void NextCharacter(){



}

// run code that decreases the sprite data for the relevant player

public void PreviousCharacter(){




}

public void LockCharacter(){

    
}


// Increases number of lives by 1 
public void NumberOfLivesIncrease(){

livesAmount++;


}

// Decreases number of lives by 1 if lives is <= 0 then lives is set to -30 effectivley disabeling lives
public void NumberOfLivesDecrease(){

livesAmount--;

if (livesAmount <= 0){

    livesAmount = 1;
}

}

// increase time in units of 30 seconds

public void RoundIncrease(){

roundsAmout =+ 1;


}


// Decrease time in units of 30 seconds if <= 0 set to -30 effectivly disabeling time 

                // - (if both lives and time is set to -30 then the game has no win condition, should we allow this?) -
public void RoundDecrease(){

roundsAmout -= 1;

if (roundsAmout <= 0){

    roundsAmout = 1;
}
}


}
