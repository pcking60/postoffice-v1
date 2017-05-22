namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeTransactionDetail : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("getRevenueStatistic",
                p => new {
                    fromDate = p.String(),
                    toDate = p.String()
                }
                ,
                @"select CAST(ts.CreatedDate as Date) as CreatedDate, sum(td.Money) as totalMoney
                from Transactions ts
                inner
                join TransactionDetails td
                on ts.ID = td.TransactionId
                inner
                join PropertyServices ps
                on ps.ID = td.PropertyServiceId
                inner
                join Services s
                on s.ID = ps.ServiceId
                where ts.CreatedDate <= CAST(@toDate as Date) and ts.CreatedDate >= CAST(@fromDate as Date)
                group by CAST(ts.CreatedDate as Date)");
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.getRevenueStatistic");
        }
    }
}
