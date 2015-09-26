public abstract class BaseSteering<T>
{
    private T npc;

    public T NPC
    {
        get { return npc; }
    }

    protected BaseSteering(T npc)
    {
        this.npc = npc;
    }
}
