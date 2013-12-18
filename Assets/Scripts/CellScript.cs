using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// This script is attached to every game piece object
// Keeps track of collisions as they enter/stay/exit


public class CellScript : MonoBehaviour {
	public List<GameObject> CollidingWith;
	public List<GameObject> CurrentMatches;
	public Transform CollectParticlesPrefab;
	
	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
	
	}
	// callback functions - invoked by unity from the physics engine
	void OnCollisionStay2D(Collision2D collision) { 
		if (!CollidingWith.Contains(collision.gameObject ) ) {
			CollidingWith.Add(collision.gameObject);
		}
	}
	// same
	void OnCollisionExit2D(Collision2D collision) { 
		CollidingWith.Remove(collision.gameObject);
	}
	// here is the function we call from Spawnscript	
	// scan the array created by the above events
	// return a match number
	
	public int DetectMatches(GameObject other) { 
		int matches = 0;	
		if (!other) {
			Debug.Log("missing gameobject (other)");
			return 0;
		}
		foreach (GameObject collided in CollidingWith) { 
			if (!collided) {
				Debug.Log("missing gameobject (collided)");
				continue;
			}
			if (other.GetInstanceID() == collided.GetInstanceID()) {
				Debug.Log("Avoid nasty hard loop");
				continue;
			}
			if (other.tag == collided.tag) { 
				matches++;
				CurrentMatches.Add(collided);
				//CellScript cScript = collided.GetComponent<CellScript>();
				//matches += cScript.DetectMatches(collided);
				//Debug.Log("Matched: " + matches);
			}
		}
		return matches;
	}
	public void DestroyMatches(GameObject other) { 
		foreach (GameObject death in CurrentMatches) { 
			if (CollectParticlesPrefab) {
				Instantiate(CollectParticlesPrefab, death.transform.position, Quaternion.identity);
			}
			CollidingWith.Remove(death);
			DestroyObject(death);
		}
		if (CollectParticlesPrefab) {	
			Instantiate(CollectParticlesPrefab, other.transform.position, Quaternion.identity);
		}
		CollidingWith.Remove(other);
		DestroyObject(other);
	}
}
