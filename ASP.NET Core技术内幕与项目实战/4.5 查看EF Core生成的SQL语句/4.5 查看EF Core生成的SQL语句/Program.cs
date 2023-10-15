using _4._5_查看EF_Core生成的SQL语句;

using TestDBContext ctx = new TestDBContext();

var book = ctx.Books.FirstOrDefault(b => b.Title == "算法(第4版)");