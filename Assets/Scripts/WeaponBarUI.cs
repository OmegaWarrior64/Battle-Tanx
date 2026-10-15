using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBarUI : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private Image fill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        player.BulletShot += time =>
        {
            fill.fillAmount = 0;
            StopAllCoroutines();
            StartCoroutine(Reload(1 / time));  
        };
    }

    private IEnumerator Reload(float rate)
    {
        yield return new WaitWhile(() =>
        {
            fill.fillAmount += rate * Time.deltaTime;
            return fill.fillAmount < 1;
        });
    }
}