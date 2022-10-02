using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Bomb Instance { get; private set; }
    public List<DefusalBase> allDefusalPrefabs;

    public List<Transform> defusalTransforms;

    private List<DefusalBase> defusals = new List<DefusalBase>();

    public NumberDigits tensDigit;
    public NumberDigits onesDigit;

    public Animator explosionAnimator;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    public void Reset()
    {
        tensDigit.Clear();
        onesDigit.Clear();

        explosionAnimator.SetTrigger("Reset");

        foreach (var defusal in defusals)
            GameObject.Destroy(defusal.gameObject);
        defusals.Clear();
    }

    public void Explode()
    {
        explosionAnimator.SetTrigger("Explode");
    }

    private void Update()
    {
        var timeLeft = Countdown.Instance.TimeLeft;

        var tens = timeLeft / 10;
        var ones = timeLeft % 10;

        tensDigit.Set(tens);
        onesDigit.Set(ones);
    }

    public void Setup(List<DefusalInstruction> zList)
    {
        foreach (var instruction in zList)
        {
            SpawnDefusal(instruction);
        }
    }

    private void SpawnDefusal(DefusalInstruction zInstruction)
    {
        var prefab = allDefusalPrefabs.Find(p => p.Type == zInstruction.Type);

        var index = defusals.Count;
        var clone = GameObject.Instantiate(prefab, defusalTransforms[index]);

        clone.SetupWithInstruction(zInstruction);
        defusals.Add(clone);
    }


}
