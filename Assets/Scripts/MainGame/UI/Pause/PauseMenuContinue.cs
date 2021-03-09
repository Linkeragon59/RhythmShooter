// definition of the MenuChoice abstract class (MenuChoice.cs)
// used for the item of the pauseMenu of the Main game to unpause the game

using UnityEngine;
using System.Collections;

public class PauseMenuContinue : MenuChoice {

	override protected void Effect(){
		GameValuesContainer.container.menuWrapper.TogglePause ();	// unpause the game (the item is not active when the game is unpaused, so it cannot be used to pause)
	}
}
