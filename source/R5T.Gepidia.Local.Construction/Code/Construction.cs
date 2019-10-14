using System;

using R5T.Magyar.Extensions;


namespace R5T.Gepidia.Local.Construction
{
    public static class Construction
    {
        public static void SubMain()
        {
            Construction.TestGUIDs();
        }

        private static void TestGUIDs()
        {
            var guid = Guid.NewGuid();

            var uppercaseRepresentation = guid.ToStringUppercase();

            Console.WriteLine(uppercaseRepresentation);
        }
    }
}
