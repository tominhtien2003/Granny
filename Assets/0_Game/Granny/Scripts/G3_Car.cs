using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class G3_Car : MonoBehaviour
{
    [SerializeField] private GameObject carModel;
    [SerializeField] private Collider coll;
    [SerializeField] private TextMeshProUGUI respawnText; 


    private void Start()
    {
        respawnText.gameObject.SetActive(false);
    }
    void LateUpdate()
    {
        if (respawnText.gameObject.activeInHierarchy)
        {
            respawnText.transform.rotation = Quaternion.LookRotation(
        respawnText.transform.position - Camera.main.transform.position
    );
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetOnCar();
        }
        if (other.CompareTag("G3_Bot"))
        {
            if (other.GetComponent<G3_BotCarTrigger>().IsOnCar) return;
            other.GetComponent<G3_BotCarTrigger>().IsOnCar = true;
            StartCoroutine(RespawnRoutine());
        }
    }
    public void SetOnCar()
    {
        if (G3_Manager.Instance.IsOnCar) return;

        G3_Manager.Instance.IsOnCar = true;
        G3_UIManager.Instance.SetOnCar();
        StartCoroutine(RespawnRoutine());
    }
    IEnumerator RespawnRoutine()
    {
        carModel.SetActive(false);
        coll.enabled = false;
        respawnText.gameObject.SetActive(true);

        for (int i = 5; i >= 1; i--)
        {
            respawnText.text = $"Respawn in {i}";
            yield return new WaitForSeconds(1f);
        }
        respawnText.gameObject.SetActive(false);

        carModel.SetActive(true);
        coll.enabled = true;
    }

}
