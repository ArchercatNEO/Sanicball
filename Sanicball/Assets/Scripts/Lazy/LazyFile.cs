using UnityEngine;

public class LazyFile<T> where T: Object
{
    private readonly string filePath;
    private T? file;
    public T File
    {
        get
        {
            if (file is not null) { return file; }
            file = Resources.Load<T>(filePath);
            return file;
        }
    }

    public LazyFile(string FilePath)
    {
        filePath = FilePath;
    }
}