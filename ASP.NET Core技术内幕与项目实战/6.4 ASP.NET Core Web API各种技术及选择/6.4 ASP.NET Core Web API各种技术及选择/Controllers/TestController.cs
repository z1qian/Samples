using Microsoft.AspNetCore.Mvc;

namespace _6._4_ASP.NET_Core_Web_API各种技术及选择.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : ControllerBase
{
    //[HttpGet("{id}")]
    //public ActionResult<Person> GetPerson(int id)
    //{
    //    if (id <= 0)
    //        return BadRequest("id必须是正数");
    //    else if (id == 1)
    //        return new Person(1, "杨中科", 18);
    //    else if (id == 2)
    //        return new Person(2, "Zack", 8);
    //    else
    //        return NotFound("人员不存在");
    //}

    [HttpGet("{id}")]
    //public Person GetPerson(int id)
    public ActionResult<Person> GetPerson(int id)
    //public async Task<ActionResult<Person>> GetCountryCode(string countryName)
    {
        if (id <= 0)
            return BadRequest(new ErrorInfo(1, "id必须是正数"));
        else if (id == 1)
            return new Person(1, "杨中科", 18);
        else if (id == 2)
            return new Person(2, "Zack", 8);
        else
            return NotFound(new ErrorInfo(2, "人员不存在"));
    }

    //从URL中获取参数
    [HttpGet("school/{schoolName}/class/{classNo}")]
    public ActionResult<Student[]> GetAll(string schoolName, [FromRoute(Name = "classNo")] string classNum)
    {
        return null;
    }

    //从QueryString中获取参数
    //?pageNum=8&pSize=10
    [HttpGet]
    public ActionResult<Student[]> GetAll2([FromQuery] string pageNum, [FromQuery(Name = "pSize")] int pageSize)
    {
        return null;
    }

    //多种方式混用
    // /Students/GetAll3/school/MIT/class/A001?pageNum=8&pSize=10
    [HttpGet("school/{schoolName}/class/{classNo}")]
    public ActionResult<Student[]> GetAll3(string schoolName,
        [FromRoute(Name = "classNo")] string classNum,
         [FromQuery] string pageNum, [FromQuery(Name = "pSize")] int pageSize)
    {
        return null;
    }

    [HttpPost]
    public ActionResult<bool> AddNew(Student s)
    {
        return true;
    }

    //POST、PUT等请求除了可以通过URL传递数据，也可以通过报文体传递数据
    //混合使用
    // /Students/AddNew2/classId/8这个路径提交请求报文体为{"name":"yzk","age":"18"}的POST请求即可
    //要设定请求报文头中的Content-Type为application/JSON，而且请求报文体必须是合法的JSON格式，否则服务器会报错。
    [HttpPost("classId/{classId}")]
    public ActionResult<long> AddNew2(long classId, Student s)
    {
        return null;
    }
}
