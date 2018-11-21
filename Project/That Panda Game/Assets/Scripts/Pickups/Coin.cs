using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private float zToDespawn;
    [SerializeField]
    private GameObject _particleSystemPrefab;

    private ParticleSystem _particleSystem;
    private static CoinAction _coinAction;
    private static Spawner _coinSpawner;
    private static GameObject _planet;

    public bool _canPickup;

    private void Awake()
    {
        _coinAction = GameObject.Find("Collectables").transform.Find("Coin").GetComponent<CoinAction>();
        _coinSpawner = GameObject.Find("CoinSpawner").GetComponent<Spawner>();
        _planet = GameObject.Find("Planet");
        _particleSystem = Instantiate(_particleSystemPrefab).GetComponent<ParticleSystem>();
        _particleSystem.transform.SetParent(GameObject.Find("ParticleSystems").transform, true);
    }

    private void Update()
    {
        if (transform.position.z > zToDespawn * -1)
            _coinSpawner.ReturnObject(gameObject);
    }

    public void CanPickup()
    {
        _canPickup = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Planet")
        {
            _canPickup = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.SetParent(_planet.transform, true);
            _coinSpawner.RotateGameObject(gameObject);
            if (transform.position.y > 0)
                transform.forward *= -1;
            transform.Rotate(new Vector3(0, Random.Range(-30, 30), 0));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_canPickup && other.transform.tag == "Character")
        {
            _coinAction.AddCoinToCharacter(other.gameObject.GetComponent<Character>());
            _coinSpawner.ReturnObject(gameObject);
            _particleSystem.transform.position = other.gameObject.transform.parent.transform.position;
            _particleSystem.transform.forward = other.gameObject.transform.parent.transform.up;
            _particleSystem.Play();
        }
    }
}
