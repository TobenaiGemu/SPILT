using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whoosh : MonoBehaviour
{
    [SerializeField]
    private Transform _point1;
    [SerializeField]
    private Transform _point2;

    [SerializeField]
    private int _maxSections;
    [SerializeField]
    private int _subSections;

    private int _sections;

    [SerializeField]
    private GameObject _side1;
    [SerializeField]
    private GameObject _side2;

    [SerializeField]
    private GameObject _player;

    private Mesh _mesh1;
    private Mesh _mesh2;

    private List<Vector3> _verts;
    private List<Vector3> _prevVerts;
    private List<Vector3> _moves;
    private List<Vector2> _uvs;

    private List<int> _tris1;
    private List<int> _tris2;

    private int _vertsLength;
    private int _uvLength;
    private int _trisLength;

    private Material _material1;
    private Material _material2;
    private Vector2 _offset;

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
        _prevVerts = new List<Vector3>();
        _moves = new List<Vector3>();

        _uvs = new List<Vector2>();

        _tris1 = new List<int>();
        _tris2 = new List<int>();

        _material1 = _side1.GetComponent<Renderer>().material;
        _material2 = _side2.GetComponent<Renderer>().material;
        _sections = _maxSections;
        MakeMesh();
        _startWhooshing = true;
    }

    void MakeMesh()
    {
        _mesh1 = new Mesh();
        _mesh2 = new Mesh();

        _vertsLength = _sections * 4 + ((_subSections - 1) * _sections * 2);
        _uvLength = _sections * 4 + ((_subSections - 1) * _sections * 2);
        _trisLength = _sections * _subSections * 6;

        Vector3 pos1 = _point1.position;
        Vector3 pos2 = _point2.position;
        for (int i = 0; i < _vertsLength; i++)
        {
            if (i < _vertsLength / 2)
            {
                if (i > 0 && i % (_subSections + 1) == 0)
                    pos1 -= -_player.transform.forward * (1f / _subSections);
                _verts.Add(pos1);
                _prevVerts.Add(pos1);
                pos1 += -_player.transform.forward * (1f / _subSections);
            }
            else
            {
                if (i > _vertsLength / 2 && i % (_subSections + 1) == 0)
                    pos2 -= -_player.transform.forward * (1f / _subSections);
                _verts.Add(pos2);
                _prevVerts.Add(pos2);
                pos2 += -_player.transform.forward * (1f / _subSections);
            }
            //Instantiate(_test, _verts[i], Quaternion.identity);
        }

        float x = 0;
        for (int i = 0; i < _uvLength; i++)
        {
            if (i < _uvLength / 2)
            {
                if (i % 2 == 0)
                    _uvs.Add(new Vector2(x, 0));
                else
                    _uvs.Add(new Vector2(x, 0));
            }
            else
            {
                if (i % 2 == 0)
                    _uvs.Add(new Vector2(x, 1));
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
        for (int i = 0; i < _vertsLength; i++)
        {
            if (triIndex >= _trisLength)
                break;

            if (endCheck == 4)
            {
                endCheck = 0;
                i++;
            }
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
        _mesh1.SetVertices(_verts);
        _mesh1.SetUVs(0, _uvs);
        _mesh1.SetTriangles(_tris1, 0);

        _mesh1.RecalculateNormals();
        _mesh1.RecalculateBounds();
        _mesh1.RecalculateTangents();

        _mesh2.SetVertices(_verts);
        _mesh2.SetUVs(0, _uvs);
        _mesh2.SetTriangles(_tris2, 0);

        _mesh2.RecalculateNormals();
        _mesh2.RecalculateBounds();
        _mesh2.RecalculateTangents();

        _side1.GetComponent<MeshFilter>().sharedMesh = _mesh1;
        _side2.GetComponent<MeshFilter>().sharedMesh = _mesh2;
    }

    IEnumerator GrowMesh()
    {
        while (_sections < _maxSections)
        {
            _sections++;
            MakeMesh();
            yield return new WaitForSeconds(0.05f);
        }
    }

	void FixedUpdate ()
    {
        if (!_startWhooshing)
            return;
        _material1.SetTextureOffset("_MainTex", _offset);
        _material2.SetTextureOffset("_MainTex", _offset);
        _offset.x += 0.05f;

        Vector3 pos1 = _point1.position;
        Vector3 pos2 = _point2.position;

        _verts[0] = pos1;
        _verts[_vertsLength / 2] = pos2;

        float distance = (_verts[0] - _prevVerts[0]).magnitude;
        Debug.Log(distance);

        for (int i = 1; i < _vertsLength; i++)
        {
            if (i != _vertsLength / 2)
            {
                _verts[i] += (_prevVerts[i - 1] - _verts[i]).normalized * distance;
            }

        }
        _prevVerts = new List<Vector3>(_verts);
        DrawMesh();
    }
}
