* 单元测试最好不要访问数据库

* 从内到外测试（models->service->appservice）

* 先创建单元测试，再创建要测试的方法（先创建领域模型的单元测试，再根据测试创建领域模型）

* `testc`

  ```c#
  [TestClass]
  public class MyTestClass
  {
  }
  ```

* `testm`

  ```c#
  [TestMethod]
  public void MyTestMethod()
  {
  }
  ```

* 测试项目要引用被测试的项目

* 测试项目的路径要和被测试项目的路径相同：Senparc.Xncf.AccountsTests\Domain\Models\AccountOperationLogTests.cs

* 