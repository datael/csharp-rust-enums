using System;

public struct RustEnum<TLargestVariant>
    where TLargestVariant : unmanaged
{

    private Type StoredType;
    private TLargestVariant StoredValue;

    public static RustEnum<TLargestVariant> Of<TVariant>(TVariant variant)
        where TVariant : unmanaged
    {
        var instance = new RustEnum<TLargestVariant>();

        instance.StoredType = typeof(TVariant);

        unsafe {
            var buffer = (int*)&instance.StoredValue;

            var ptr = (int*)&variant;
            for (int i = 0; i < sizeof(TVariant); i++)
            {
                *(buffer + i) = *(ptr + i);
            }
        }

        return instance;
    }

    private static unsafe TVariant As<TVariant>(TLargestVariant original)
        where TVariant : unmanaged
    {
        TVariant readValue = default;
        int* outPtr = (int*)&readValue;
        int* inPtr = (int*)&original;
        for (int i = 0; i < sizeof(TVariant); i++)
        {
            *(outPtr + i) = *(inPtr + i);
        }
        return readValue;
    }

    public bool Is<TVariant>(out TVariant variant)
        where TVariant : unmanaged
    {
        if (typeof(TVariant) != StoredType)
        {
            variant = default;
            return false;
        }

        variant = As<TVariant>(this.StoredValue);
        return true;
    }

}
