using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildableItem
{
    public string id;           // Identificador o nombre opcional
    public GameObject prefab;   // El prefab a instanciar
    public Sprite icon;         // Imagen para mostrar en la UI
    public int amount;          // Cantidad disponible
}

public class LevelManager : MonoBehaviour
{
  public static LevelManager Instance { get; private set; }

  [Header("Inventario de Construcción")]
  [SerializeField] private List<BuildableItem> buildables = new List<BuildableItem>(3);
  
  [Header("Selección Actual")]
  [SerializeField] private int selectedBuildableIndex = 0;
  public int SelectedBuildableIndex => selectedBuildableIndex;
  public int BuildableCount => buildables != null ? buildables.Count : 0;

  public BuildableItem GetBuildableAtIndex(int index)
  {
    if (buildables != null && index >= 0 && index < buildables.Count)
      return buildables[index];
    return null;
  }

  public BuildableItem SelectedBuildable
  {
    get
    {
      if (buildables != null && selectedBuildableIndex >= 0 && selectedBuildableIndex < buildables.Count)
        return buildables[selectedBuildableIndex];
      return null;
    }
  }

  private void Awake()
  {
    Instance = this;
  }

  private void Start()
  {
    InputManager.OnArrowUpPressed += MoveUp;
    InputManager.OnArrowDownPressed += MoveDown;
  }

  public void AddBuildable(int index)
  {
    buildables[index].amount++;
  }

  public void SelectBuildable(int index)
  {
    if (index >= 0 && index < buildables.Count)
      selectedBuildableIndex = index;
  }

  public void MoveUp()
  {
    selectedBuildableIndex++;
    selectedBuildableIndex%=buildables.Count;
  }

  public void MoveDown()
  {
    selectedBuildableIndex--;
    if (selectedBuildableIndex<0)
      selectedBuildableIndex+=buildables.Count;
  }

  private void OnDestroy()
  {
    InputManager.OnArrowUpPressed -= MoveUp;
    InputManager.OnArrowDownPressed -= MoveDown;
  }
}