namespace Project.Scripts.GameQueue.Events
{
    
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventQueueManager : MonoBehaviour
{
    private static EventQueueManager _instance;
    public static EventQueueManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("EventQueueManager");
                _instance = go.AddComponent<EventQueueManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    
    private Queue<GameEvent> eventQueue = new Queue<GameEvent>();
    private List<GameEvent> priorityBuffer = new List<GameEvent>();
    
    // Flags
    public bool IsProcessing { get; private set; }
    public bool InputBlocked { get; private set; }
    
    // Animation tracking
    private int activeAnimations = 0;
    private bool isExecutingEvent = false; // FIX 1: Add flag to prevent re-entrance
    
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Update()
    {
        // FIX 2: Add guard to prevent re-entrance during execution
        if (!isExecutingEvent)
        {
            ProcessQueue();
        }
    }
    
    public void EnqueueEvent(GameEvent gameEvent)
    {
        // Priority handling için buffer'a ekle
        priorityBuffer.Add(gameEvent);
        
        // FIX 3: Don't flush buffer here - only flush when processing
        // This was causing events to be added to queue while processing
    }
    
    private void FlushPriorityBuffer()
    {
        if (priorityBuffer.Count == 0) return;
        
        // Priority'ye göre sırala (yüksek önce)
        var sortedEvents = priorityBuffer.OrderByDescending(e => e.Priority).ToList();
        
        foreach(var evt in sortedEvents)
        {
            eventQueue.Enqueue(evt);
        }
        
        priorityBuffer.Clear();
    }
    
    private void ProcessQueue()
    {
        // FIX 4: Check animations first
        if (activeAnimations > 0) return;
        
        // Event işleniyorsa veya queue boşsa çık
        if (IsProcessing || (eventQueue.Count == 0 && priorityBuffer.Count == 0)) return;
        
        // Buffer'daki eventleri flush et
        FlushPriorityBuffer();
        
        // Check again after flush
        if (eventQueue.Count == 0) return;
        
        // Sıradaki eventi al ve işle
        IsProcessing = true;
        InputBlocked = true;
        isExecutingEvent = true; // FIX 5: Set execution flag
        
        var currentEvent = eventQueue.Dequeue();
        
        try
        {
            Debug.Log($"Processing event: {currentEvent.GetType().Name} with priority {currentEvent.Priority}");
            currentEvent.Execute();
        }
        catch(System.Exception e)
        {
            Debug.LogError($"Event execution failed: {e.Message}");
        }
        finally
        {
            isExecutingEvent = false; // FIX 6: Clear execution flag
            IsProcessing = false;
            
            // FIX 7: Only unblock input if no animations AND no more events
            if (activeAnimations == 0 && eventQueue.Count == 0 && priorityBuffer.Count == 0)
            {
                InputBlocked = false;
            }
        }
    }
    
    // Animation tracking methods
    public void RegisterAnimationStart()
    {
        activeAnimations++;
        InputBlocked = true;
        Debug.Log($"Animation started. Active animations: {activeAnimations}");
    }
    
    public void RegisterAnimationComplete()
    {
        activeAnimations--;
        Debug.Log($"Animation completed. Active animations: {activeAnimations}");
        
        if (activeAnimations <= 0)
        {
            activeAnimations = 0;
            
            // FIX 8: Only unblock if no pending events
            if (eventQueue.Count == 0 && priorityBuffer.Count == 0)
            {
                InputBlocked = false;
            }
        }
    }
    
    public void ClearQueue()
    {
        eventQueue.Clear();
        priorityBuffer.Clear();
        IsProcessing = false;
        isExecutingEvent = false;
        activeAnimations = 0;
        InputBlocked = false;
    }
}
}