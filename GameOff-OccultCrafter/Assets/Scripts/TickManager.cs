using System;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public static event EventHandler<OnTickEventArgs> OnTick;
    public static event EventHandler<OnTickEventArgs> OnTick5;
    public static event EventHandler<OnTickEventArgs> OnTick10;
    public class OnTickEventArgs : EventArgs
    {
        public int tick;
    }

    private int _tick = 0;
    public int Tick {  get { return _tick; } }
    [SerializeField, Range(0.1f, 100)]
    private float tickFrequency = 5f;
    public float TickFrequency { get { return tickFrequency; } }
    private float timeBetweenTicks;
    private float tickTimer;
    
    private bool ticking;
    public bool Ticking { get { return ticking; } set { ticking = value; } }
    void Start()
    {
        timeBetweenTicks = 1 / tickFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ticking) { return; }
        tickTimer += Time.deltaTime;
        if (tickTimer >= timeBetweenTicks)
        {
            tickTimer -= timeBetweenTicks;
            _tick++;
            if (OnTick != null)
                OnTick.Invoke( this, new OnTickEventArgs { tick = _tick });
            if (OnTick5 != null && _tick % 5 == 0)
                OnTick5.Invoke( this, new OnTickEventArgs { tick = _tick });
            if (OnTick10 != null && _tick % 10 == 0)
                OnTick10.Invoke( this, new OnTickEventArgs { tick = _tick });
        }
    }
}
