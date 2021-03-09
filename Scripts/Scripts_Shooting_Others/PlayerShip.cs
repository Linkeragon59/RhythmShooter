// Script that defines the behaviour of the player ship in function of the user : pressed keys, score in rhythm game, equipped items...
// Is attached to the playerShip object

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : MonoBehaviour {

	public float speed = 5.0f;
	public GameObject explosion; // prefab of the explosion animation for the player

	private Vector3 startPosition;
	private Weapon weapon; // currently used weapon
	private Armor armor; // currently used armor

	private bool invincible = false;
	private Animator animator;

	private bool auto = false;

	void Awake(){
		startPosition = this.transform.position;
		animator = GetComponent<Animator> ();
	}

	public void BeginGame(){
		if (animator != null) {
			Destroy (animator);
		}
		GameValuesContainer.container.shooterHandler.Initialize ();
		GameValuesContainer.container.rhythmHandler.Initialize ();
	}

	// Update is called once per frame
	void Update () { // move the ship in function of the arrows pressed on the keyboard
		if (Input.GetKeyDown("o")) {
			auto = !auto;
		}
		if (!auto) {
			Move (Input.GetAxis ("Vertical"), Input.GetAxis ("Horizontal"));
		}
	}

	// May move automatically if the AI is activated
	public void AutoMove(float vertical, float horizontal){
		if (auto) {
			Move (vertical+Input.GetAxis ("Vertical"), horizontal+Input.GetAxis ("Horizontal"));
		}
	}

	public void Move(float vertical, float horizontal){
		GetComponent<Rigidbody>().velocity = new Vector3(vertical,0.0f,-horizontal)*speed;
	}

	public void Initialize(){
		UpdateItems ();
		if (armor != null) {
			armor.gameObject.SetActive (true);
		}
		if (weapon != null) {
			weapon.gameObject.SetActive (true);
		}
	}

	///////////////////////////
	/// Gestion of Items //////
	///////////////////////////

	// Choose what items to be used in function of damages, combo...
	// In the current state, 1 weapon and 1 armor can be used at a time
	// among the items that can be selected (see CanBeSelected of Items.cs), the items with the best levels are used
	public void UpdateItems(){
		UpdateArmors ();
		UpdateWeapons ();
		int armorLevel = 1;
		int weaponLevel = 1;
		if (armor != null) {
			armorLevel = armor.level+1;
		}
		if (weapon != null) {
			weaponLevel = weapon.level+1;
		}
		float level = (float)Mathf.Max (weaponLevel, armorLevel) * 3f / 5f + (float)Mathf.Min (weaponLevel, armorLevel) * 2f / 5f;
		GameValuesContainer.container.rhythmHandler.spawnSpot.SetDifficulty(Mathf.CeilToInt(level));
		GameValuesContainer.container.shooterHandler.enemySpawner.SetDifficulty(Mathf.CeilToInt(level));
	}

	public void UpdateArmors(){
		int buffCurrentArmor = GameValuesContainer.container.currentArmor;
		Armor newArmor = null;
		int maxLevelArmor;
		if (this.armor == null || this.armor.IsBroken ()) {
			maxLevelArmor = -1;
		}
		else {
			maxLevelArmor = this.armor.level;
		}
		for (int i = 0; i < GameValuesContainer.container.possibleArmors.Length; i++) {
			newArmor = GameValuesContainer.container.possibleArmors [i];
			if (newArmor.CanBeSelected () && newArmor.level > maxLevelArmor) {
				GameValuesContainer.container.currentArmor = i;
				maxLevelArmor = newArmor.level;
			}
		}
		if (GameValuesContainer.container.currentArmor != buffCurrentArmor) {
			SetArmor (GameValuesContainer.container.possibleArmors [GameValuesContainer.container.currentArmor]);
		}
		else if(this.armor != null && this.armor.IsBroken ()) {
			GameValuesContainer.container.currentArmor = -1;
			SetArmor (null);
		}
	}

	public void UpdateWeapons(){
		int buffCurrentWeapon = GameValuesContainer.container.currentWeapon;
		Weapon newWeapon = null;
		int maxLevelWeapon;
		if (this.weapon == null || this.weapon.IsBroken ()) {
			maxLevelWeapon = -1;
		}
		else {
			maxLevelWeapon = this.weapon.level;
		}
		for (int i = 0; i < GameValuesContainer.container.possibleWeapons.Length; i++) {
			newWeapon = GameValuesContainer.container.possibleWeapons [i];
			if (newWeapon.CanBeSelected () && newWeapon.level > maxLevelWeapon) {
				GameValuesContainer.container.currentWeapon = i;
				maxLevelWeapon = newWeapon.level;
			}
		}
		if (GameValuesContainer.container.currentWeapon != buffCurrentWeapon){
			SetWeapon (GameValuesContainer.container.possibleWeapons [GameValuesContainer.container.currentWeapon]);
		}
		else if(this.weapon != null && this.weapon.IsBroken ()) {
			GameValuesContainer.container.currentWeapon = -1;
			SetWeapon (null);
		}
	}

	// Used by UpdateItems to change the armor
	void SetArmor(Armor newArmor)
	{
		if (armor != null) {
			Destroy (armor.gameObject);
		}
		if (newArmor != null) {
			armor = (Armor)Instantiate (newArmor, transform.position, transform.rotation);
			armor.transform.parent = this.transform;
		}
	}

	// Used by UpdateItems to change the weapon
	void SetWeapon(Weapon newWeapon)
	{
		if (weapon != null) {
			Destroy (weapon.gameObject);
		}
		if (newWeapon != null) {
			weapon = (Weapon)Instantiate (newWeapon, transform.position, transform.rotation);
			weapon.transform.parent = this.transform;
		}
		weapon.Initialize ();
	}

	///////////////////////////
	/// Gestion of damages ////
	///////////////////////////

	public void TakeRhythmDamage(int damage){ // When the player miss the rhythm game, the damages are sent to the ship (here)
										//, and the ship send damages to equipped items
		if (weapon != null) {
			weapon.TakeDamage (damage);
		}
		UpdateItems ();
	}

	public void TakeShootingDamage(int damage){

		if (armor == null) {
			Explode ();
		}
		else {
			armor.TakeDamage (damage);
		}
		UpdateItems ();
	}

	public void SetInvisibility(bool invincible){
		this.invincible = invincible;
	}

	// Used by the event handler to respawn the player
	public void ResetPosition(){
		this.transform.position = this.startPosition;
		if (weapon != null) {
			weapon.Initialize ();
		}
	}

	// Destroys itself and display an explosion animation
	public void Explode() {
		if (!invincible) {
			GameObject particle = (GameObject)Instantiate (explosion, transform.position, Quaternion.identity);
			particle.transform.SetParent (GameValuesContainer.container.particlesContainer.transform);
			GameValuesContainer.container.shooterHandler.ResetPlayer ();
		}
	}
}
