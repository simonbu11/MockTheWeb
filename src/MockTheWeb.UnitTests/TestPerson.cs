namespace MockTheWeb.UnitTests
{
    public class TestPerson
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static TestPerson Default()
        {
            return new TestPerson
            {
                Id = 927,
                FirstName = "Peter",
                LastName = "Larpin",
            };
        }
    }
}