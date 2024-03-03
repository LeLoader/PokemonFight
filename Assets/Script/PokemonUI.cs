using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;
using DG.Tweening;

public class PokemonUI : MonoBehaviour
{
    public Pokemon pokemon;
    [SerializeField] new string name;
    [SerializeField] string baseHealth;
    [SerializeField] string attack;
    [SerializeField] string defense;
    [SerializeField] string speed;
    [SerializeField] Pokemon.PokemonType[] types;
    [SerializeField] GameObject healthBar;
    [SerializeField] Slider healthSlider;
    RawImage pokemonImage;
    Image healthColorImage;
    TextMeshProUGUI[] tmps;

    public IEnumerator Init(Pokemon pokemon)
    {
        ResetAll();
        this.pokemon = pokemon;
        tmps = GetComponentsInChildren<TextMeshProUGUI>();
        pokemonImage = GetComponentInChildren<RawImage>();
        pokemonImage.enabled = true;
        pokemonImage.texture = pokemon.GetTexture();
        yield return new WaitForSeconds(0.5f);
        foreach (var tmp in tmps)
        {
            switch (tmp.name)
            {
                case "Name":
                    name = pokemon.GetName();
                    tmp.text = name;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Health":
                    baseHealth = pokemon.GetHealth().ToString();
                    tmp.text = baseHealth;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Attack":
                    attack = pokemon.GetAttack().ToString();
                    tmp.text = attack;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Defense":
                    defense = pokemon.GetDefense().ToString();
                    tmp.text = defense;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Speed":
                    speed = pokemon.GetSpeed().ToString();
                    tmp.text = speed;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Types":
                    types = pokemon.GetType();
                    if (types[1] == Pokemon.PokemonType.None) tmp.text = types[0].ToString();
                    else tmp.text = types[0].ToString() + ", " + types[1].ToString();
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }
        yield return new WaitForSeconds(0.5f);
        healthBar.SetActive(true);
        healthSlider.value = pokemon.GetMaxHealth();
        yield return new WaitForSeconds(1.5f);
        foreach (TextMeshProUGUI tmp in tmps)
        {
            tmp.text = "";
        }
    }

    private void ResetAll()
    {
        healthBar.SetActive(false);
        if (healthColorImage == null) healthColorImage = healthSlider.transform.Find("HealthBarImage").GetComponent<Image>();
        healthColorImage.color = Color.green;
        healthSlider.value = 1
;    }

    public void OnHealthChange(float health)
    {
        if (healthColorImage == null) healthColorImage = healthSlider.transform.Find("HealthBarImage").GetComponent<Image>();
        healthSlider.value = health / pokemon.GetMaxHealth();
        switch (healthSlider.value)
        {
            case > 0.5f:
                healthColorImage.color = Color.green;
                break;
            case > 0.2f:
                healthColorImage.color = Color.yellow;
                break;
            case > 0:
                healthColorImage.color = Color.red;
                break;
        }
    }

    public void DamagedAnimation()
    {
        pokemonImage.rectTransform.DOShakePosition(1f, 10f);
        //DOTween.Play();
    }
}
