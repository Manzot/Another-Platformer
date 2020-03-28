using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : MonoBehaviour
{
    public float loseHeight = -5f;
	PlayerController player;
	private void Awake()
	{
		player = FindObjectOfType<PlayerController>();
	}

	// Update is called once per frame
	void Update ()
    {
	    if (transform.position.y <= loseHeight)
	    {
			PlayerManager.Instance.IsDead();
	    }
	}
}
