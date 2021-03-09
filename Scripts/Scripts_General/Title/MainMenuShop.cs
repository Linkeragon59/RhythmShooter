// definition of the MenuChoice abstract class (MenuChoice.cs)
// used for the item of the Main Menu of the Title scene to go in the shop to choose weapons and armors by going to the Shop scene

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuShop : MenuChoice {

	override protected void Effect(){
		SceneManager.LoadSceneAsync ("Shop");
	}
}
