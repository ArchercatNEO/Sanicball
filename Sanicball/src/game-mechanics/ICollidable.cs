namespace Sanicball.GameMechanics;

public interface ICollidableEmmitter
{
    public int Layer { get; }
}

public interface ICollidableReciever<T> where T : ICollidableEmmitter
{
    void OnCollision(T collider);
}
