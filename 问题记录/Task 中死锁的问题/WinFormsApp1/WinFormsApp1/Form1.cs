using System.Diagnostics;

namespace WinFormsApp1;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    #region ÎÊÌâ

    private void button1_Click(object sender, EventArgs e)
    {
        Debug.WriteLine($"1£º{nameof(button1_Click)}:{Environment.CurrentManagedThreadId}");

        DelayAsync().Wait();

        Debug.WriteLine($"2£º{nameof(button1_Click)}:{Environment.CurrentManagedThreadId}");
        Debug.WriteLine("over");
    }

    private async Task DelayAsync()
    {
        Debug.WriteLine($"3£º{nameof(DelayAsync)}:{Environment.CurrentManagedThreadId}");
        await Task.Delay(1000);
    }
    #endregion

    #region Task.Run()

    //private void button1_Click(object sender, EventArgs e)
    //{
    //    Debug.WriteLine($"1£º{nameof(button1_Click)}:{Environment.CurrentManagedThreadId}");

    //    Task.Run(() => DelayAsync()).Wait();

    //    Debug.WriteLine($"2£º{nameof(button1_Click)}:{Environment.CurrentManagedThreadId}");
    //    Debug.WriteLine("over");
    //}

    //private async Task DelayAsync()
    //{
    //    Debug.WriteLine($"3£º{nameof(DelayAsync)}:{Environment.CurrentManagedThreadId}");
    //    await Task.Delay(1000);
    //}
    #endregion

    #region ConfigureAwait(false)

    //private void button1_Click(object sender, EventArgs e)
    //{
    //    Debug.WriteLine($"1£º{nameof(button1_Click)}:{Environment.CurrentManagedThreadId}");

    //    DelayAsync().Wait();

    //    Debug.WriteLine($"2£º{nameof(button1_Click)}:{Environment.CurrentManagedThreadId}");
    //    Debug.WriteLine("over");
    //}

    //private async Task DelayAsync()
    //{
    //    Debug.WriteLine($"3£º{nameof(DelayAsync)}:{Environment.CurrentManagedThreadId}");
    //    await Task.Delay(1000).ConfigureAwait(false);
    //}
    #endregion
}
