using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _4._6_关系配置;

internal class CommentConfig : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("T_Comments");
        //Comment类的Article属性是指向Article实体类型的
        builder.HasOne(c => c.Article)
            //一个Article对应多个Comment，
            //并且在Article中可以通过Comments属性访问到相关的Comment对象。
            .WithMany(a => a.Comments)
            //Comment中的Article属性是不可以为空的
            .IsRequired()
            //指定外键列
            .HasForeignKey(c => c.ArticleId);

        builder.Property(c => c.Message).IsRequired().IsUnicode();
    }
}