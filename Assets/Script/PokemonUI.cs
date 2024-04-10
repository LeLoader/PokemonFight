using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Networking;

public class PokemonUI : MonoBehaviour
{
    public Pokemon pokemon;
    [SerializeField] new string name;
    [SerializeField] TextMeshProUGUI nameTMP;
    [SerializeField] GameObject healthBar;
    [SerializeField] Slider healthSlider;
    [SerializeField] RawImage pokemonImage;
    Image healthColorImage;
    TextMeshProUGUI[] tmps;

    public IEnumerator Init(Pokemon pokemon)
    {
        ResetAll();
        this.pokemon = pokemon;
        pokemonImage.enabled = true;
        pokemonImage.texture = pokemon.Texture;
        yield return new WaitForSeconds(0.5f);
        name = pokemon.Name;
        nameTMP.text = name;
        yield return new WaitForSeconds(0.5f);
        healthBar.SetActive(true);
        healthSlider.value = pokemon.ScaledStats.Health;
        yield return new WaitForSeconds(1.5f);
        nameTMP.text = "";
    }

    public void ResetAll()
    {
        pokemonImage.enabled = false;
        healthBar.SetActive(false);
        if (healthColorImage == null) healthColorImage = healthSlider.transform.Find("HealthBarImage").GetComponent<Image>();
        healthColorImage.color = Color.green;
        healthSlider.value = 1
;
    }

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
