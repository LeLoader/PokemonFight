using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PokemonUI : MonoBehaviour
{
    public Pokemon pokemon;
    [SerializeField] new string name;
    [SerializeField] string maxHealth;
    [SerializeField] string attack;
    [SerializeField] string defense;
    [SerializeField] string speed;
    [SerializeField] Pokemon.PokemonType[] types;
    [SerializeField] GameObject healthBar;
    [SerializeField] Slider healthSlider;
    [SerializeField] RawImage pokemonImage;
    Image healthColorImage;
    TextMeshProUGUI[] tmps;

    public IEnumerator Init(Pokemon pokemon)
    {
        ResetAll();
        this.pokemon = pokemon;
        tmps = GetComponentsInChildren<TextMeshProUGUI>();
        pokemonImage.enabled = true;
        pokemonImage.texture = pokemon.Texture;
        yield return new WaitForSeconds(0.5f);
        foreach (var tmp in tmps)
        {
            switch (tmp.name)
            {
                case "Name":
                    name = pokemon.Name;
                    tmp.text = name;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Health":
                    maxHealth = pokemon.ScaledStats.Health.ToString();
                    tmp.text = maxHealth;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Attack":
                    attack = pokemon.ScaledStats.Attack.ToString();
                    tmp.text = attack;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Defense":
                    defense = pokemon.ScaledStats.Defense.ToString();
                    tmp.text = defense;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Speed":
                    speed = pokemon.ScaledStats.Speed.ToString();
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
        healthSlider.value = pokemon.ScaledStats.Health;
        yield return new WaitForSeconds(1.5f);
        foreach (TextMeshProUGUI tmp in tmps)
        {
            tmp.text = "";
        }
    }

    public void ResetAll()
    {
        pokemonImage.enabled = false;
        healthBar.SetActive(false);
        if (healthColorImage == null) healthColorImage = healthSlider.transform.Find("HealthBarImage").GetComponent<Image>();
        healthColorImage.color = Color.green;
        healthSlider.value = 1
;    }

    public void OnHealthChange(float health)
    {
        if (healthColorImage == null) healthColorImage = healthSlider.transform.Find("HealthBarImage").GetComponent<Image>();
        healthSlider.value = health / pokemon.ScaledStats.Health;
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
