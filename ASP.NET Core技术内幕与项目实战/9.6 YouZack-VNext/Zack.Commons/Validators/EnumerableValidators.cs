namespace FluentValidation;

public static class EnumerableValidators
{
    /// <summary>
    /// 集合中没有重复元素
    /// </summary>
    public static IRuleBuilderOptions<T, IEnumerable<TItem>> NotDuplicated<T, TItem>(this IRuleBuilder<T, IEnumerable<TItem>> ruleBuilder)
    {
        return ruleBuilder.Must(p => p == null || p.Distinct().Count() == p.Count());
    }

    /// <summary>
    /// 集合中不包含指定的值comparedValue
    /// </summary>
    public static IRuleBuilderOptions<T, IEnumerable<TItem>> NotContains<T, TItem>(this IRuleBuilder<T, IEnumerable<TItem>> ruleBuilder, TItem comparedValue)
    {
        return ruleBuilder.Must(p => p == null || !p.Contains(comparedValue));
    }

    /// <summary>
    /// 集合中包含指定的值comparedValue
    /// </summary>
    public static IRuleBuilderOptions<T, IEnumerable<TItem>> Contains<T, TItem>(this IRuleBuilder<T, IEnumerable<TItem>> ruleBuilder, TItem comparedValue)
    {
        return ruleBuilder.Must(p => p != null && p.Any() && p.Contains(comparedValue));
    }
}
