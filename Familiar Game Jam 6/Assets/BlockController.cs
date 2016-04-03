﻿using System;
using UnityEngine;
using System.Collections;

public enum BlockContents { Nothing, Coin, Egg, Star };

public class BlockController : MonoBehaviour
{
    public BlockContents contents = BlockContents.Coin;

    public GameObject coinPrefab;
    public GameObject eggPrefab;
    public GameObject starPrefab;

    public SpriteRenderer spriteRenderer;
    public Sprite usedSprite;

    public float bumpAnimationAmount = 0.05f;
    public AnimationCurve bumpAnimationCurve = new AnimationCurve();

    public bool used = false;

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (!this.used)
        {
            if (Vector2.Dot(coll.contacts[0].normal, Vector2.up) > 0.75f)
            {
                this.used = true;
                this.spriteRenderer.sprite = this.usedSprite;
                this.StartCoroutine(this.MakeBump());
            }
        }
    }

    // Update is called once per frame
    private IEnumerator MakeBump()
    {
        Vector3 originalPosition = this.transform.localPosition;
        Vector3 targetPosition = originalPosition + (Vector3.up * this.bumpAnimationAmount);

        float normalizedTime = 0.0f, inverseTotalTime = 1.0f / 0.25f;
        while (normalizedTime < 1.0f)
        {
            this.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition,
                                                   this.bumpAnimationCurve.Evaluate(normalizedTime));

            normalizedTime += Time.deltaTime * inverseTotalTime;
            yield return null;
        }

        this.transform.localPosition = originalPosition;
    }

}