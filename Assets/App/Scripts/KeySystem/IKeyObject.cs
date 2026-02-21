namespace App.KeySystem
{
    public interface IKeyObject
    {
        string GetKey();
        KeyType GetKeyType();
    }
}