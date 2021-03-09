// definition of the MenuChoice abstract class (MenuChoice.cs)
// used for the item of the Main Menu of the Title scene to start playing by going to the Main scene

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuPlay : MenuChoice {

	override protected void Effect(){
		SceneManager.LoadSceneAsync ("StageSelection");
	}
}
