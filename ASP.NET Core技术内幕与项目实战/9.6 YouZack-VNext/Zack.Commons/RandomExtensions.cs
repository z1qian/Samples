namespace System;

public static class RandomExtensions
{
    /// <summary>
    ///  返回特定范围的随机双精度浮点数
    /// </summary>
    /// <param name="random"></param>
    /// <param name="minValue">返回的随机数的包含下界</param>
    /// <param name="maxValue">返回的随机数的独占上限，maxValue必须大于minValue</param>
    /// <returns></returns>
    public static double NextDouble(this Random random, double minValue, double maxValue)
    {
        if (minValue >= maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(minValue), "minValue cannot be greater than or equal to maxValue");
        }
        //https://stackoverflow.com/questions/65900931/c-sharp-random-number-between-double-minvalue-and-double-maxvalue
        double x = random.NextDouble();
        return x * maxValue + (1 - x) * minValue;
    }
}
