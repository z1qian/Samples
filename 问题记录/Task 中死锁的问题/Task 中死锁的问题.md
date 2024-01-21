# Task 中死锁的问题

## 问题

在具有同步上下文的应用程序中（`Winform` 等），有如下代码：

```c#
  private void button1_Click(object sender, EventArgs e)
  {
      Debug.WriteLine($"1：{nameof(button1_Click)}:{Environment.CurrentManagedThreadId}");

      DelayAsync().Wait();

      Debug.WriteLine($"2：{nameof(button1_Click)}:{Environment.CurrentManagedThreadId}");
      Debug.WriteLine("over");
  }

  private async Task DelayAsync()
  {
      Debug.WriteLine($"3：{nameof(DelayAsync)}:{Environment.CurrentManagedThreadId}");
      await Task.Delay(1000);
  }
```

> 1：button1_Click:1
> 3：DelayAsync:1
>
> 会造成界面卡死

原因：`DelayAsync().Wait();` 使用了 `Wait` 方法，`Wait` 方法会阻塞当前线程，直到任务完成，而当前线程是 `UI` 线程（1 号线程），而 `DelayAsync` 也在 1 号线程中执行任务，但 1 号线程不可用，导致任务一直无法完成，此时 ` DelayAsync().Wait();` 阻塞了 `UI` 线程，一直在等待任务的完成，所以会一直卡在那，造成死锁



## 解决方法

### 1.Task.Run(）

```c#
//DelayAsync().Wait();
Task.Run(() => DelayAsync()).Wait();
```

> 1：button1_Click:1
> 3：DelayAsync:9
> 2：button1_Click:1
> over

原因：`DelayAsync().Wait();` 换成了 `Task.Run(() => DelayAsync()).Wait();`，使用了 `Wait` 方法，`Wait` 方法会阻塞当前线程，直到任务完成，当前线程是 `UI` 线程（1 号线程），而 `DelayAsync` 在 9 号线程中执行任务，9 号线程可用，并没有被阻塞，任务能够顺利完成，并且切换到 `UI` 线程，也就是 1 号线程

### 2.ConfigureAwait(false)

```C#
private async Task DelayAsync()
{
    Debug.WriteLine($"3：{nameof(DelayAsync)}:{Environment.CurrentManagedThreadId}");
    //await Task.Delay(1000)；
    await Task.Delay(1000).ConfigureAwait(false);
}
```

> 1：button1_Click:1
> 3：DelayAsync:1
> 2：button1_Click:1
> over

原因：通过使用 `ConfigureAwait(false)`，告诉异步操作在完成后不需要回到 `UI` 线程中（1 号线程），这意味着异步操作可以在任何上下文中继续执行，而不会被阻止在原始的`UI` 同步上下文中