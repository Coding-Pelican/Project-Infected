using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour {
    public Vector2 offset = new Vector2(-0.2f, -0.2f);

    SpriteRenderer sprRndCaster;
    SpriteRenderer sprRndShadow;

    Transform transCaster;
    Transform transShadow;

    public Material shadowMaterial;
    public Color shadowColor;

    private void Start() {
        transCaster = transform;
        transShadow = new GameObject().transform;
        transShadow.parent = transCaster;
        transShadow.gameObject.name = "shadow";
        transShadow.localRotation = Quaternion.identity;

        sprRndCaster = GetComponent<SpriteRenderer>();
        sprRndShadow = transShadow.gameObject.AddComponent<SpriteRenderer>();

        sprRndShadow.material = shadowMaterial;
        sprRndShadow.color = shadowColor;
        sprRndShadow.sortingLayerName = sprRndCaster.sortingLayerName;
        sprRndShadow.sortingOrder = sprRndCaster.sortingOrder - 1;

        Color alpha = sprRndShadow.color;
        alpha.a = 0.5f;
        sprRndShadow.color = alpha;
    }

    private void LateUpdate() {
        transShadow.position = new Vector2(transCaster.position.x + offset.x, transCaster.position.y + offset.y);

        sprRndShadow.sprite = sprRndCaster.sprite;
    }
}
