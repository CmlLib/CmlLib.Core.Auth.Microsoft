// THIS FILE SHOULD NOT BE COMPILED!
// I can't debug unit test project in some reason. (maybe my cloud development environment setting has some problem)
// So whenever I need to debug unit test I just change project type and manually call unit test method
// It works

#if !TEST_SDK

//Console.WriteLine(DateTime.MinValue.CompareTo(DateTime.MinValue));
var test = new XboxAuthNet.Game.Test.XboxGameAccountTest();
test.TestCompareToEqual();

#endif