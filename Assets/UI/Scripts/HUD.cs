using UnityEngine;
using System.Collections.Generic;

internal class HUD : MonoBehaviour
{
    [SerializeField] private List<GameObject> slots;

    private void Start()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            SetupSlot(i);
        }
        LevelManager.Instance.OnSelectedBuildable += OnSelectSlot;
        LevelManager.Instance.OnAddBuildable += SetupSlot;
        LevelManager.Instance.OnRemoveBuildable += SetupSlot;
        OnSelectSlot();
    }

    private void SetupSlot(int index) {
        BuildableItem item = LevelManager.Instance.GetBuildableAtIndex(index);
        slots[index].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = item.icon;
        slots[index].transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = item.amount.ToString();
    }

    private void OnSelectSlot() {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
        slots[LevelManager.Instance.SelectedBuildableIndex].GetComponent<UnityEngine.UI.Image>().color = Color.orange;
    }
}
