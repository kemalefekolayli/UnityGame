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
        ProcessQueue();
    }
    
    public void EnqueueEvent(GameEvent gameEvent)
    {
        // Priority handling için buffer'a ekle
        priorityBuffer.Add(gameEvent);
        
        // Priority'ye göre sırala ve queue'ya ekle
        if (!IsProcessing)
        {
            FlushPriorityBuffer();
        }
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
        // Event işleniyorsa veya queue boşsa çık
        if (IsProcessing || eventQueue.Count == 0) return;
        
        // Buffer'daki eventleri flush et
        FlushPriorityBuffer();
        
        // Sıradaki eventi al ve işle
        IsProcessing = true;
        InputBlocked = true;
        
        var currentEvent = eventQueue.Dequeue();
        
        try
        {
            currentEvent.Execute();
        }
        catch(System.Exception e)
        {
            Debug.LogError($"Event execution failed: {e.Message}");
        }
        finally
        {
            IsProcessing = false;
            
            // Eğer animasyon yoksa input'u aç
            if (activeAnimations == 0)
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
    }
    
    public void RegisterAnimationComplete()
    {
        activeAnimations--;
        if (activeAnimations <= 0)
        {
            activeAnimations = 0;
            InputBlocked = false;
        }
    }
    
    public void ClearQueue()
    {
        eventQueue.Clear();
        priorityBuffer.Clear();
        IsProcessing = false;
    }
}
}