using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whoosh : MonoBehaviour
{
    [SerializeField]
    private GameObject _follow;
    [SerializeField]
    private Material _material;
    [SerializeField]
    private int _maxSections;
    [SerializeField]
    private int _subSections;
    private int _subSectionCount;

    private int _sections;

    [SerializeField]
    private GameObject _player;

    private Mesh _mesh;

    private List<Vector3> _verts;
    private List<Vector2> _uvs;

    private List<Vector3> _moves;
    private List<Vector3> _vertPositions;

    private List<int> _tris1;
    private List<int> _tris2;

    private int _vertsLength;
    private int _uvLength;
    private int _trisLength;

    private Material _material1;
    private Material _material2;
    private Vector2 _offset;

    private Vector3 _basePos;

    private bool _startWhooshing;

    [SerializeField]
    private GameObject _test;
	void Start ()
    {
        StartWoosh();
    }

    public void StartWoosh()
    {
        _verts = new List<Vector3>();

        _uvs = new List<Vector2>();

        _tris1 = new List<int>();
        _tris2 = new List<int>();

        _moves = new List<Vector3>();
        _mesh = new Mesh();

        _vertPositions = new List<Vector3>();

        int fullVertCount = (_subSections * 4);

        for (int i = 0; i < fullVertCount / 2; i++)
        {
            _moves.Add(new Vector3(1, 0, 0));
        }

        Vector3 pos = _follow.transform.position;
        for (int i = 0; i < fullVertCount / 2; i++)
        {
            if (i > 0 && i % 2 == 0)
                pos -= _moves[i] * (1f / _subSections);
            pos += _moves[i] * (1f / _subSections);
            _vertPositions.Add(pos);
        }

        _basePos = _player.transform.position;

        gameObject.AddComponent<MeshRenderer>().material = _material;
        gameObject.AddComponent<MeshFilter>();
        _sections = _maxSections;
        _subSectionCount = 1;

        StartCoroutine(GrowMesh());
        _startWhooshing = true;
    }

    void MakeMesh()
    {
        _verts.Clear();
        _uvs.Clear();
        _tris1.Clear();
        _tris2.Clear();

        _vertsLength = _subSectionCount * 4;
        _uvLength = _subSectionCount * 4;
        _trisLength = _subSectionCount * 6;


        for (int i = 0; i < _vertsLength / 2; i++)
        {
            _verts.Add(_vertPositions[i] + _moves[i]);
        }

        for (int i = _vertsLength / 2; i < _vertsLength; i++)
        {
            _verts.Add(_vertPositions[i - _vertsLength / 2] + Vector3.down + _moves[i - _vertsLength / 2]);
        }

        float x = 0;
        for (int i = 0; i < _uvLength; i++)
        {
            if (i < _uvLength / 2)
            {
                if (i != 0 && i % 2 != 0 && i != _uvLength/2-1)
                {
                    _uvs.Add(new Vector2(x, 0));
                    _uvs.Add(new Vector2(x, 0));
                    i++;
                }
                else
                    _uvs.Add(new Vector2(x, 0));
            }
            else
            {
                if (i != _uvLength / 2 && i % 2 != 0 && i != _uvLength - 1)
                {
                    _uvs.Add(new Vector2(x, 1));
                    _uvs.Add(new Vector2(x, 1));
                    i++;
                }
                else
                    _uvs.Add(new Vector2(x, 1));
            }
            if (x == 1)
                x = 0;
            else
                x += 1f / _subSections;
        }

        int triIndex = 0;
        int endCheck = 0;
        for (int i = 0; i < _vertsLength; i+=2)
        {
            if (i >= _vertsLength)
                break;

            if (triIndex >= _trisLength)
                break;

            endCheck++;
            //Side 1
            _tris1.Add(i + _vertsLength / 2);

            _tris1.Add(i);

            _tris1.Add(i + 1);

            _tris1.Add(i + _vertsLength / 2);

            _tris1.Add(i + 1);

            _tris1.Add(i + 1 + _vertsLength / 2);
            triIndex += 6;

            //Side 2
            _tris2.Add(i + 1);

            _tris2.Add(i);

            _tris2.Add(i + _vertsLength / 2);

            _tris2.Add(i + 1 + _vertsLength / 2);

            _tris2.Add(i + 1);

            _tris2.Add(i + _vertsLength / 2);

        }
        DrawMesh();
    }

    void DrawMesh()
    {
        _tris1.AddRange(_tris2);

        _mesh.SetVertices(_verts);
        _mesh.SetUVs(0, _uvs);
        _mesh.SetTriangles(_tris1, 0);

        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
        _mesh.RecalculateTangents();

        gameObject.GetComponent<MeshFilter>().sharedMesh = _mesh;
    }

    IEnumerator GrowMesh()
    {
        while (_subSectionCount != _subSections)
        {
            MakeMesh();
            _subSectionCount++;
            yield return new WaitForSeconds(0.01f);
        }
    }

	void Update ()
    {
        if ((_basePos - _player.transform.position).magnitude > (1f / _subSections))
        {
            _moves.Insert(0, (_basePos - _player.transform.position).normalized);
            _moves.RemoveAt(_moves.Count - 1);
            _basePos = _player.transform.position;
            MakeMesh();
        }
    }
}
