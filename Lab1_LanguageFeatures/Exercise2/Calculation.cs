namespace Exercise2;

/// <summary>
/// Contains the input and result (output) of a mathematical calculation.
/// </summary>
public class Calculation
{
    public int Input { get;}
    public long Result { get; }

    public Calculation(int input, long result)
    {
        Input = input;
        Result = result;
    }
}