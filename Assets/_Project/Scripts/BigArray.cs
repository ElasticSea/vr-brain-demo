using System.Collections;
using System.Collections.Generic;

public class BigArray<T> : IEnumerable<T>
{
    private const int ChunkSize = 1024 * 1024; // Must be 2^n in order for the fastmodule to work and under 0X7FEFFFFF
    private const int FastModulo = ChunkSize - 1;

    private readonly T[][] data;
        
    public readonly long Length;

    public BigArray(long length)
    {
        Length = length;
        var chunk = (int)(length / ChunkSize);
        var index = (int)(length % ChunkSize);

        data = new T[chunk + 1][];
        var i = 0;
        for (; i < chunk; i++)
        {
            data[i] = new T[ChunkSize];
        }
        data[i] = new T[index];
    }

    public T this[long i]
    {
        get
        {
            var chunk = (int)(i / ChunkSize);
            var index = (int)(i & FastModulo);
            return data[chunk][index];
        }
        set
        {
            var chunk = (int)(i / ChunkSize);
            var index = (int)(i & FastModulo);
            data[chunk][index] = value;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (long i = 0; i < Length; i++)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}