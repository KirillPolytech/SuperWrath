using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Records : MonoBehaviour
{
    [SerializeField]private GameObject _recordPrefab;

    private GameObject __group;
    private LinkedList<RecordData> _userData;
    private GameObject _tempRecord;

    RecordData _temp;
    private RectTransform _tempRectTransform;
    private void Start()
    {
        __group = transform.GetChild(0).GetChild(0).gameObject;
        _tempRectTransform = __group.GetComponent<RectTransform>();
        _tempRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

        _userData = AccesToDataBase.GetDataFromRecords();

        for (int i = 0; i < _userData.Count; i++)
        {
            _temp = _userData.ElementAt(i);
            _tempRecord = Instantiate(_recordPrefab);
            _tempRecord.transform.SetParent(__group.transform);
            _tempRecord.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Place {i + 1}";
            _tempRecord.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Time: " + _temp.RecordTime + "s";
            _tempRecord.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Date: " + $"{_temp.RecordDate}";

            _tempRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _tempRectTransform.rect.height + 100);
        }
        _lastDataCount = _userData.Count;
    }
    private float _delay = 0f, _maxDelay = 25f;
    private int _lastDataCount = 0;
    private void Update()
    {
        if (_delay > _maxDelay)
        {
            _userData = AccesToDataBase.GetDataFromRecords();
            for (int i = _lastDataCount; i < _userData.Count; i++)
            {
                _temp = _userData.ElementAt(i);
                _tempRecord = Instantiate(_recordPrefab);
                _tempRecord.transform.SetParent(__group.transform);
                _tempRecord.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Place {i + 1}";
                _tempRecord.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Time: " + _temp.RecordTime + "s";
                _tempRecord.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Date: " + $"{_temp.RecordDate}";
                _lastDataCount = _userData.Count;

                _tempRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _tempRectTransform.rect.height + 100);
            }
            _delay = 0f;
            Debug.Log("Updated");
        }
        _delay += Time.fixedDeltaTime;
    }
}
