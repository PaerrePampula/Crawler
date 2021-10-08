using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTrackController : MonoBehaviour
{
    [SerializeField] ChildOnTrigger triggerToReturnCart;
    [SerializeField] GameObject _minecartPrefab;
    [SerializeField] float _minecartReturnToStartDelay = 5f;
    [SerializeField] float _minecartSpeed = 5f;
    [SerializeField] Transform trackStart;
    [SerializeField] Transform trackEnd;
    [SerializeField] Vector3 cartOffset;
    Vector3 cartDirection = Vector3.zero;
    GameObject _cart;
    private void Awake()
    {
        //Parent to this to easily prevent instantiated cart from moving to other rooms
        _cart = Instantiate(_minecartPrefab, transform);
        //Sub to trigger to return cart, and specify that the cart is need to return any collider transform
        triggerToReturnCart.delegatesOnTrigger = (collider) =>  setCartToStart();
        triggerToReturnCart.predicatesForDelegateTrigger = isCart();
        setCartToStart();
    }

    void setCartToStart()
    {
        _cart.transform.position = trackStart.position;
        cartDirection = (trackEnd.position - trackStart.position).normalized;
    }
    // Update is called once per frame
    void Update()
    {
        _cart.transform.position += (cartDirection * Time.deltaTime * _minecartSpeed);
    }
    //Set predicate to only trigger the child for the cart transform
    Func<Collider, bool> isCart() => (other) => (other.transform == _cart.transform);
}
