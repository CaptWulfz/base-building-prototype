public abstract class Singleton<T> where T : new()
{
    private static readonly T m_Instance;
    public static T Instance = m_Instance ??= new T();

    public bool IsDone { get; protected set; }

    public Singleton()
    {
        this.IsDone = false;
        Initialize();
    }

    protected virtual void Initialize()
    {
        this.IsDone = true;
    }
}
