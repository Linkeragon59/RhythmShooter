// definition of the MenuChoice abstract class (MenuChoice.cs)

using UnityEngine;
using System.Collections;

public class GameOverContinue : MenuChoice {

	override protected void Effect(){
		GameValuesContainer.container.shooterHandler.Clear ();
		GameValuesContainer.container.rhythmHandler.Clear ();
		GameValuesContainer.container.menuWrapper.ToggleGameOver ();
	}
}
