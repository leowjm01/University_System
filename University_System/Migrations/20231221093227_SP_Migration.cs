using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace University_System.Migrations
{
    /// <inheritdoc />
    public partial class SP_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames =
                        assembly.GetManifestResourceNames().
                        Where(str => str.EndsWith(".sql"));
            foreach (string resourceName in resourceNames)
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string sql = reader.ReadToEnd();
                    migrationBuilder.Sql(sql);
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
