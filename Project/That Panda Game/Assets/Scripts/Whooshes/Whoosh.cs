using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whoosh : MonoBehaviour
{

    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private Material _material;
    [SerializeField]
    private int _subSections;
    private int _subSectionCount;

    private Mesh _mesh;

    private List<Vector3> _verts;
    private List<Vector2> _uvs;
    private List<int> _tris;

    private List<Vector3> _moves;
    private Vector3 _basePos;
    private Vector3 _aimPoint;

    private float _xUv;

    private bool _whooshing;

    [SerializeField]
    private GameObject _test;
	void Start ()
    {

    }

    public void StartWoosh()
    {
        _whooshing = true;
        _verts = new List<Vector3>();
        _uvs = new List<Vector2>();
        _tris = new List<int>();

        _moves = new List<Vector3>();
        _mesh = new Mesh();

        _basePos = _player.transform.position;
        _aimPoint = _player.transform.position + -_player.transform.forward;
        gameObject.AddComponent<MeshRenderer>().material = _material;
        gameObject.AddComponent<MeshFilter>();
        _subSectionCount = 0;
        _moves.Add(_player.transform.forward);
        MakeMesh();
        StartCoroutine(GrowMesh());
    }

    void MakeMesh()
    {
        if (_subSectionCount == _subSections)
            return;

        Vector3 dir = -_player.transform.forward;
        if (_verts.Count == 0)
        {
            Vector3 pos1 = _player.transform.position;
            Vector3 pos2 = pos1 + -_player.transform.up;
            _verts.Add(pos1);
            _verts.Add(pos2);
            _verts.Add(pos1 + dir * (1f / _subSections));
            _verts.Add(pos2 + dir * (1f / _subSections));
        }
        else
        {
            Vector3 pos1 = _verts[_verts.Count - 2];
            Vector3 pos2 = _verts[_verts.Count - 1];
            dir = (pos1 - _verts[_verts.Count - 4]).normalized;
            _verts.Add(pos1);
            _verts.Add(pos2);
            _verts.Add(pos1 + dir * (1f / _subSections));
            _verts.Add(pos2 + dir * (1f / _subSections));
        }

        _uvs.Add(new Vector2(_xUv, 0));
        _uvs.Add(new Vector2(_xUv, 1));
        _xUv += 1f / _subSections;
        _uvs.Add(new Vector2(_xUv, 0));
        _uvs.Add(new Vector2(_xUv, 1));

        _tris.Add(_verts.Count - 3);
        _tris.Add(_verts.Count - 4);
        _tris.Add(_verts.Count - 2);

        _tris.Add(_verts.Count - 3);
        _tris.Add(_verts.Count - 2);
        _tris.Add(_verts.Count - 1);

        _tris.Add(_verts.Count - 2);
        _tris.Add(_verts.Count - 4);
        _tris.Add(_verts.Count - 3);

        _tris.Add(_verts.Count - 1);
        _tris.Add(_verts.Count - 2);
        _tris.Add(_verts.Count - 3);

        _subSectionCount++;
        _moves.Add(-dir);
        DrawMesh();
    }

    private int _num;
    void MoveVerts()
    {
        foreach (Vector3 dir in _moves)
            Debug.Log(dir);
        int vertIndex = 0;
        for (int i = 0; i < _subSectionCount + 1; i++)
        {
            _verts[vertIndex] += _moves[i] * (1f / _subSections);
            _verts[vertIndex + 1] += _moves[i] * (1f / _subSections);
            if (i != 0 && i != _subSectionCount)
            {
                _verts[vertIndex + 2] += _moves[i] * (1f / _subSections);
                _verts[vertIndex + 3] += _moves[i] * (1f / _subSections);
                vertIndex += 2;
            }
            vertIndex += 2;
        }
        _num++;
        DrawMesh();
    }

    void DrawMesh()
    {
        _mesh.SetVertices(_verts);
        _mesh.SetUVs(0, _uvs);
        _mesh.SetTriangles(_tris, 0);

        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
        _mesh.RecalculateTangents();

        gameObject.GetComponent<MeshFilter>().sharedMesh = _mesh;
    }

    IEnumerator GrowMesh()
    {
        int count = 0;
        while (count < _subSections)
        {
            MakeMesh();
            count++;
            yield return new WaitForSeconds(0.1f);
        }
    }

	void Update ()
    {
        if (!_whooshing)
            return;
        if ((_basePos - _player.transform.position).magnitude > (1f / _subSections))
        {
            _moves.Insert(0, (_player.transform.position - _basePos).normalized);
            _moves.RemoveAt(_moves.Count - 1);
            _basePos = _player.transform.position;
            MoveVerts();
        }
    }
}
