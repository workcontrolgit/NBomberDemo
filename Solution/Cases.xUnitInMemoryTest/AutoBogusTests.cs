using AutoBogus;
using AutoBogus.Conventions;
using Xunit;

namespace Cases.xUnitInMemoryTest
{
    public class AutoBogusTests
    {
        static AutoBogusTests()
        {
            AutoFaker.Configure(builder =>
            {
                builder.WithConventions();
            });
        }

        internal class User
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }

        [Fact(Skip = "For dev demonstration only")]
        public void GenerateUser()
        {
            var user = AutoFaker.Generate<User>();
            
            Assert.NotEmpty(user.FirstName);
            Assert.NotEmpty(user.LastName);
            Assert.NotEmpty(user.Email);
        }
    }
}
