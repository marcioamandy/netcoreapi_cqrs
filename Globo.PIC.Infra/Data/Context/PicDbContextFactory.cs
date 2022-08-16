using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Globo.PIC.Infra.Data.Context
{
	public class PicDbContextFactory : IDesignTimeDbContextFactory<PicDbContext>
	{
		public PicDbContext CreateDbContext(string[] args)
		{
			var builder = new DbContextOptionsBuilder<PicDbContext>();

            //string cs = @"Server=127.0.0.1;Port=3320;Database=db_pic;Uid=dba;Pwd=8ADBCKr7EhaX8QTT;Convert Zero Datetime=True";
            //string cs = @"Server=127.0.0.1;Port=3306;Database=db_pic;Uid=root;Pwd=q1w2e3r4!@#;Convert Zero Datetime=True";

            //string cs = @"Server=127.0.0.1;Port=3350;Database=db_pic;Uid=dba;Pwd=c9j4WNFk6hiD7Mb%;Convert Zero Datetime=True";

            string cs = @"Server=127.0.0.1;Port=3309;Database=db_pic_v2;Uid=root;Pwd=mariaflor@31;Convert Zero Datetime=True";
            //string cs = @"Server=127.0.0.1;Port=3307;Database=db_pic;Uid=root;Pwd=1Qaz2wsx@31;Convert Zero Datetime=True";

            //string cs = @"Server=127.0.0.1;Port=3320;Database=db_pic;Uid=dba;Pwd=8ADBCKr7EhaX8QTT;Convert Zero Datetime=True";
            builder.UseMySql(cs, ServerVersion.AutoDetect(cs));

            //DEVELOPER
            //builder.UseMySql(@"Server=127.0.0.1;Port=3320;Database=db_pic;Uid=dba;Pwd=8ADBCKr7EhaX8QTT;Convert Zero Datetime=True");

            // QUALIDADE
            //builder.UseMySql(@"Server=localhost;Port=3332;Database=db_pic;Uid=dba;Pwd=c9j4WNFk6hiD7Mb%;Convert Zero Datetime=True");
            //builder.UseMySql(@"Server=127.0.0.1;Port=3306;Database=db_pic;Uid=dba;Pwd=c9j4WNFk6hiD7Mb%;Convert Zero Datetime=True");

            // LOCAL
            //builder.UseMySql(@"Server=127.0.0.1;Port=3309;Database=db_pic;Uid=root;Pwd=mariaflor@31;Convert Zero Datetime=True");
            //builder.UseMySql(@"Server=127.0.0.1;Port=3306;Database=db_pic;Uid=dba_pic;Pwd=mariaflor@31;Convert Zero Datetime=True");
            return new PicDbContext(builder.Options);
		}
	}
}

