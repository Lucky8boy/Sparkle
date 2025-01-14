using Sparkle.csharp.entity;

namespace Sparkle.csharp.scene; 

public abstract class Scene : Disposable {

    public readonly string Name;
    
    private readonly Dictionary<int, Entity> _entities;

    private int _entityIds;
    
    public bool HasInitialized { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Scene class with the specified name.
    /// Also initializes an empty dictionary to hold entities within the scene.
    /// </summary>
    /// <param name="name">The name of the scene.</param>
    public Scene(string name) {
        this.Name = name;
        this._entities = new Dictionary<int, Entity>();
    }

    /// <summary>
    /// Used for Initializes objects.
    /// </summary>
    protected internal virtual void Init() {
        this.HasInitialized = true;
    }
    
    /// <summary>
    /// Is invoked during each tick and is used for updating dynamic elements and game logic.
    /// </summary>
    protected internal virtual void Update() {
        foreach (Entity entity in this._entities.Values) {
            entity.Update();
        }
    }
    
    /// <summary>
    /// Called after the Update method on each tick to further update dynamic elements and game logic.
    /// </summary>
    protected internal virtual void AfterUpdate() {
        foreach (Entity entity in this._entities.Values) {
            entity.AfterUpdate();
        }
    }
    
    /// <summary>
    /// Is invoked at a fixed rate of every <see cref="GameSettings.FixedTimeStep"/> frames following the <see cref="AfterUpdate"/> method.
    /// It is used for handling physics and other fixed-time operations.
    /// </summary>
    protected internal virtual void FixedUpdate() {
        foreach (Entity entity in this._entities.Values) {
            entity.FixedUpdate();
        }
    }
    
    /// <summary>
    /// Is called every tick, used for rendering stuff.
    /// </summary>
    protected internal virtual void Draw() {
        foreach (Entity entity in this._entities.Values) {
            entity.Draw();
        }
    }
    
    /// <summary>
    /// Adds an entity to the collection and initializes it.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    public void AddEntity(Entity entity) {
        entity.Id = this._entityIds++;
        entity.Init();
        
        this._entities.Add(entity.Id, entity);
    }
    
    /// <summary>
    /// Removes an entity from the collection and disposes of it.
    /// </summary>
    /// <param name="id">The ID of the entity to be removed.</param>
    public void RemoveEntity(int id) {
        this._entities[id].Dispose();
        this._entities.Remove(id);
    }
    
    /// <summary>
    /// Removes an entity from the collection and disposes of it.
    /// </summary>
    /// <param name="entity">The entity to be removed.</param>
    public void RemoveEntity(Entity entity) {
        this.RemoveEntity(entity.Id);
    }

    /// <summary>
    /// Retrieves an entity from the collection by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to be retrieved.</param>
    /// <returns>The entity associated with the specified ID.</returns>
    public Entity GetEntity(int id) {
        return this._entities[id];
    }

    /// <summary>
    /// Retrieves an array of all entities currently in the collection.
    /// </summary>
    /// <returns>An array containing all entities in the collection.</returns>
    public Entity[] GetEntities() {
        return this._entities.Values.ToArray();
    }
    
    /// <summary>
    /// Retrieves entities from the collection that have a specific tag.
    /// </summary>
    /// <param name="tag">The tag used to filter entities.</param>
    /// <returns>An enumerable of entities with the specified tag.</returns>
    public IEnumerable<Entity> GetEntitiesWithTag(string tag) {
        foreach (Entity entity in this._entities.Values) {
            if (entity.Tag == tag) {
                yield return entity;
            }
        }
    }
    
    protected override void Dispose(bool disposing) {
        if (disposing) {
            foreach (Entity entity in this._entities.Values) {
                entity.Dispose();
            }
            this._entities.Clear();
            this._entityIds = 0;
        }
    }
}