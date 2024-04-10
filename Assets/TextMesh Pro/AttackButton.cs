using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button button;
    public Move move;

    private void Awake()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        button.onClick.AddListener(() => Attack(move));
    }
    
    public void Init(Move move)
    {
        this.move = move;
        text.text = $"{move.Name.FirstCharacterToUpper()} ACC:{move.Accuracy} POW:{move.Power} MOV.TYPE:{move.MoveType} ELE.TYPE:{move.ElementalType}";
    }

    private void Attack(Move move)
    {
        PokemonDatabaseManager.Instance.pokemonFight.OnAttackButtonDown(move);
    }
}
