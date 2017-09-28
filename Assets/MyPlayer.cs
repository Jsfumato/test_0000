using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> nodes = new List<GameObject>();
    public int totalCount = 50;
    public float defaultSpeed = 1.0f;
    public float boostedSpeed = 1.8f;
    public float turnSpeed = 1.0f;
    public float dirDegree = 0.0f;

    private float _speed;
    private List<Vector3> _lastPosList = new List<Vector3>();
    private float _nodeDistance = 1.0f;

	void Awake () {
        prefab.SetActive(false);
        _speed = defaultSpeed;
        //_nodeDistance = prefab.GetComponent<RectTransform>().rect.height;

        for (int i = 0; i < totalCount; ++i)
        {
            GameObject node = Instantiate<GameObject>(prefab, prefab.transform.parent);
            node.transform.parent.SetSiblingIndex(1);
            node.SetActive(true);
            if (nodes.Count <= 0)
                node.transform.localPosition = Vector3.zero;
            else
                node.transform.position = nodes.Last().transform.position - new Vector3(0.0f, 5.0f, 0.0f);

            _lastPosList.Add(node.transform.position);
            nodes.Add(node);
        }

        _nodeDistance = (nodes[0].transform.position - nodes[1].transform.position).magnitude;
    }
	
	void Update () {
        float _time = Time.deltaTime;
        Vector3 moved = Vector3.zero;

        for (int i = 0; i < nodes.Count; ++i)
        {
            if (i == 0)
            {
                Vector3 vec = Quaternion.AngleAxis(dirDegree, Vector3.forward) * Vector3.up;
                moved = vec.normalized * _time * _speed;
                nodes[0].transform.position = nodes[0].transform.position + moved;
                _lastPosList.Insert(0, nodes[0].transform.position);
            }
            else
            {
                Vector3 dir = nodes[i - 1].transform.position - nodes[i].transform.position;
                if (dir.magnitude < _nodeDistance)
                    continue;

                if (dir.magnitude > moved.magnitude)
                    nodes[i].transform.position = nodes[i].transform.position + dir.normalized * moved.magnitude;
                
                //nodes[i].transform.position = _lastPosList[i];
            }
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
            dirDegree -= turnSpeed * Time.deltaTime;
            dirDegree = dirDegree % 360;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            dirDegree += turnSpeed * Time.deltaTime;
            dirDegree = dirDegree % 360;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            _speed = boostedSpeed;
        }else if(Input.GetKeyUp(KeyCode.Space))
        {
            _speed = defaultSpeed;
        }
	}
}