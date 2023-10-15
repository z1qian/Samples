namespace MediaEncoder.Domain.Entities;

public enum ItemStatus
{
    /// <summary>
    /// 任务刚创建完成
    /// </summary>
    Ready,
    /// <summary>
    /// 开始处理
    /// </summary>
    Started,
    /// <summary>
    /// 成功
    /// </summary>
    Completed,
    /// <summary>
    /// 失败
    /// </summary>
    Failed,
}