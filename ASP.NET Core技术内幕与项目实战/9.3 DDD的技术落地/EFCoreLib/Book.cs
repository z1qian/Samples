using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreLib;

[Table("T_Books")]
public class Book
{
    public long Id { get; set; }          //主键

    private string? title;                //标题
    public string? Title
    {
        get
        {
            Console.WriteLine("get被调用");
            return title;
        }
        set
        {
            Console.WriteLine("set被调用");
            this.title = value;
        }
    }    

    public DateTime PubTime { get; set; } //发布日期
    public double Price { get; set; }     //单价
    public string? AuthorName { get; set; }//作者名字

    public string? Remark { get; set; }
}