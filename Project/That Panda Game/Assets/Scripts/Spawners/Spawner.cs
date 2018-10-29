using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _object;

    [SerializeField]
    private float _spawnRate;

    [SerializeField]
    private Rect _spawnArea;

    [SerializeField]
    private int _maxObjects;

    private GameObject _planet;

    protected ObjectPool _objectPool;
    private List<GameObject> _activeObjects;

    private float _spawnTimer;
	
    void Awake()
    {
        _planet = GameObject.Find("Planet");
        _activeObjects = new List<GameObject>();
        _objectPool = new ObjectPool(_object, _maxObjects);
    }

    public void Tick()
    {
        _spawnTimer += Time.deltaTime;
        if (_activeObjects.Count < _maxObjects && _spawnTimer > _spawnRate)
        {
            _spawnTimer = 0;
            SpawnObject(_spawnArea);
        }
    }

    private void SpawnObject(Rect spawnArea)
    {
        //Get an object from the object pool and position it on a random positon on a 2d plane above the planet
        GameObject obj = _objectPool.GetObject();
        _activeObjects.Add(obj);
        Vector3 spawnPos = new Vector3(0, 0, -30);

        //Raycast towards the planet and get the Z position of the point it hits to place the object there
        RaycastHit hit;
        Physics.Raycast(spawnPos, Vector3.forward, out hit, 30, 1 << 9);
        bool hitPlanet = false;

        int fallback = 0;

        while (!hitPlanet)
        {
            spawnPos.x = Random.Range(spawnArea.x, spawnArea.x + spawnArea.width);
            spawnPos.y = Random.Range(spawnArea.y, spawnArea.y + spawnArea.height);
            Physics.Raycast(spawnPos, Vector3.forward, out hit, 30, 1 << 9);
            if (hit.collider.transform.name == "Planet")
                hitPlanet = true;
            fallback++;
            if (fallback > 50)
                return;
        }
        spawnPos.z = hit.point.z - 1;

        //Rotate the object to correctly allign it with the planet
        obj.transform.position = spawnPos;
        obj.transform.LookAt(_planet.transform);
        obj.transform.SetParent(_planet.transform, true);
        //If the object is a coin, add the GravitySim component to it
        Coin coin = obj.GetComponent<Coin>();
        if (coin != null)
        {
            coin.GetComponent<GravitySim>().enabled = true;
            coin.CanPickup();
        }
        obj.SetActive(true);
    }

    public GameObject GetCoin()
    {
        return _objectPool.GetObject();
    }

    public void Cleanup()
    {
        foreach (GameObject obj in _activeObjects)
            _objectPool.ReturnObject(obj);
        _activeObjects.Clear();
    }

    public void ReturnObject(GameObject obj)
    {
        _objectPool.ReturnObject(obj);
        _activeObjects.Remove(obj);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
